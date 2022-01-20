// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.Token;
using System;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Helper methods for creating expressions
    /// </summary>
    public static class Expressions
    {
        /// <summary>
        /// define a decimal constant
        /// </summary>
        /// <param name="value">decimal value</param>
        /// <returns>the constant expression</returns>
        public static ConstantExpression Constant(decimal value)
            => value.Constant(KnownTypes.Number);

        /// <summary>
        /// define a string constant
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>the constant expression</returns>
        public static ConstantExpression Constant(string value)
            => value.Constant(KnownTypes.String);

        /// <summary>
        /// define a boolean constant
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>the constant expression</returns>
        public static ConstantExpression Constant(bool value)
            => value.Constant(KnownTypes.Boolean);

        /// <summary>
        /// define a DateTime constant
        /// </summary>
        /// <param name="value">DateTime value</param>
        /// <returns>the constant expression</returns>
        public static ConstantExpression Constant(DateTime value)
            => value.Constant(KnownTypes.DateTime);

        /// <summary>
        /// define an array expression
        /// </summary>
        /// <param name="expressions">array members</param>
        /// <returns>the array expression</returns>
        /// <remarks>the array type is the static type of the first element</remarks>
        public static ArrayExpression Array(params IExpression[] expressions)
            => Array((IReadOnlyList<IExpression>)expressions);

        /// <summary>
        /// define an array expression
        /// </summary>
        /// <param name="expressions">array members</param>
        /// <returns>the array expression</returns>
        /// <remarks>the array type is the static type of the first element</remarks>
        public static ArrayExpression Array(IReadOnlyList<IExpression> expressions)
        {
            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            if (expressions.Count == 0)
                throw Errors.EmptyArray.ToException(ErrorSource.Parser);

            if (expressions[0] == null)
                throw new ArgumentException("expression at index 0 is null");

            for (var i = 1; i < expressions.Count; ++i)
            {
                if (expressions[i] == null)
                    throw new ArgumentException($"expression at index {i} is null");

                if (expressions[i].StaticType != expressions[0].StaticType)
                    throw Errors.Expected.ToException(ErrorSource.Parser, expressions[0].StaticType, expressions[i].StaticType);
            }

            return new() {Operand = expressions, StaticType = expressions[0].StaticType};
        }

        /// <summary>
        /// define an identifier
        /// </summary>
        /// <param name="id">identifier id</param>
        /// <param name="type">identifier type</param>
        /// <returns>the identifier expression</returns>
        public static IdentifierExpression Identifier(int id, Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var isNumeric = type.IsNumber();

            if (!isNumeric && !type.IsKnown())
                throw new ArgumentException($"type {type.FullName} is not a predefined type.");

            return new() {Id = id, StaticType = isNumeric ? KnownTypes.Number : type};
        }

        /// <summary>
        /// define an if expression
        /// </summary>
        /// <param name="condition">condition expression</param>
        /// <param name="true">true expression</param>
        /// <param name="false">false expression</param>
        /// <returns>the if expression</returns>
        public static IfExpression If(IExpression condition, IExpression @true, IExpression @false)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            if (@true == null)
                throw new ArgumentNullException(nameof(@true));

            if (@false == null)
                throw new ArgumentNullException(nameof(@false));

            if (condition.StaticType != KnownTypes.Boolean)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Boolean, condition.StaticType);

            if (@true.StaticType != @false.StaticType)
                throw Errors.IfExpressionInvalidTypes.ToException(ErrorSource.Parser);

            return new() {Condition = condition, TruePart = @true, FalsePart = @false, StaticType = @true.StaticType};
        }

        /// <summary>
        /// define a negate operation for numeric expression
        /// </summary>
        /// <param name="expression">numeric expression</param>
        /// <returns>the negation expression</returns>
        public static UnaryExpression Negate(IExpression expression)
            => NodeType.UnaryMinus.NegatePlusNot(expression, KnownTypes.Number);

        /// <summary>
        /// define a plus operation for numeric expression
        /// </summary>
        /// <param name="expression">numeric expression</param>
        /// <returns>the plus expression</returns>
        public static UnaryExpression Plus(IExpression expression)
            => NodeType.UnaryPlus.NegatePlusNot(expression, KnownTypes.Number);

        /// <summary>
        /// define a logical not operation for boolean expression
        /// </summary>
        /// <param name="expression">boolean expression</param>
        /// <returns>the not expression</returns>
        public static UnaryExpression Not(IExpression expression)
            => NodeType.LogicNot.NegatePlusNot(expression, KnownTypes.Boolean);

        /// <summary>
        /// define and expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the and expression</returns>
        public static BinaryExpression And(IExpression left, IExpression right)
            => NodeType.LogicAnd.AndOr(left, right);

        /// <summary>
        /// define or expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the or expression</returns>
        public static BinaryExpression Or(IExpression left, IExpression right)
            => NodeType.LogicOr.AndOr(left, right);

        /// <summary>
        /// define multiply expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the multiply expression</returns>
        public static BinaryExpression Multiply(IExpression left, IExpression right)
            => NodeType.Multiply.MultiplyDivideReminder(left, right);

        /// <summary>
        /// define divide expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the divide expression</returns>
        public static BinaryExpression Divide(IExpression left, IExpression right)
            => NodeType.Divide.MultiplyDivideReminder(left, right);

        /// <summary>
        /// define reminder expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the reminder expression</returns>
        public static BinaryExpression Reminder(IExpression left, IExpression right)
            => NodeType.Reminder.MultiplyDivideReminder(left, right);

        /// <summary>
        /// define add expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the add expression</returns>
        public static BinaryExpression Add(IExpression left, IExpression right)
            => NodeType.Add.AddSubtract(left, right);

        /// <summary>
        /// define subtract expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the subtract expression</returns>
        public static BinaryExpression Subtract(IExpression left, IExpression right)
            => NodeType.Subtract.AddSubtract(left, right);

        /// <summary>
        /// define append expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the append expression</returns>
        public static BinaryExpression Append(IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.String)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.String, left.StaticType);

            if (right.StaticType != KnownTypes.String)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.String, right.StaticType);

            return new(NodeType.Append) {Left = left, Right = right, StaticType = KnownTypes.String};
        }

        /// <summary>
        /// define in expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the in expression</returns>
        public static BinaryExpression In(IExpression left, IExpression right)
            => NodeType.In.InNotIn(left, right);

        /// <summary>
        /// define not in expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the not in expression</returns>
        public static BinaryExpression NotIn(IExpression left, IExpression right)
            => NodeType.NotIn.InNotIn(left, right);

        /// <summary>
        /// define have expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the have expression</returns>
        public static BinaryExpression Have(IExpression left, IExpression right)
            => NodeType.Have.HaveNotHave(left, right);

        /// <summary>
        /// define not have expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the not have expression</returns>
        public static BinaryExpression NotHave(IExpression left, IExpression right)
            => NodeType.NotHave.HaveNotHave(left, right);

        /// <summary>
        /// define greater than expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the greater than expression</returns>
        public static BinaryExpression GreaterThan(IExpression left, IExpression right)
            => NodeType.GreaterThan.Relational(left, right);

        /// <summary>
        /// define less than expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the less than expression</returns>
        public static BinaryExpression LessThan(IExpression left, IExpression right)
            => NodeType.LessThan.Relational(left, right);

        /// <summary>
        /// define greater than or equals to expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the greater than or equals expression</returns>
        public static BinaryExpression GreaterThanOrEquals(IExpression left, IExpression right)
            => NodeType.GreaterThanOrEquals.Relational(left, right);

        /// <summary>
        /// define less than or equals expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the less than or equals expression</returns>
        public static BinaryExpression LessThanOrEquals(IExpression left, IExpression right)
            => NodeType.LessThanOrEquals.Relational(left, right);

        /// <summary>
        /// define equals expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the equals expression</returns>
        public static BinaryExpression Equals(IExpression left, IExpression right)
            => NodeType.Equals.Equality(left, right);

        /// <summary>
        /// define not equals expression
        /// </summary>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the not equals expression</returns>
        public static BinaryExpression NotEquals(IExpression left, IExpression right)
            => NodeType.NotEquals.Equality(left, right);

        #region support methods

        /// <summary>
        /// define a constant expression
        /// </summary>
        /// <param name="value">expression value</param>
        /// <param name="type">value type</param>
        /// <returns>the constant expression</returns>
        internal static ConstantExpression Constant(this object value, Type type)
        {
            if (type == KnownTypes.String && value == null)
                throw new ArgumentNullException(nameof(value));

            return new() {Value = value, StaticType = type};
        }

        /// <summary>
        /// define a negate operation for numeric expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.UnaryPlus"/> or <see cref="NodeType.UnaryMinus"/> or <see cref="NodeType.LogicNot"/></param>
        /// <param name="expression">numeric expression</param>
        /// <param name="expected">expected type</param>
        /// <returns>the negation expression</returns>
        internal static UnaryExpression NegatePlusNot(this NodeType nodeType, IExpression expression, Type expected)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.StaticType != expected)
                throw Errors.Expected.ToException(ErrorSource.Parser, expected.Dump(), expression.Dump());

            return new(nodeType) {Operand = expression, StaticType = expected};
        }

        /// <summary>
        /// define and/or expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.LogicAnd"/> or <see cref="NodeType.LogicOr"/></param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the and/or expression</returns>
        internal static BinaryExpression AndOr(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.Boolean)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Boolean.Dump(), left.Dump());

            if (right.StaticType != KnownTypes.Boolean)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Boolean.Dump(), right.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Boolean};
        }

        /// <summary>
        /// define multiply/divide/reminder expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.Multiply"/> or <see cref="NodeType.Divide"/> or <see cref="NodeType.Reminder"/></param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the multiply/divide/reminder expression</returns>
        internal static BinaryExpression MultiplyDivideReminder(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), left.Dump());

            if (right.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), right.Dump());

            if (nodeType != NodeType.Multiply && right is ConstantExpression ce && (decimal) ce.Value == 0)
                throw Errors.DivideByZero.ToException(ErrorSource.Parser);

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Number};
        }

        /// <summary>
        /// define add/subtract expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.Add"/> or <see cref="NodeType.Subtract"/></param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the add/subtract expression</returns>
        internal static BinaryExpression AddSubtract(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), left.Dump());

            if (right.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), right.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Number};
        }

        /// <summary>
        /// define in/not-in expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.In"/> or <see cref="NodeType.NotIn"/></param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the in/not-in expression</returns>
        internal static BinaryExpression InNotIn(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (right is not ArrayExpression ae)
                throw Errors.Expected.ToException(ErrorSource.Parser, "<array>", right.Dump());

            if (ae.StaticType != left.StaticType)
                throw Errors.Expected.ToException(ErrorSource.Parser, left.StaticType.Dump(), right.StaticType.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Boolean};
        }

        /// <summary>
        /// define have/not-have expression
        /// </summary>
        /// <param name="nodeType">either <see cref="NodeType.Have"/> or <see cref="NodeType.NotHave"/></param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the have/not-have expression</returns>
        internal static BinaryExpression HaveNotHave(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.String)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.String.Dump(), left.Dump());

            if (right.StaticType != KnownTypes.String)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.String.Dump(), right.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Boolean};
        }

        /// <summary>
        /// define a relational expression
        /// </summary>
        /// <param name="nodeType">a relational operator</param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the relational expression</returns>
        internal static BinaryExpression Relational(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), left.Dump());

            if (right.StaticType != KnownTypes.Number)
                throw Errors.Expected.ToException(ErrorSource.Parser, KnownTypes.Number.Dump(), right.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Boolean};
        }

        /// <summary>
        /// define equals/not-equals expression
        /// </summary>
        /// <param name="nodeType">a relational operator</param>
        /// <param name="left">left expression</param>
        /// <param name="right">right expression</param>
        /// <returns>the equals/not-equals expression</returns>
        internal static BinaryExpression Equality(this NodeType nodeType, IExpression left, IExpression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StaticType != right.StaticType)
                throw Errors.Expected.ToException(ErrorSource.Parser, left.StaticType.Dump(), right.StaticType.Dump());

            return new(nodeType) {Left = left, Right = right, StaticType = KnownTypes.Boolean};
        }

        /// <summary>
        /// dump expression type
        /// </summary>
        /// <param name="expression">expression node</param>
        /// <returns>type of expression</returns>
        private static string Dump(this IExpression expression)
        {
            return expression switch
            {
                IfExpression => "<if-expression>",
                ArrayExpression => "<array>",
                IdentifierExpression => "<identifier>",

                ConstantExpression ce when ce.StaticType == KnownTypes.String => "<string-literal>",
                ConstantExpression ce when ce.StaticType == KnownTypes.Number => "<numeric-literal>",
                ConstantExpression ce when ce.StaticType == KnownTypes.Boolean => "<boolean-literal>",
                ConstantExpression => "<datetime-literal>",
                
                UnaryExpression => expression.NodeType switch
                {
                    NodeType.UnaryMinus => "<negate-expression>",
                    NodeType.UnaryPlus => "<plus-expression>",
                    _ => "<not-expression>"
                },

                BinaryExpression => expression.NodeType switch
                {
                    NodeType.LogicAnd => "<and-expression>",
                    NodeType.LogicOr => "<or-expression>",
                    NodeType.Multiply => "<multiply-expression>",
                    NodeType.Divide => "<divide-expression>",
                    NodeType.Reminder => "<reminder-expression>",
                    NodeType.Add => "<add-expression>",
                    NodeType.Append => "<append-expression>",
                    NodeType.Subtract => "<subtract-expression>",
                    NodeType.In => "<in-expression>",
                    NodeType.NotIn => "<not-in-expression>",
                    NodeType.Have => "<have-expression>",
                    NodeType.NotHave => "<not-have-expression>",
                    NodeType.GreaterThan => "<greater-than-expression>",
                    NodeType.GreaterThanOrEquals => "<greater-than-equals-expression>",
                    NodeType.LessThan => "<less-than-expression>",
                    NodeType.LessThanOrEquals => "<less-than-equals-expression>",
                    NodeType.Equals => "<equals-expression>",
                    _ => "<not-equals-expression>"
                },

                _ => string.Empty
            };
        }

        /// <summary>
        /// dump type name
        /// </summary>
        /// <param name="type">type information</param>
        /// <returns>type name</returns>
        private static string Dump(this Type type)
        {
            return type switch
            {
                _ when type == KnownTypes.Boolean => "<boolean>",
                _ when type == KnownTypes.String => "<string>",
                _ when type == KnownTypes.Number => "<numeric>",
                _ when type == KnownTypes.DateTime => "<date-time>",
                _ => $"'{type.FullName}'"
            };
        }

        #endregion
    }
}
