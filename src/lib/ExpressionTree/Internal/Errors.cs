// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;

namespace NoRealm.ExpressionLite
{
    internal static partial class Errors
    {
        internal static readonly ErrorDefinition UnknownExpressionNode =
            new(0x2000001, "unknown expression node found for token '{0}'.");

        internal static readonly ErrorDefinition EmptyArray =
            new(0x2000002, "empty array is not allowed.");

        internal static readonly ErrorDefinition IfExpressionInvalidTypes =
            new(0x2000003, "true/false expressions must have same static type.");

        internal static readonly ErrorDefinition Expected =
            new(0x2000004, "expected '{0}', found '{1}'.");

        internal static readonly ErrorDefinition DivideByZero =
            new(0x2000005, "a division by zero will result in undefined behavior.");
    }
}
