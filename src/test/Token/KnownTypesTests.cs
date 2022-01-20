// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;
using Xunit;

namespace NoRealm.ExpressionLite.Test.Token
{
    public class KnownTypesTests
    {
        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(DateTime))]
        public void TypeShouldBeKnownType(Type type)
        {
            Assert.True(type.IsKnown());
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(short))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(ulong))]
        public void TypeShouldBeUnknown(Type type)
        {
            Assert.False(type.IsKnown());
        }
    }
}
