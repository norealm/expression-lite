// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.ExpressionTree;
using NoRealm.ExpressionLite.Token;
using System;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    internal sealed class GlobalOptimizer : ExpressionVisitorBase
    {
        private readonly ParserOptions options;
        private IExpression root;

        public GlobalOptimizer(ParserOptions options, IExpression root)
        {
            this.options = options;
            this.root = root;
        }

        public IExpression Optimize()
        {
            if (!options.OptimizeConstantOperations || options.OptimizationLevels < 1)
                return root;

            for (var i = 0; i < options.OptimizationLevels; ++i)
                root = Visit(root);

            return root;
        }

        protected override IExpression OnVisitUnary(UnaryExpression expression)
        {
            base.OnVisitUnary(expression);

            if (expression.NodeType == NodeType.UnaryPlus || expression.Operand is not ConstantExpression ce)
                return expression;

            return expression.NodeType == NodeType.UnaryMinus ?
                Expressions.Constant(((decimal) ce.Value) * -1) :
                Expressions.Constant(!(bool) ce.Value);
        }

        protected override IExpression OnVisitBinary(BinaryExpression expression)
        {
            base.OnVisitBinary(expression);

            if (expression.NodeType is NodeType.In or NodeType.NotIn)
                return OptimizeInNotIn(expression);

            if (expression.Left is not ConstantExpression ce1 || expression.Right is not ConstantExpression ce2)
                return expression;

            switch (expression.NodeType)
            {
                case NodeType.LogicAnd:
                    return Expressions.Constant((bool) ce1.Value && (bool) ce2.Value);

                case NodeType.LogicOr:
                    return Expressions.Constant((bool) ce1.Value || (bool) ce2.Value);

                case NodeType.Append:
                    return Expressions.Constant((string) ce1.Value + (string) ce2.Value);

                case NodeType.Add:
                    return Expressions.Constant((decimal) ce1.Value + (decimal) ce2.Value);

                case NodeType.Subtract:
                    return Expressions.Constant((decimal) ce1.Value - (decimal) ce2.Value);

                case NodeType.Multiply:
                    return Expressions.Constant((decimal) ce1.Value * (decimal) ce2.Value);

                case NodeType.Divide:
                    CheckDivisionByZero(expression.Right);
                    return Expressions.Constant((decimal) ce1.Value / (decimal) ce2.Value);

                case NodeType.Reminder:
                    CheckDivisionByZero(expression.Right);
                    return Expressions.Constant((decimal) ce1.Value % (decimal) ce2.Value);

                case NodeType.GreaterThan:
                    return Expressions.Constant((decimal) ce1.Value > (decimal) ce2.Value);

                case NodeType.GreaterThanOrEquals:
                    return Expressions.Constant((decimal) ce1.Value >= (decimal) ce2.Value);

                case NodeType.LessThan:
                    return Expressions.Constant((decimal) ce1.Value < (decimal) ce2.Value);

                case NodeType.LessThanOrEquals:
                    return Expressions.Constant((decimal) ce1.Value <= (decimal) ce2.Value);

                case NodeType.Have:
                    return Expressions.Constant(((string) ce1.Value).Contains((string) ce2.Value));

                case NodeType.NotHave:
                    return Expressions.Constant(!((string) ce1.Value).Contains((string) ce2.Value));

                case NodeType.Equals:
                    bool eResult;

                    if (ce1.StaticType == KnownTypes.String)
                        eResult = (string) ce1.Value == (string) ce2.Value;
                    else if (ce1.StaticType == KnownTypes.Number)
                        eResult = (decimal) ce1.Value == (decimal) ce2.Value;
                    else if (ce1.StaticType == KnownTypes.DateTime)
                        eResult = (DateTime) ce1.Value == (DateTime) ce2.Value;
                    else
                        eResult = (bool) ce1.Value == (bool) ce2.Value;

                    return Expressions.Constant(eResult);

                case NodeType.NotEquals:
                    bool neResult;

                    if (ce1.StaticType == KnownTypes.String)
                        neResult = (string) ce1.Value != (string) ce2.Value;
                    else if (ce1.StaticType == KnownTypes.Number)
                        neResult = (decimal) ce1.Value != (decimal) ce2.Value;
                    else if (ce1.StaticType == KnownTypes.DateTime)
                        neResult = (DateTime) ce1.Value != (DateTime) ce2.Value;
                    else
                        neResult = (bool) ce1.Value != (bool) ce2.Value;

                    return Expressions.Constant(neResult);

                default:
                    return expression;
            }
        }

        public override IExpression VisitIf(IfExpression expression)
        {
            base.VisitIf(expression);

            if (expression.Condition is ConstantExpression ce)
                return (bool) ce.Value ? expression.TruePart : expression.FalsePart;

            return expression;
        }

        private static void CheckDivisionByZero(IExpression expression)
        {
            if (expression is not ConstantExpression ce)
                return;

            if ((decimal) ce.Value == 0)
                throw Errors.DivideByZero.ToException(ErrorSource.Parser);
        }

        private static IExpression OptimizeInNotIn(BinaryExpression expression)
        {
            if (expression.Left is not ConstantExpression ce)
                return expression;

            var array = ((ArrayExpression) expression.Right).Operand;

            var newArray = new List<IExpression>();

            foreach (var item in array)
            {
                if (item is not ConstantExpression ce2)
                {
                    newArray.Add(item);
                    continue;
                }

                bool result;

                if (ce.StaticType == KnownTypes.Number)
                    result = (decimal) ce2.Value == (decimal) ce.Value;
                else if (ce.StaticType == KnownTypes.String)
                    result = (string) ce2.Value == (string) ce.Value;
                else if (ce.StaticType == KnownTypes.DateTime)
                    result = (DateTime) ce2.Value == (DateTime) ce.Value;
                else
                    result = (bool) ce2.Value == (bool) ce.Value;

                if ((expression.NodeType == NodeType.In && result) || (expression.NodeType == NodeType.NotIn && !result))
                    return Expressions.Constant(result);
            }

            if (newArray.Count == 0)
                return Expressions.Constant(expression.NodeType == NodeType.NotIn);

            expression.Right = Expressions.Array(newArray);
            return expression;
        }
    }
}
