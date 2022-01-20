// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent an array expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class ArrayExpression : IExpression
    {
        /// <inheritdoc />
        public NodeType NodeType => NodeType.Array;

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <summary>
        /// Get array items
        /// </summary>
        public IReadOnlyList<IExpression> Operand { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => visitor.VisitArray(this);

        internal ArrayExpression()
        {
        }
    }
}
