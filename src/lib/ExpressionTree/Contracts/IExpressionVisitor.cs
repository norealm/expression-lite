// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// A visitor for expression tree
    /// </summary>
    public interface IExpressionVisitor
    {
        /// <summary>
        /// dispatch the call to specific visitor
        /// </summary>
        /// <param name="expression">expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression Visit(IExpression expression);

        /// <summary>
        /// Visit negation expression node
        /// </summary>
        /// <param name="expression">unary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitNegation(UnaryExpression expression);

        /// <summary>
        /// Visit unary plus expression node
        /// </summary>
        /// <param name="expression">unary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitUnaryPlus(UnaryExpression expression);

        /// <summary>
        /// Visit logic not expression node
        /// </summary>
        /// <param name="expression">unary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitLogicalNot(UnaryExpression expression);

        /// <summary>
        /// Visit logic and expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitLogicalAnd(BinaryExpression expression);

        /// <summary>
        /// Visit logic and expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitLogicalOr(BinaryExpression expression);

        /// <summary>
        /// Visit logic and expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitMultiply(BinaryExpression expression);

        /// <summary>
        /// Visit logic and expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitDivide(BinaryExpression expression);

        /// <summary>
        /// Visit logic and expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitReminder(BinaryExpression expression);

        /// <summary>
        /// Visit add expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitAdd(BinaryExpression expression);

        /// <summary>
        /// Visit append expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitAppend(BinaryExpression expression);

        /// <summary>
        /// Visit subtract expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitSubtract(BinaryExpression expression);

        /// <summary>
        /// Visit in expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitIn(BinaryExpression expression);

        /// <summary>
        /// Visit not in expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitNotIn(BinaryExpression expression);

        /// <summary>
        /// Visit have expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitHave(BinaryExpression expression);

        /// <summary>
        /// Visit not have expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitNotHave(BinaryExpression expression);

        /// <summary>
        /// Visit greater than expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitGreaterThan(BinaryExpression expression);

        /// <summary>
        /// Visit greater than or equals to expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitGreaterThanOrEqualsTo(BinaryExpression expression);

        /// <summary>
        /// Visit less than expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitLessThan(BinaryExpression expression);

        /// <summary>
        /// Visit less than or equals to expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitLessThanOrEqualsTo(BinaryExpression expression);

        /// <summary>
        /// Visit equals to expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitEqualsTo(BinaryExpression expression);

        /// <summary>
        /// Visit not equals to expression node
        /// </summary>
        /// <param name="expression">binary expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitNotEqualsTo(BinaryExpression expression);

        /// <summary>
        /// Visit array expression node
        /// </summary>
        /// <param name="expression">expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitArray(ArrayExpression expression);

        /// <summary>
        /// Visit if expression node
        /// </summary>
        /// <param name="expression">expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitIf(IfExpression expression);

        /// <summary>
        /// Visit identifier expression node
        /// </summary>
        /// <param name="expression">expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitIdentifier(IdentifierExpression expression);

        /// <summary>
        /// Visit constant expression node
        /// </summary>
        /// <param name="expression">expression to visit</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression VisitConstant(ConstantExpression expression);
    }
}
