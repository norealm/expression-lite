// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent a whitespace token
    /// </summary>
    public sealed class WhitespaceToken : IToken
    {
        /// <inheritdoc />
        public TokenGroup Group => TokenGroup.Whitespace;

        /// <inheritdoc />
        public string Lexeme => "<whitespace>";

        /// <summary>
        /// Get whitespaces count
        /// </summary>
        public int Count { get; internal set; }

        internal WhitespaceToken() { }
    }
}
