// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Parser;
using System;
using System.Linq.Expressions;

namespace NoRealm.ExpressionLite.Generator
{
    /// <summary>
    /// Represent a generator which convert the expression tree into <see cref="Expression{TDelegate}"/>
    /// </summary>
    public sealed class LinqExpressionGenerator : IGenerator<LambdaExpression>
    {
        /// <inheritdoc />
        public LambdaExpression Generate(IParsingResult parsingResult)
            => parsingResult.CreateExpression(parsingResult.Expression.StaticType);

        /// <inheritdoc />
        public LambdaExpression Generate(IParsingResult parsingResult, Type inType)
            => parsingResult.CreateExpression(inType, parsingResult.Expression.StaticType);

        /// <summary>
        /// Generate a LINQ Expression which have no user input
        /// </summary>
        /// <typeparam name="TR">result type, must be one of the primitive types</typeparam>
        /// <param name="parsingResult">the parsing results to use for generation</param>
        /// <returns>the LINQ Expression which match input parser results</returns>
        public Expression<Func<TR>> Generate<TR>(IParsingResult parsingResult)
            => (Expression<Func<TR>>)parsingResult.CreateExpression(typeof(TR));

        /// <summary>
        /// Generate a LINQ Expression which have a user input
        /// </summary>
        /// <typeparam name="T">user input type</typeparam>
        /// <typeparam name="TR">result type, must be one of the primitive types</typeparam>
        /// <param name="parsingResult">the parsing results to use for generation</param>
        /// <returns>the LINQ Expression which match input parser results</returns>
        public Expression<Func<T, TR>> Generate<T, TR>(IParsingResult parsingResult)
            => (Expression<Func<T, TR>>)parsingResult.CreateExpression(typeof(T), typeof(TR));
    }
}
