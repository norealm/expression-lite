// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Scanner;
using System;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// Default implementation for <see cref="IParserFactory"/>
    /// </summary>
    public class ParserFactory : IParserFactory
    {
        /// <summary>
        /// Initialize new instance
        /// </summary>
        public ParserFactory()
        {
        }

        /// <inheritdoc />
        public IParser GetParser(ParserOptions parserOptions)
        {
            if (parserOptions == null)
                throw new ArgumentNullException(nameof(parserOptions));

            return new ExpressionParser(parserOptions, null, null);
        }

        /// <inheritdoc />
        public IParser GetParser(ParserOptions parserOptions, IScanner scanner,
            IEnumerable<INameProvider> nameProviders)
        {
            if (parserOptions == null)
                throw new ArgumentNullException(nameof(parserOptions));

            if (scanner == null)
                throw new ArgumentNullException(nameof(scanner));

            if (nameProviders == null)
                throw new ArgumentNullException(nameof(nameProviders));

            return new ExpressionParser(parserOptions, nameProviders, scanner);
        }
    }
}
