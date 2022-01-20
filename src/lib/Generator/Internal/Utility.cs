// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Parser;
using NoRealm.ExpressionLite.Token;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NoRealm.ExpressionLite.Generator
{
    internal static class Utility
    {
        internal static LambdaExpression CreateExpression(this IParsingResult parsingResult, Type inType, Type outType)
        {
            ValidateInputs(parsingResult, inType, outType, true);

            return new LinqGenerator(parsingResult, inType, outType).Generate();
        }

        internal static LambdaExpression CreateExpression(this IParsingResult parsingResult, Type outType)
        {
            ValidateInputs(parsingResult, null, outType, false);

            return new LinqGenerator(parsingResult, null, outType).Generate();
        }

        private static void ValidateInputs(IParsingResult parsingResult, Type inType, Type outType, bool inTypeRequired)
        {
            if (parsingResult == null)
                throw new ArgumentNullException(nameof(parsingResult));

            if (parsingResult.Expression == null)
                throw new ArgumentException("the expression in parsing result is null.");

            if (parsingResult.Names == null)
                throw new ArgumentException("the names table in parsing result is null.");

            if (inTypeRequired && inType == null)
                throw new ArgumentNullException(nameof(inType));

            if (inType != null && parsingResult.Names.Count != 0)
            {
                foreach (var details in parsingResult.Names.Values)
                {
                    if (details.Name.NameType == NameType.Member)
                    {
                        var member = (MemberInfo) details.Name.Value;
                        var isStatic = member.GetMemberInfo().IsStatic;

                        if (!isStatic && member.DeclaringType != inType)
                            throw Errors.InvalidLinqExpressionParameterType.ToException(ErrorSource.Generator, details.Name, inType.FullName, member.DeclaringType?.FullName);
                    }
                    else if (details.Name.NameType == NameType.LinqExpression)
                    {
                        var lambda = (LambdaExpression)details.Name.Value;

                        if (lambda.Parameters.Count != 1)
                            throw Errors.IncorrectLinqExpressionArguments.ToException(ErrorSource.Generator, details.Name, inType.FullName);

                        if (lambda.Parameters[0].Type != inType)
                            throw Errors.InvalidLinqExpressionParameterType.ToException(ErrorSource.Generator, details.Name, inType.FullName, lambda.Parameters[0].Type.FullName);
                    }
                }
            }

            if (outType == null)
                throw new ArgumentNullException(nameof(outType));

            if (outType != typeof(object))
            {
                if (!outType.IsKnown())
                    throw Errors.Expected.ToException(ErrorSource.Generator, "a primitive type as a return type", outType.FullName);

                if (!outType.IsAssignableFrom(parsingResult.Expression.StaticType))
                    throw new ArgumentException($"the return type cannot converted from expression return type '{parsingResult.Expression.StaticType.FullName}' to existing  '{outType.FullName}'.");
            }
        }

        internal static (bool IsStatic, Type Type) GetMemberInfo(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    var p = (PropertyInfo) member;
                    return (p.GetMethod.IsStatic, p.PropertyType);
                case MemberTypes.Field:
                    var f = (FieldInfo) member;
                    return (f.IsStatic, f.FieldType);
                default:
                    return (false, null);
            }
        }
    }
}
