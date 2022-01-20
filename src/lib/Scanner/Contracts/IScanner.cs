// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// Represent a Scanner which is responsible for converting source into tokens
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Get scanner options
        /// </summary>
        ScannerOptions Options { get; }

        /// <summary>
        /// Scan a source and produce a sequence of tokens
        /// </summary>
        /// <param name="sourceText">source text to scan</param>
        /// <returns>A sequence of tokens</returns>
        IEnumerable<IToken> Scan(string sourceText);
    }
}
