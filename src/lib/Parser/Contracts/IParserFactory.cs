// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Scanner;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// A factory for creating <see cref="IParser"/> instance
    /// </summary>
    public interface IParserFactory
    {
        /// <summary>
        /// Get parser instance
        /// </summary>
        /// <param name="parserOptions">the parser options</param>
        /// <returns>parser instance</returns>
        IParser GetParser(ParserOptions parserOptions);

        /// <summary>
        /// Get parser instance
        /// </summary>
        /// <param name="parserOptions">the parser options</param>
        /// <param name="scanner">a scanner to get named-expression tokens</param>
        /// <param name="nameProviders">the name providers</param>
        /// <returns>parser instance</returns>
        IParser GetParser(ParserOptions parserOptions, IScanner scanner, IEnumerable<INameProvider> nameProviders);
    }
}
