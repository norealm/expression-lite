// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent if expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class IfExpression : IExpression
    {
        /// <inheritdoc />
        public NodeType NodeType => NodeType.If;

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <summary>
        /// Get condition expression
        /// </summary>
        public IExpression Condition { get; internal set; }

        /// <summary>
        /// Get true expression
        /// </summary>
        public IExpression TruePart { get; internal set; }

        /// <summary>
        /// Get false expression
        /// </summary>
        public IExpression FalsePart { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => visitor.VisitIf(this);

        internal IfExpression() { }
    }
}
