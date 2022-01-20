// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;

namespace NoRealm.ExpressionLite
{
    internal static partial class Errors
    {
        internal static readonly ErrorDefinition UnknownTokenAtIndex =
            new(0x1100001, "unknown token found at index {0}");

        internal static readonly ErrorDefinition UnterminatedString =
            new(0x1100002, "unterminated string literal detected.");
        
        internal static readonly ErrorDefinition UnterminatedDateTime =
            new(0x1100003, "unterminated date time literal detected.");
        
        internal static readonly ErrorDefinition InvalidDateTimeFormat =
            new(0x1100004, "invalid date time literal at index {0}.");
        
        internal static readonly ErrorDefinition InvalidNumberFormat =
            new(0x1100005, "invalid numeric literal at index {0}.");
        
        internal static readonly ErrorDefinition LongIdentifier =
            new(0x1100006, "the identifier '{0}' have a long name, max allowed size is {1} character(s).");
    }
}
