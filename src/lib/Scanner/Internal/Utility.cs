// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// Utility functions to support <see cref="ExpressionScanner"/>
    /// </summary>
    internal static class Utility
    {
        public static bool IsLetter(this char c)
            => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z';

        public static bool IsDigit(this char c)
            => c is >='0' and <= '9';

        public static bool IsLetterOrDigit(this char c)
            => IsDigit(c) || IsLetter(c);

        public static bool IsIdentifierStart(this char c)
            => c is '_' or '$' or '@' || IsLetter(c);

        public static bool IsIdentifierBody(this char c)
            => IsIdentifierStart(c) || IsLetterOrDigit(c);
    }
}
