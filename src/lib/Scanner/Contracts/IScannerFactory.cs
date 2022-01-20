// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// A factory for creating <see cref="IScanner"/> instance
    /// </summary>
    public interface IScannerFactory
    {
        /// <summary>
        /// Get scanner instance using input <see cref="ScannerOptions"/> and default scanner lexeme converter
        /// </summary>
        /// <param name="scannerOptions">scanner options</param>
        /// <returns>scanner instance</returns>
        IScanner GetScanner(ScannerOptions scannerOptions);

        /// <summary>
        /// Get scanner instance using input <see cref="ScannerOptions"/> and <see cref="ILexemeConverter"/>
        /// </summary>
        /// <param name="scannerOptions">scanner options</param>
        /// <param name="lexemeConverter">lexeme converter</param>
        /// <returns>scanner instance</returns>
        IScanner GetScanner(ScannerOptions scannerOptions, ILexemeConverter lexemeConverter);
    }
}
