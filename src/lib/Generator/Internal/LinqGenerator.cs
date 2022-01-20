// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.ExpressionTree;
using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Parser;
using NoRealm.ExpressionLite.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BinaryExpression = NoRealm.ExpressionLite.ExpressionTree.BinaryExpression;
using ConstantExpression = NoRealm.ExpressionLite.ExpressionTree.ConstantExpression;
using LinqExpression = System.Linq.Expressions.Expression;
using UnaryExpression = NoRealm.ExpressionLite.ExpressionTree.UnaryExpression;

namespace NoRealm.ExpressionLite.Generator
{
    internal class LinqGenerator
    {
        private readonly IParsingResult result;
        private readonly Type inType;
        private readonly Type retType;

        private readonly ParameterExpression inParam;

        private readonly IReadOnlyDictionary<NodeType, ExpressionType> binaryMap;

        public LinqGenerator(IParsingResult result, Type inType, Type retType)
        {
            this.result = result;
            this.inType = inType;
            this.retType = retType;

            if (inType != null)
                inParam = LinqExpression.Parameter(inType);

            binaryMap = GenerateMap();
        }

        public LambdaExpression Generate()
        {
            var body = Generate(result.Expression);

            if (body.Type != retType)
                body = LinqExpression.Convert(body, retType);

            if (inType == null)
                return LinqExpression.Lambda(typeof(Func<>).MakeGenericType(retType), body);

            return LinqExpression.Lambda(typeof(Func<,>).MakeGenericType(inType, retType), body, inParam);
        }

        private LinqExpression Generate(IExpression expression)
        {
            return expression switch
            {
                ArrayExpression ae => GenerateArray(ae),
                BinaryExpression be => GenerateBinary(be),
                ConstantExpression ce => GenerateConstant(ce),
                IdentifierExpression ie => GenerateIdentifier(ie),
                UnaryExpression ue => GenerateUnary(ue),
                IfExpression ife => GenerateIf(ife),
                _ => throw new ArgumentException("unknown expression type")
            };
        }

        private LinqExpression GenerateUnary(UnaryExpression expression)
        {
            var exp = Generate(expression.Operand);

            return expression.NodeType switch
            {
                NodeType.UnaryMinus => LinqExpression.Negate(exp),
                NodeType.UnaryPlus => exp,
                NodeType.LogicNot => LinqExpression.Not(exp),
                _ => throw new ArgumentException("unknown unary expression")
            };
        }

        private LinqExpression GenerateBinary(BinaryExpression expression)
        {
            var left = Generate(expression.Left);
            var right = Generate(expression.Right);

            if (binaryMap.TryGetValue(expression.NodeType, out var exType))
                return LinqExpression.MakeBinary(exType, left, right);

            if (expression.NodeType == NodeType.Append)
            {
                var methodDef = typeof(string).GetMethod(nameof(string.Concat), new[] {typeof(string), typeof(string)});
                return LinqExpression.Call(methodDef, left, right);
            }

            if (expression.NodeType is NodeType.Have or NodeType.NotHave)
            {
                var methodDef = typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)});
                return LinqExpression.Call(left, methodDef, right);
            }

            if (expression.NodeType is NodeType.In or NodeType.NotIn)
            {
                var methodDef = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);

                return LinqExpression.Call(methodDef.MakeGenericMethod(expression.Left.StaticType), right, left);
            }

            throw new ArgumentException("unknown binary expression");
        }

        private LinqExpression GenerateIf(IfExpression expression)
            => LinqExpression.Condition(Generate(expression.Condition), Generate(expression.TruePart), Generate(expression.FalsePart));

        private LinqExpression GenerateArray(ArrayExpression expression)
            => LinqExpression.NewArrayInit(expression.StaticType, expression.Operand.Select(Generate));

        private LinqExpression GenerateConstant(ConstantExpression expression)
            => LinqExpression.Constant(expression.Value, expression.StaticType);

        private LinqExpression GenerateIdentifier(IdentifierExpression expression)
        {
            var info = result.Names[expression.Id].Name;

            if (info.NameType == NameType.Plain)
                return LinqExpression.Constant(info.Value, info.StaticType);

            if (info.NameType == NameType.LinqExpression)
            {
                var f = new ReplaceLinqParameter(inParam).Fix((LambdaExpression) info.Value);

                if (f.ReturnType.IsNumber())
                    return f.ReturnType == KnownTypes.Number ? f.Body : LinqExpression.Convert(f.Body, KnownTypes.Number);
                else
                    return f.Body;
            }

            if (info.NameType == NameType.Member)
            {
                var member = (MemberInfo) info.Value;
                var (isStatic, type) = member.GetMemberInfo();

                var exp = LinqExpression.MakeMemberAccess(isStatic ? null : inParam, member);

                if (type.IsNumber())
                    return type == KnownTypes.Number ? exp : LinqExpression.Convert(exp, KnownTypes.Number);
                else
                    return exp;
            }

            throw new ArgumentException("unknown identifier name type");
        }

        private IReadOnlyDictionary<NodeType, ExpressionType> GenerateMap()
        {
            return new Dictionary<NodeType, ExpressionType>
            {
                [NodeType.Multiply] = ExpressionType.Multiply,
                [NodeType.Divide] = ExpressionType.Divide,
                [NodeType.Reminder] = ExpressionType.Modulo,

                [NodeType.Add] = ExpressionType.Add,
                [NodeType.Subtract] = ExpressionType.Subtract,

                [NodeType.LogicAnd] = ExpressionType.AndAlso,
                [NodeType.LogicOr] = ExpressionType.OrElse,

                [NodeType.GreaterThan] = ExpressionType.GreaterThan,
                [NodeType.LessThan] = ExpressionType.LessThan,

                [NodeType.GreaterThanOrEquals] = ExpressionType.GreaterThanOrEqual,
                [NodeType.LessThanOrEquals] = ExpressionType.LessThanOrEqual,

                [NodeType.Equals] = ExpressionType.Equal,
                [NodeType.NotEquals] = ExpressionType.NotEqual
            };
        }

        private sealed class ReplaceLinqParameter : ExpressionVisitor
        {
            private readonly ParameterExpression param;

            public ReplaceLinqParameter(ParameterExpression param)
                => this.param = param;

            public LambdaExpression Fix(LambdaExpression expression)
                => LinqExpression.Lambda(expression.Type, Visit(expression.Body), param);

            protected override Expression VisitParameter(ParameterExpression node)
                => param;
        }
    }
}
