// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent token group
    /// </summary>
    public enum TokenGroup : ushort
    {
        /// <summary>
        /// Token is a Whitespace
        /// </summary>
        Whitespace = 0x01,

        /// <summary>
        /// Token is an Identifier
        /// </summary>
        Identifier = 0x02,

        /// <summary>
        /// Token is a Keyword
        /// </summary>
        Keyword = 0x03,

        /// <summary>
        /// Token is a Literal
        /// </summary>
        Literal = 0x04,

        /// <summary>
        /// Token is a Symbol
        /// </summary>
        Symbol = 0x05
    }
}
