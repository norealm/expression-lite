// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent an identifier token
    /// </summary>
    public sealed class IdentifierToken : IToken
    {
        /// <inheritdoc />
        public TokenGroup Group => TokenGroup.Identifier;

        /// <inheritdoc />
        public string Lexeme { get; internal set; }

        internal IdentifierToken() { }
    }
}
