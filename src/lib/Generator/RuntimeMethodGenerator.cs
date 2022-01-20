// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Parser;
using System;

namespace NoRealm.ExpressionLite.Generator
{
    /// <summary>
    /// Represent a generator which convert the expression tree into a runtime function
    /// </summary>
    public sealed class RuntimeMethodGenerator : IGenerator<Delegate>
    {
        /// <inheritdoc />
        public Delegate Generate(IParsingResult parsingResult)
            => parsingResult.CreateExpression(parsingResult.Expression.StaticType).Compile();

        /// <inheritdoc />
        public Delegate Generate(IParsingResult result, Type input)
            => result.CreateExpression(input, result.Expression.StaticType).Compile();

        /// <summary>
        /// Generate a runtime method which have a result of input type
        /// </summary>
        /// <typeparam name="T">result type, must be one of the primitive types</typeparam>
        /// <param name="parsingResult">the parsing results to use for generation</param>
        /// <returns>the runtime method which match input parser results</returns>
        public Func<T> Generate<T>(IParsingResult parsingResult)
            => (Func<T>)parsingResult.CreateExpression(typeof(T)).Compile();

        /// <summary>
        /// Generate a runtime method which have a user input
        /// </summary>
        /// <typeparam name="T">user input type</typeparam>
        /// <typeparam name="TR">result type, must be one of the primitive types</typeparam>
        /// <param name="parsingResult">the parsing results to use for generation</param>
        /// <returns>the runtime method which match input parser results</returns>
        public Func<T, TR> Generate<T, TR>(IParsingResult parsingResult)
            => (Func<T, TR>)parsingResult.CreateExpression(typeof(T), typeof(TR)).Compile();
    }
}
