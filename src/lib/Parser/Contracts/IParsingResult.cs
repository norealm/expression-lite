// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.ExpressionTree;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// The result of parsing an expression
    /// </summary>
    public interface IParsingResult
    {
        /// <summary>
        /// The parsed expression
        /// </summary>
        IExpression Expression { get; }

        /// <summary>
        /// The table will all names in it
        /// </summary>
        IDictionary<int, NameDetails> Names { get; }
    }
}
