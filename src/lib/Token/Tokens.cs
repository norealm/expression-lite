// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Define a token
    /// </summary>
    public static class Tokens
    {
        /// <summary>
        /// Create an identifier token
        /// </summary>
        /// <param name="lexeme">token lexeme</param>
        /// <returns>an identifier token</returns>
        public static IdentifierToken Identifier(string lexeme)
            => new() {Lexeme = lexeme};

        /// <summary>
        /// Create a whitespace token
        /// </summary>
        /// <param name="count">number of consecutive whitespaces</param>
        /// <returns>a whitespace token</returns>
        public static WhitespaceToken Whitespace(int count = 1)
            => new() {Count = count};

        /// <summary>
        /// Create a date and time literal token
        /// </summary>
        /// <param name="lexeme">lexeme data</param>
        /// <param name="value">date and time value</param>
        /// <returns>a date and time literal token</returns>
        public static LiteralToken DateTimeLiteral(string lexeme, DateTime value)
            => new() {StaticType = KnownTypes.DateTime, Lexeme = lexeme, Value = value};

        /// <summary>
        /// Create a decimal literal token
        /// </summary>
        /// <param name="lexeme">lexeme data</param>
        /// <param name="value">decimal value</param>
        /// <returns>a decimal literal token</returns>
        public static LiteralToken NumericLiteral(string lexeme, decimal value)
            => new() {StaticType = KnownTypes.Number, Lexeme = lexeme, Value = value};

        /// <summary>
        /// Create a string literal token
        /// </summary>
        /// <param name="lexeme">lexeme data</param>
        /// <param name="value">string value</param>
        /// <returns>a string literal token</returns>
        public static LiteralToken StringLiteral(string lexeme, string value)
            => new() {StaticType = KnownTypes.String, Lexeme = lexeme, Value = value};
    }
}
