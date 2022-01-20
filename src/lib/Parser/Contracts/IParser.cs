// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// Represent expression parser
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Get parser options
        /// </summary>
        ParserOptions Options { get; }

        /// <summary>
        /// Parse input token stream
        /// </summary>
        /// <param name="tokens">a sequence of tokens to parse</param>
        /// <param name="name">an optional name to identify this token series, in order to detect self-referencing</param>
        /// <returns>parsing results</returns>
        IParsingResult Parse(IEnumerable<IToken> tokens, string name = null);
    }
}
