// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent a literal token
    /// </summary>
    public sealed class LiteralToken : IToken
    {
        /// <inheritdoc />
        public TokenGroup Group => TokenGroup.Literal;

        /// <inheritdoc />
        public string Lexeme { get; internal set; }

        /// <summary>
        /// Get literal value
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// Get value static type
        /// </summary>
        public Type StaticType { get; internal set; }

        internal LiteralToken() {}
    }
}
