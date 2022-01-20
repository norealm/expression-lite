// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NoRealm.ExpressionLite.Naming
{
    /// <summary>
    /// Entry point to define custom names
    /// </summary>
    public static class NameInfo
    {
        /// <summary>
        /// Define a custom name with specific value
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>name information</returns>
        public static INameInfo FromValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var type = value.GetType();

            if (type.IsNumber())
                return new InternalNameInfo {Value = value.NumberToDecimal(), NameType = NameType.Plain, StaticType = KnownTypes.Number};

            if (!type.IsKnown())
                throw new ArgumentException("the type of the value must be one of the primitive type");

            return new InternalNameInfo {Value = value, NameType = NameType.Plain, StaticType = type};
        }

        /// <summary>
        /// Define a custom name with an expression to get scanned
        /// </summary>
        /// <param name="expression">expression to scan</param>
        /// <returns>name information</returns>
        public static INameInfo FromStringExpression(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new InternalNameInfo {Value = expression, NameType = NameType.Expression, StaticType = KnownTypes.String};
        }

        /// <summary>
        /// Define a custom name which point to a property/field inside an instance
        /// </summary>
        /// <typeparam name="T">input type</typeparam>
        /// <param name="propertySelector">the property selector</param>
        /// <returns>name information</returns>
        /// <remarks>the property/field type must be one of the four primitive types</remarks>
        public static INameInfo FromOtherMember<T>(Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var member = GetLambdaInfo(propertySelector,
                "the only expressions are allowed in this context is to access a property or a field in input instance",
                true);

            if (member is not {Expression: ParameterExpression})
                throw new ArgumentException("you can only use input parameter to select a property/field from it.");

            return new InternalNameInfo {Value = member.Member, NameType = NameType.Member, StaticType = member.Type};
        }

        /// <summary>
        /// Define a custom name which point to a constant or property/field inside a static type
        /// </summary>
        /// <param name="propertySelector">the property selector</param>
        /// <returns>name information</returns>
        /// <remarks>the constant/property/field type must be one of the four primitive types</remarks>
        public static INameInfo FromOtherMember(Expression<Func<object>> propertySelector)
        {
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var member = GetLambdaInfo(propertySelector,
                "the only expressions are allowed in this context is to access a property or a field in input instance",
                false);

            if (member == null)
                throw new ArgumentException("you can only access the property/field in here, you can not use an expression to do that");

            return new InternalNameInfo {Value = member.Member, NameType = NameType.Member, StaticType = member.Type};
        }

        /// <summary>
        /// Define a custom name which maps to a lambda expression
        /// </summary>
        /// <typeparam name="T">input type</typeparam>
        /// <typeparam name="U">return type</typeparam>
        /// <param name="linqExpression">the linq expression</param>
        /// <returns>name information</returns>
        /// <remarks>the return type must be one of the four primitive types or object which later deduce to one of the four primitive types</remarks>
        public static INameInfo FromLinqExpression<T, U>(Expression<Func<T, U>> linqExpression)
        {
            if (linqExpression == null)
                throw new ArgumentNullException(nameof(linqExpression));

            if (!typeof(U).IsNumber() && !KnownTypes.GeTypes().Contains(typeof(U)))
                throw new ArgumentException($"the type of the value must be one of the primitive type");

            return new InternalNameInfo {Value = linqExpression, NameType = NameType.LinqExpression, StaticType = typeof(U)};
        }

        private static MemberExpression GetLambdaInfo(LambdaExpression expression, string message, bool haveParam)
        {
            var exp = expression.Body;

            if (exp.NodeType == ExpressionType.Convert && exp.Type == typeof(object))
                exp = ((UnaryExpression) exp).Operand;

            if (exp.NodeType == ExpressionType.Convert)
                exp = ((UnaryExpression) exp).Operand;

            if (exp.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(message);

            var member = (MemberExpression) exp;

            if (!haveParam && member.Expression != null)
                return null;

            if (haveParam && member.Expression == null)
                return null;

            if (!member.Type.IsNumber() && !member.Type.IsKnown())
                throw new ArgumentException("the type of the property/field must be one of the primitive type");

            return member;
        }

        private static decimal NumberToDecimal(this object value)
        {
            return value switch
            {
                decimal d => d,
                float f => new decimal(f),
                double d2 => new decimal(d2),
                < 0 => new decimal((long)value),
                _ => new decimal((ulong) value)
            };
        }
    }
}
