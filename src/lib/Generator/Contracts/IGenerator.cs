// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Parser;
using System;

namespace NoRealm.ExpressionLite.Generator
{
    /// <summary>
    /// Responsible for generating expression output
    /// </summary>
    public interface IGenerator<out T>
    {
        /// <summary>
        /// Generate the output <typeparamref name="T"/> from input <see cref="IParsingResult"/>
        /// </summary>
        /// <param name="parsingResult">the parsing results</param>
        /// <returns>the output</returns>
        T Generate(IParsingResult parsingResult);

        /// <summary>
        /// Generate the output <typeparamref name="T"/> from input <see cref="IParsingResult"/>
        /// </summary>
        /// <param name="parsingResult">the parsing results</param>
        /// <param name="inType">type to bind the generated code with</param>
        /// <returns>the output</returns>
        T Generate(IParsingResult parsingResult, Type inType);
    }
}
