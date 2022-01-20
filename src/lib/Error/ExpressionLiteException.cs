// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Error
{
    /// <summary>
    /// A generic exception class for the expression lite
    /// </summary>
    [Serializable]
    public class ExpressionLiteException : Exception
    {
        /// <summary>
        /// initialize new instance
        /// </summary>
        /// <param name="message">exception message</param>
        public ExpressionLiteException(string message) : base(message)
        {
        }

        /// <summary>
        /// Get error source
        /// </summary>
        public ErrorSource ErrorSource { get; init; }

        /// <summary>
        /// Get error code
        /// </summary>
        public int ErrorCode { get; init; }
    }
}
