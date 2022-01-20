// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Represent an expression tree node
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Get node type
        /// </summary>
        NodeType NodeType { get; }

        /// <summary>
        /// Get node static type
        /// </summary>
        Type StaticType { get; }

        /// <summary>
        /// calls the appropriate visitor method for this node
        /// </summary>
        /// <param name="visitor">the visitor object</param>
        /// <returns>the expression result from visiting the node</returns>
        IExpression Accept(IExpressionVisitor visitor);
    }
}
