// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using System;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent a unary expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class UnaryExpression : IExpression
    {
        /// <inheritdoc />
        public NodeType NodeType { get; }

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <summary>
        /// Get unary expression operand
        /// </summary>
        public IExpression Operand { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => NodeType switch
            {
                NodeType.UnaryMinus => visitor.VisitNegation(this),
                NodeType.UnaryPlus => visitor.VisitUnaryPlus(this),
                NodeType.LogicNot => visitor.VisitLogicalNot(this),
                _ => throw Errors.UnknownExpressionNode.ToException(ErrorSource.Parser, NodeType)
            };

        internal UnaryExpression(NodeType nodeType)
            => NodeType = nodeType;
    }
}
