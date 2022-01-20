// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using System;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent a binary expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class BinaryExpression : IExpression
    {
        /// <inheritdoc />
        public NodeType NodeType { get; }

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <summary>
        /// Get left expression
        /// </summary>
        public IExpression Left { get; internal set; }

        /// <summary>
        /// Get right expression
        /// </summary>
        public IExpression Right { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => NodeType switch
            {
                NodeType.LogicAnd => visitor.VisitLogicalAnd(this),
                NodeType.LogicOr => visitor.VisitLogicalOr(this),
                NodeType.Multiply => visitor.VisitMultiply(this),
                NodeType.Divide => visitor.VisitDivide(this),
                NodeType.Reminder => visitor.VisitReminder(this),
                NodeType.Add => visitor.VisitAdd(this),
                NodeType.Append => visitor.VisitAppend(this),
                NodeType.Subtract => visitor.VisitSubtract(this),
                NodeType.In => visitor.VisitIn(this),
                NodeType.NotIn => visitor.VisitNotIn(this),
                NodeType.Have => visitor.VisitHave(this),
                NodeType.NotHave => visitor.VisitNotHave(this),
                NodeType.GreaterThan => visitor.VisitGreaterThan(this),
                NodeType.GreaterThanOrEquals => visitor.VisitGreaterThanOrEqualsTo(this),
                NodeType.LessThan => visitor.VisitLessThan(this),
                NodeType.LessThanOrEquals => visitor.VisitLessThanOrEqualsTo(this),
                NodeType.Equals => visitor.VisitEqualsTo(this),
                NodeType.NotEquals => visitor.VisitNotEqualsTo(this),
                _ => throw Errors.UnknownExpressionNode.ToException(ErrorSource.Parser, NodeType)
            };

        internal BinaryExpression(NodeType nodeType)
            => NodeType = nodeType;
    }
}
