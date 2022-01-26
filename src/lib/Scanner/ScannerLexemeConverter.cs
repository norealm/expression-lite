// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;
using System.Text;

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// A lexeme converter used by expression scanner
    /// </summary>
    public class ScannerLexemeConverter : DefaultLexemeConverter
    {
        /// <summary>
        /// replace the escape sequences with corresponding values, and remove trailing double quotes
        /// </summary>
        /// <param name="value">string value to process</param>
        /// <returns>a processed string</returns>
        protected override string ProcessString(string value)
        {
            var str = new StringBuilder();

            // skip start and end double-quotes
            for (var i = 1; i < value.Length - 1; ++i)
            {
                if (value[i] != '\\')
                {
                    str.Append(value[i]);
                    continue;
                }

                switch (value[i + 1])
                {
                    case 'n':
                        str.Append('\n');
                        break;
                    case '"' or '\\':
                        str.Append(value[i + 1]);
                        break;
                    default:
                        str.Append('\\');
                        str.Append(value[i + 1]);
                        break;
                }

                ++i;
            }

            return str.ToString();
        }

        /// <summary>
        /// remove the datetime marker and double quotes then convert the value to date time
        /// </summary>
        /// <param name="lexeme">lexeme to convert</param>
        /// <returns>the datetime value</returns>
        protected override DateTime? ConvertDateTime(string lexeme)
            => base.ConvertDateTime(lexeme.Substring(1, lexeme.Length - 2));
    }
}
