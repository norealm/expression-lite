// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;
using Xunit;

namespace NoRealm.ExpressionLite.Test.Token
{
    public class DefaultLexemeConverterTests
    {
        private readonly ILexemeConverter converter = new DefaultLexemeConverter();

        [Fact]
        public void ConvertStringShouldBeOK()
        {
            var str = "\"  some \\n string \"";
            var result = converter.Convert(str, KnownTypes.String);

            Assert.IsType<string>(result);
            Assert.Equal(str, result);
        }

        [Fact]
        public void ConvertNumericShouldBeOK()
        {
            var num = $"{3.14e+1m}";
            var result = converter.Convert(num, KnownTypes.Number);

            Assert.IsType<decimal>(result);
            Assert.Equal(num, result.ToString());
        }

        [Fact]
        public void ConvertDateTimeShouldBeOK()
        {
            var now = $"{DateTime.Now}";

            var result = converter.Convert(now, KnownTypes.DateTime);

            Assert.IsType<DateTime>(result);
            Assert.Equal(now, result.ToString());
        }
    }
}
