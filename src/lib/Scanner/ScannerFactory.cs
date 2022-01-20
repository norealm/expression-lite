// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using System;

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// Default implementation for <see cref="IScannerFactory"/>
    /// </summary>
    public class ScannerFactory : IScannerFactory
    {
        /// <summary>
        /// The default literal converter instance
        /// </summary>
        protected readonly ILexemeConverter DefaultLexemeConverter;

        /// <summary>
        /// Initialize new instance with <see cref="ScannerLexemeConverter"/> as Default lexeme converter
        /// </summary>
        public ScannerFactory()
            => DefaultLexemeConverter = new ScannerLexemeConverter();

        /// <summary>
        /// Initialize new instance with input <see cref="ILexemeConverter"/> as Default lexeme converter
        /// </summary>
        /// <param name="lexemeConverter">the lexeme converter to use as default converter</param>
        public ScannerFactory(ILexemeConverter lexemeConverter)
            => DefaultLexemeConverter = lexemeConverter ?? throw new ArgumentNullException(nameof(lexemeConverter));

        /// <inheritdoc />
        public IScanner GetScanner(ScannerOptions scannerOptions)
            => GetScanner(scannerOptions, DefaultLexemeConverter);

        /// <inheritdoc />
        public IScanner GetScanner(ScannerOptions scannerOptions, ILexemeConverter lexemeConverter)
        {
            if (scannerOptions == null)
                throw new ArgumentNullException(nameof(scannerOptions));
            
            if (lexemeConverter == null)
                throw new ArgumentNullException(nameof(lexemeConverter));

            return new ExpressionScanner(scannerOptions, lexemeConverter);
        }
    }
}
