// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using Xunit;

namespace NoRealm.ExpressionLite.Test.Token
{
    public class KnownTokensTests
    {
        [Fact]
        public void KeywordsShouldBeKnown()
        {
            foreach (var token in KnownTokens.GetKeywords())
                Assert.IsAssignableFrom<IKnownToken>(token);
        }

        [Fact]
        public void LiteralsShouldBeKnown()
        {
            foreach (var token in KnownTokens.GetLiterals())
                Assert.IsAssignableFrom<IKnownToken>(token);
        }

        [Fact]
        public void SymbolsShouldBeKnown()
        {
            foreach (var token in KnownTokens.GetSymbols())
                Assert.IsAssignableFrom<IKnownToken>(token);
        }

    }
}
