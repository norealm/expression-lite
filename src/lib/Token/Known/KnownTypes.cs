// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Get known types
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// String data type
        /// </summary>
        public static readonly Type String = typeof(string);

        /// <summary>
        /// Numeric data type
        /// </summary>
        public static readonly Type Number = typeof(decimal);

        /// <summary>
        /// Date and time data type
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// Boolean data type
        /// </summary>
        public static readonly Type Boolean = typeof(bool);

        /// <summary>
        /// Get all known types
        /// </summary>
        /// <returns>all known types</returns>
        public static Type[] GeTypes()
            => new[] { String, Number, DateTime, Boolean };

        /// <summary>
        /// Determine if a type is a known type
        /// </summary>
        /// <param name="type">type to check</param>
        /// <returns>true if known type, false otherwise</returns>
        public static bool IsKnown(this Type type)
            => type == String || type == Number || type == DateTime || type == Boolean;

        /// <summary>
        /// Get a type of value from predefined types
        /// </summary>
        /// <param name="value">value to get type</param>
        /// <returns>value type if type is known type; null otherwise</returns>
        internal static Type GetKnownType(this object value)
        {
            var type = value.GetType();
            return type.IsKnown() ? type : null;
        }

        /// <summary>
        /// check whether the type is numeric
        /// </summary>
        /// <param name="type">type information</param>
        /// <returns>true for numeric type; false otherwise</returns>
        internal static bool IsNumber(this Type type)
            => type == typeof(sbyte)
               || type == typeof(byte)
               || type == typeof(short)
               || type == typeof(ushort)
               || type == typeof(int)
               || type == typeof(uint)
               || type == typeof(long)
               || type == typeof(ulong)
               || type == typeof(float)
               || type == typeof(double)
               || type == typeof(decimal);
    }
}
