// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Error
{
    /// <summary>
    /// The source of error
    /// </summary>
    public enum ErrorSource
    {
        /// <summary>
        /// Scanner component
        /// </summary>
        Scanner = 0x1000000,

        /// <summary>
        /// Parser component
        /// </summary>
        Parser = 0x2000000,

        /// <summary>
        /// generator component
        /// </summary>
        Generator = 0x3000000
    }
}
