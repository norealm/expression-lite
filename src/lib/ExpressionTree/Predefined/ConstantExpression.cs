// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent a constant expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class ConstantExpression : IExpression
    {
        /// <inheritdoc />
        public NodeType NodeType => NodeType.Constant;

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <summary>
        /// Get constant value
        /// </summary>
        public object Value { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => visitor.VisitConstant(this);

        internal ConstantExpression()
        {
        }
    }
}
