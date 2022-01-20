// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Converts a lexeme to its corresponding value
    /// </summary>
    public interface ILexemeConverter
    {
        /// <summary>
        /// Convert input lexeme to requested type
        /// </summary>
        /// <param name="lexeme">string lexeme</param>
        /// <param name="targetType">target type</param>
        /// <returns>input lexeme converted to its type; null for invalid input</returns>
        object Convert(string lexeme, Type targetType);
    }
}
