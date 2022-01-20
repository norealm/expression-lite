// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Naming
{
    /// <summary>
    /// Custom name content type
    /// </summary>
    public enum NameType
    {
        /// <summary>
        /// A plain value
        /// </summary>
        Plain,

        /// <summary>
        /// An expression to get scanned
        /// </summary>
        Expression,

        /// <summary>
        /// A (static/instance) property/field
        /// </summary>
        Member,

        /// <summary>
        /// A linq expression
        /// </summary>
        LinqExpression
    }
}
