// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Represent a token
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Get token group
        /// </summary>
        TokenGroup Group { get; }

        /// <summary>
        /// Get token lexeme
        /// </summary>
        string Lexeme { get; }
    }
}
