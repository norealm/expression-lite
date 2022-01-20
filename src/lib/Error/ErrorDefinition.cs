// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Error
{
    /// <summary>
    /// Represent an error definition
    /// </summary>
    internal class ErrorDefinition
    {
        /// <summary>
        /// Get error code
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// Get error message
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// initialize new instance
        /// </summary>
        /// <param name="code">error code</param>
        /// <param name="message">error message</param>
        public ErrorDefinition(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
