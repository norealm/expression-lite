// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Naming
{
    /// <summary>
    /// Represent a provider to fetch name information
    /// </summary>
    public interface INameProvider
    {
        /// <summary>
        /// Get name information
        /// </summary>
        /// <param name="name">requested name</param>
        /// <returns>name information; null if name not defined</returns>
        INameInfo GetNameInfo(string name);
    }
}
