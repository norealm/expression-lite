// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.Error
{
    /// <summary>
    /// Extension methods to support <see cref="ExpressionLiteException"/>
    /// </summary>
    public static class ExpressionLiteExceptionExtensions
    {
        /// <summary>
        /// Create an <see cref="ExpressionLiteException"/> instance
        /// </summary>
        /// <param name="errorSource">error source</param>
        /// <param name="errorCode">error code</param>
        /// <param name="message">error message</param>
        /// <param name="args">error message arguments</param>
        /// <returns>exception instance</returns>
        public static ExpressionLiteException ToException(this ErrorSource errorSource, int errorCode, string message, params object[] args)
            => new(string.Format(message, args)) {ErrorSource = errorSource, ErrorCode = errorCode};

        /// <summary>
        /// Create and throw <see cref="ExpressionLiteException"/> instance
        /// </summary>
        /// <param name="errorSource">error source</param>
        /// <param name="errorCode">error code</param>
        /// <param name="message">error message</param>
        /// <param name="args">error message arguments</param>
        public static void Throw(this ErrorSource errorSource, int errorCode, string message, params object[] args)
            => throw new ExpressionLiteException(string.Format(message, args)) {ErrorSource = errorSource, ErrorCode = errorCode};

        /// <summary>
        /// Create an <see cref="ExpressionLiteException"/> instance from input error definition
        /// </summary>
        /// <param name="error">error information</param>
        /// <param name="errorSource">error source</param>
        /// <param name="args">error message arguments</param>
        /// <returns>exception instance</returns>
        internal static ExpressionLiteException ToException(this ErrorDefinition error, ErrorSource errorSource, params object[] args)
            => ToException(errorSource, error.Code, error.Message, args);

        /// <summary>
        /// Create and throw <see cref="ExpressionLiteException"/> instance from input error definition
        /// </summary>
        /// <param name="error">error information</param>
        /// <param name="errorSource">error source</param>
        /// <param name="args">error message arguments</param>
        /// <returns>exception instance</returns>
        internal static void Throw(this ErrorDefinition error, ErrorSource errorSource, params object[] args)
            => Throw(errorSource, error.Code, error.Message, args);
    }
}
