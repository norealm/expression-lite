// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent a predefined token
    /// </summary>
    public interface IKnownToken : IToken
    {
        /// <summary>
        /// Get token id
        /// </summary>
        ushort Id { get; }
    }
}
