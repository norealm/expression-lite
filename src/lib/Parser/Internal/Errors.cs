// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;

namespace NoRealm.ExpressionLite
{
    internal static partial class Errors
    {
        internal static readonly ErrorDefinition ExpectedEndOfStream =
            new(0x3100001, "extra token(s) found when expecting end of stream.");

        internal static readonly ErrorDefinition IdentifierSelfReference =
            new(0x3100002, "the identifier '{0}' has a reference to itself directly or indirectly.");

        internal static readonly ErrorDefinition MissingIdentifierDefinition =
            new(0x3100003, "the identifier '{0}' is being used before defining a value.");

        internal static readonly ErrorDefinition UnknownNameInfoType =
            new(0x3100004, "a custom INameInfo implementation is not supported, please consider using a member of NameInfo class.");

        internal static readonly ErrorDefinition UnknownIdentifier =
            new(0x3100005, "the identifier '{0}' has been used without being defined in a name provider.");

        internal static readonly ErrorDefinition UnexpectedEndOfTokens =
            new(0x3100006, "unexpected end of tokens.");
    }
}
