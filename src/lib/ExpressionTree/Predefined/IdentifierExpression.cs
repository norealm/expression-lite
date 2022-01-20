// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent an identifier expression
    /// </summary>
    [DebuggerDisplay("{DebugHelper.Render(this), nq}")]
    public class IdentifierExpression : IExpression
    {
        /// <summary>
        /// Get identifier identity in symbol table
        /// </summary>
        public int Id { get; internal set; }

        /// <inheritdoc />
        public NodeType NodeType => NodeType.Identifier;

        /// <inheritdoc />
        public Type StaticType { get; internal set; }

        /// <inheritdoc />
        public IExpression Accept(IExpressionVisitor visitor)
            => visitor.VisitIdentifier(this);

        internal IdentifierExpression()
        {
        }
    }
}
