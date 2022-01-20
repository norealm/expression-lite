// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Token
{
    /// <summary>
    /// Default implementation for <see cref="ILexemeConverter"/>
    /// </summary>
    public class DefaultLexemeConverter : ILexemeConverter
    {
        /// <inheritdoc />
        public virtual object Convert(string lexeme, Type targetType)
        {
            if (lexeme == null) return null;

            if (targetType == KnownTypes.String)
                return ConvertString(lexeme);

            if (targetType == KnownTypes.DateTime)
                return ConvertDateTime(lexeme);

            if (targetType == KnownTypes.Number)
                return ConvertNumber(lexeme);

            return null;
        }

        #region string type

        /// <summary>
        /// Convert input value to string
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the converted string</returns>
        protected virtual string ConvertString(string lexeme)
            => lexeme == null ? null : ProcessString(lexeme);

        /// <summary>
        /// when overriden modify the string content before returned
        /// </summary>
        /// <param name="value">string to process</param>
        /// <returns>the final string</returns>
        protected virtual string ProcessString(string value)
            => value;

        #endregion

        #region number type

        /// <summary>
        /// Convert input value to decimal
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the converted number</returns>
        protected virtual decimal? ConvertNumber(string lexeme)
        {
            if (lexeme == null) return null;

            var content = LexemeToNumber(lexeme);
            if (content == null) return null;
            return ProcessNumber(content.Value);
        }

        /// <summary>
        /// when overriden convert string to decimal number
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the converted number; null for invalid format</returns>
        protected virtual decimal? LexemeToNumber(string lexeme)
            => decimal.TryParse(lexeme, out var n) ? n : null;

        /// <summary>
        /// when overriden modify the input number before returned
        /// </summary>
        /// <param name="value">value to process</param>
        /// <returns>the modified number</returns>
        protected virtual decimal ProcessNumber(decimal value)
            => value;

        #endregion

        #region datetime type

        /// <summary>
        /// Convert input value to datetime
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the converted datetime</returns>
        protected virtual DateTime? ConvertDateTime(string lexeme)
        {
            if (lexeme == null) return null;

            var content = LexemeToDateTime(lexeme);
            if (content == null) return null;
            return ProcessDateTime(content.Value);
        }
        
        /// <summary>
        /// when overriden convert string to datetime value
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the converted value; null for invalid format</returns>
        protected virtual DateTime? LexemeToDateTime(string lexeme)
            => DateTime.TryParse(lexeme, out var dt) ? dt : null;

        /// <summary>
        /// when overriden modify the input value before returned
        /// </summary>
        /// <param name="value">value to process</param>
        /// <returns>the modified value</returns>
        protected virtual DateTime ProcessDateTime(DateTime value)
            => value;

        #endregion
    }
}
