// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    using static TokenGroup;

    /// <summary>
    /// Represent known tokens
    /// </summary>
    public static class KnownTokens
    {
        #region keywords

        /// <summary>
        /// true keyword
        /// </summary>
        public static readonly IKnownToken True =
            Keyword.Define(1, "true");

        /// <summary>
        /// false keyword
        /// </summary>
        public static readonly IKnownToken False =
            Keyword.Define(2, "false");

        /// <summary>
        /// in keyword
        /// </summary>
        public static readonly IKnownToken In =
            Keyword.Define(3, "in");

        /// <summary>
        /// have keyword
        /// </summary>
        public static readonly IKnownToken Have =
            Keyword.Define(4, "have");

        /// <summary>
        /// if keyword
        /// </summary>
        public static readonly IKnownToken If =
            Keyword.Define(5, "if");

        #endregion

        #region literals

        /// <summary>
        /// a string literal
        /// </summary>
        public static readonly IKnownToken String =
            Literal.Define(1, "<string>");

        /// <summary>
        /// a numeric literal
        /// </summary>
        public static readonly IKnownToken Number =
            Literal.Define(2, "<number>");

        /// <summary>
        /// a date and time literal
        /// </summary>
        public static readonly IKnownToken DateTime =
            Literal.Define(3, "<date-time>");

        #endregion

        #region symbols

        /// <summary>
        /// plus symbol
        /// </summary>
        public static readonly IKnownToken Plus =
            Symbol.Define(1, "+");

        /// <summary>
        /// minus symbol
        /// </summary>
        public static readonly IKnownToken Minus =
            Symbol.Define(2, "-");

        /// <summary>
        /// multiplication symbol
        /// </summary>
        public static readonly IKnownToken Multiply =
            Symbol.Define(3, "*");

        /// <summary>
        /// division symbol
        /// </summary>
        public static readonly IKnownToken Divide =
            Symbol.Define(4, "/");

        /// <summary>
        /// division reminder symbol
        /// </summary>
        public static readonly IKnownToken Reminder =
            Symbol.Define(5, "%");

        /// <summary>
        /// equal symbol
        /// </summary>
        public static readonly IKnownToken Equal =
            Symbol.Define(6, "==");

        /// <summary>
        /// not equal symbol
        /// </summary>
        public static readonly IKnownToken NotEqual =
            Symbol.Define(7, "!=");

        /// <summary>
        /// greater than symbol
        /// </summary>
        public static readonly IKnownToken GreaterThan =
            Symbol.Define(8, ">");

        /// <summary>
        /// greater than or equal symbol
        /// </summary>
        public static readonly IKnownToken GreaterThanEqual =
            Symbol.Define(9, ">=");

        /// <summary>
        /// less than symbol
        /// </summary>
        public static readonly IKnownToken LessThan =
            Symbol.Define(10, "<");

        /// <summary>
        /// less than or equal symbol
        /// </summary>
        public static readonly IKnownToken LessThanEqual =
            Symbol.Define(11, "<=");

        /// <summary>
        /// Open parentheses symbol
        /// </summary>
        public static readonly IKnownToken OpenParen =
            Symbol.Define(12, "(");

        /// <summary>
        /// Close parentheses symbol
        /// </summary>
        public static readonly IKnownToken CloseParen =
            Symbol.Define(13, ")");

        /// <summary>
        /// open bracket symbol
        /// </summary>
        public static readonly IKnownToken OpenBracket =
            Symbol.Define(14, "[");

        /// <summary>
        /// close bracket symbol
        /// </summary>
        public static readonly IKnownToken CloseBracket =
            Symbol.Define(15, "]");

        /// <summary>
        /// comma symbol
        /// </summary>
        public static readonly IKnownToken Comma =
            Symbol.Define(16, ",");

        /// <summary>
        /// logical and symbol
        /// </summary>
        public static readonly IKnownToken LogicAnd =
            Symbol.Define(17, "&&");

        /// <summary>
        /// logical or symbol
        /// </summary>
        public static readonly IKnownToken LogicOr =
            Symbol.Define(18, "||");

        /// <summary>
        /// logical not Symbol
        /// </summary>
        public static readonly IKnownToken LogicNot =
            Symbol.Define(19, "!");

        #endregion

        #region list tokens

        /// <summary>
        /// Get a list of all keywords
        /// </summary>
        /// <returns>a list of all keywords</returns>
        public static IKnownToken[] GetKeywords()
            => new[] { True, False, In, Have, If };

        /// <summary>
        /// Get a list of all literal types
        /// </summary>
        /// <returns>a list of all literal types</returns>
        public static IKnownToken[] GetLiterals()
            => new[] { String, Number, DateTime };

        /// <summary>
        /// Get a list of all symbols
        /// </summary>
        /// <returns>a list of all symbols</returns>
        public static IKnownToken[] GetSymbols()
            => new[]
            {
                Plus, Minus, Multiply, Divide, Reminder, OpenParen, CloseParen,
                OpenBracket, CloseBracket, GreaterThan, LessThan, Comma, LogicNot,
                /*****************************************************************/
                GreaterThanEqual, LessThanEqual, Equal, NotEqual, LogicAnd, LogicOr
            };

        #endregion

        #region known token

        private class KnownToken : IKnownToken
        {
            public TokenGroup Group { get; init; }
            public ushort Id { get; init; }
            public string Lexeme { get; init; }
        }

        /// <summary>
        /// Define a token using id
        /// </summary>
        /// <param name="tokenGroup">token group</param>
        /// <param name="tokenId">token id</param>
        /// <returns>a token definition</returns>
        private static IKnownToken Define(this TokenGroup tokenGroup, ushort tokenId)
            => Define(tokenGroup, tokenId, string.Empty);

        /// <summary>
        /// Define a token using id and lexeme
        /// </summary>
        /// <param name="tokenGroup">token group</param>
        /// <param name="tokenId">token id</param>
        /// <param name="lexeme">token lexeme</param>
        /// <returns>a token definition</returns>
        private static IKnownToken Define(this TokenGroup tokenGroup, ushort tokenId, string lexeme)
            => new KnownToken {Group = tokenGroup, Id = tokenId, Lexeme = lexeme};

        #endregion
    }
}
