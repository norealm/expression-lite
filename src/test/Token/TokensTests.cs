// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;
using Xunit;

namespace NoRealm.ExpressionLite.Test.Token
{
    public class TokensTests
    {
        [Fact]
        public void DateTimeLiteralShouldGetCreated()
        {
            var lexeme = "2022-03-04 01:02:03 AM";
            var dt = DateTime.Parse(lexeme);
            var dtLit = Tokens.DateTimeLiteral(lexeme, dt);

            Assert.NotNull(dtLit);
            Assert.Equal(TokenGroup.Literal, dtLit.Group);
            Assert.Equal(lexeme, dtLit.Lexeme);
            Assert.Equal(dt, dtLit.Value);
        }

        [Fact]
        public void IdentifierShouldGetCreated()
        {
            var lexeme = "some-identifier";
            var ident = Tokens.Identifier(lexeme);

            Assert.NotNull(ident);
            Assert.Equal(TokenGroup.Identifier, ident.Group);
            Assert.Equal(lexeme, ident.Lexeme);
        }

        [Fact]
        public void NumericLiteralShouldGetCreated()
        {
            var lexeme = "123.456";
            var value = decimal.Parse(lexeme);
            var nLit = Tokens.NumericLiteral(lexeme, value);

            Assert.NotNull(nLit);
            Assert.Equal(TokenGroup.Literal, nLit.Group);
            Assert.Equal(lexeme, nLit.Lexeme);
            Assert.Equal(value, nLit.Value);
        }

        [Fact]
        public void StringLiteralShouldGetCreated()
        {
            var lexeme = "expression-lite";
            var sLit = Tokens.StringLiteral(lexeme, lexeme);

            Assert.NotNull(sLit);
            Assert.Equal(TokenGroup.Literal, sLit.Group);
            Assert.Equal(lexeme, sLit.Lexeme);
            Assert.Equal(lexeme, sLit.Value);
        }

        [Fact]
        public void WhitespaceShouldGetCreated()
        {
            var wsLit = Tokens.Whitespace(7);

            Assert.NotNull(wsLit);
            Assert.Equal(TokenGroup.Whitespace, wsLit.Group);
            Assert.Equal("<whitespace>", wsLit.Lexeme);
            Assert.Equal(7, wsLit.Count);
        }
    }
}
