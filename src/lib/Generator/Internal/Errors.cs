// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;

namespace NoRealm.ExpressionLite
{
    internal static partial class Errors
    {
        internal static readonly ErrorDefinition InvalidLinqExpressionParameterType =
            new(0x3000001, "a linq expression for '{0}' must have a type of '{1}', found '{2}'.");

        internal static readonly ErrorDefinition IncorrectLinqExpressionArguments =
            new(0x3000002, "a linq expression for '{0}' must have only one argument of type '{1}'.");
    }
}
