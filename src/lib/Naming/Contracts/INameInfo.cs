// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Naming
{
    /// <summary>
    /// Represent user-defined name information
    /// </summary>
    public interface INameInfo
    {
        /// <summary>
        /// Get identifier name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get value of identifier name
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Determine value static type
        /// </summary>
        Type StaticType { get; }

        /// <summary>
        /// Get name content type
        /// </summary>
        NameType NameType { get; }
    }
}
