// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Naming
{
    internal sealed class InternalNameInfo : INameInfo
    {
        public string Name { get; internal set; }
        public object Value { get; internal set; }
        public Type StaticType { get; internal set; }
        public NameType NameType { get; internal set; }
    }
}
