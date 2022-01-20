// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent a base class for visiting <see cref="IExpression"/>
    /// </summary>
    public abstract class ExpressionVisitorBase : IExpressionVisitor
    {
        /// <inheritdoc />
        public virtual IExpression Visit(IExpression expression)
            => expression.Accept(this);

        /// <inheritdoc />
        public virtual IExpression VisitNegation(UnaryExpression expression)
            => OnVisitUnary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitUnaryPlus(UnaryExpression expression)
            => OnVisitUnary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitLogicalNot(UnaryExpression expression)
            => OnVisitUnary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitLogicalAnd(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitLogicalOr(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitMultiply(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitDivide(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitReminder(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitAdd(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitAppend(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitSubtract(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitIn(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitNotIn(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitHave(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitNotHave(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitGreaterThan(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitGreaterThanOrEqualsTo(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitLessThan(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitLessThanOrEqualsTo(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitEqualsTo(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitNotEqualsTo(BinaryExpression expression)
            => OnVisitBinary(expression);

        /// <inheritdoc />
        public virtual IExpression VisitArray(ArrayExpression expression)
        {
            var exp = new IExpression[expression.Operand.Count];

            for (var i = 0; i < expression.Operand.Count; ++i)
                exp[i] = OnVisitArrayItem(expression.Operand[i]);

            expression.Operand = exp;

            return expression;
        }

        /// <inheritdoc />
        public virtual IExpression VisitIf(IfExpression expression)
        {
            expression.Condition = Visit(expression.Condition);
            expression.TruePart = Visit(expression.TruePart);
            expression.FalsePart = Visit(expression.FalsePart);

            return expression;
        }

        /// <inheritdoc />
        public virtual IExpression VisitIdentifier(IdentifierExpression expression)
            => expression;

        /// <inheritdoc />
        public virtual IExpression VisitConstant(ConstantExpression expression)
            => expression;

        /// <summary>
        /// executed when visiting a unary expression
        /// </summary>
        /// <param name="expression">a unary expression</param>
        /// <returns>the unary expression after visiting its operand</returns>
        protected virtual IExpression OnVisitUnary(UnaryExpression expression)
        {
            expression.Operand = Visit(expression.Operand);
            return expression;
        }

        /// <summary>
        /// executed when visiting a binary expression
        /// </summary>
        /// <param name="expression">a binary expression</param>
        /// <returns>the binary expression after visiting its left and right expression</returns>
        protected virtual IExpression OnVisitBinary(BinaryExpression expression)
        {
            expression.Left = Visit(expression.Left);
            expression.Right = Visit(expression.Right);
            return expression;
        }

        /// <summary>
        /// executed when visiting array expressions
        /// </summary>
        /// <param name="expression">array expression</param>
        /// <returns>the array expression item after being visited</returns>
        protected virtual IExpression OnVisitArrayItem(IExpression expression)
            => Visit(expression);
    }
}
