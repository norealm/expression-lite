// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Token;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// the name details
    /// </summary>
    public sealed class NameDetails
    {
        /// <summary>
        /// Get name key
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The name information
        /// </summary>
        public INameInfo Name { get; init; }

        /// <summary>
        /// Get number of times this name is referenced
        /// </summary>
        public int ReferenceCount { get; set; }

        internal IToken[] Original;
        internal IToken[] Expanded;
    }
}
