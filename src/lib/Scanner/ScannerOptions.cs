// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// The Infix expression scanner options
    /// </summary>
    public sealed class ScannerOptions
    {
        /// <summary>
        /// Get whether whitespace tokens are ignored, default to false
        /// </summary>
        public bool IgnoreWhitespaceToken { get; init; } = false;

        /// <summary>
        /// Get maximum number of characters in identifier, default 255
        /// </summary>
        public int MaxIdentifierLength { get; init; } = 255;
    }
}
