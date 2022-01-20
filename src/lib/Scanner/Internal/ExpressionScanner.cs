// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.Token;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoRealm.ExpressionLite.Scanner
{
    /// <summary>
    /// An Infix expression scanner
    /// </summary>
    internal sealed class ExpressionScanner : IScanner
    {
        private readonly IReadOnlyDictionary<string, IKnownToken> keywordMap;
        private readonly IReadOnlyDictionary<string, IKnownToken> symbolMap;

        private readonly ILexemeConverter lexemeConverter;
        private readonly ScannerOptions scannerOptions;
        private readonly Func<ScannerContext, IToken>[] scanners;

        /// <summary>
        /// Initialize new instance
        /// </summary>
        /// <param name="scannerOptions">the scanner options</param>
        /// <param name="lexemeConverter">a lexeme converter instance</param>
        internal ExpressionScanner(ScannerOptions scannerOptions, ILexemeConverter lexemeConverter)
        {
            this.lexemeConverter = lexemeConverter ?? throw new ArgumentNullException(nameof(lexemeConverter));
            this.scannerOptions = scannerOptions ?? throw new ArgumentNullException(nameof(scannerOptions));

            scanners = InitializeScannerStages();
            symbolMap = KnownTokens.GetSymbols().ToDictionary(e => e.Lexeme);
            keywordMap = KnownTokens.GetKeywords().ToDictionary(e => e.Lexeme);
        }

        /// <inheritdoc />
        public ScannerOptions Options => scannerOptions;

        /// <inheritdoc />
        public IEnumerable<IToken> Scan(string sourceText)
        {
            if (string.IsNullOrWhiteSpace(sourceText))
                yield break;

            var context = new ScannerContext {SourceText = sourceText, Options = scannerOptions};

            while (context.CurrentIndex < context.SourceText.Length)
            {
                var wsToken = context.SkipWhitespaces();
                
                if (wsToken != null)
                    yield return wsToken;

                if (context.CurrentIndex >= context.SourceText.Length) yield break;
                var isFound = false;

                for (var i = 0; i < scanners.Length; ++i)
                {
                    var token = scanners[i](context);

                    if (token == null) continue;

                    isFound = true;
                    yield return token;
                    break;
                }

                if (!isFound)
                    throw Errors.UnknownTokenAtIndex.ToException(ErrorSource.Scanner, context.CurrentIndex);
            }
        }

        #region helper methods

        private Func<ScannerContext, IToken>[] InitializeScannerStages()
        {
            return new Func<ScannerContext, IToken>[]
            {
                GetSymbolToken,
                GetStringLiteralToken,
                GetDateTimeLiteralToken,
                GetNumericLiteralToken,
                GetIdentifierOrKeywordToken
            };
        }

        private bool TryGetBoundedValue(string sourceText, int startIndex, Func<int, bool> isBound, out int lastIndex)
        {
            lastIndex = startIndex + 1;
            var isClosed = false;

            while (lastIndex < sourceText.Length)
            {
                isClosed = isBound(lastIndex);

                if (isClosed) break;
                ++lastIndex;
            }

            return isClosed;
        }

        private string GetBoundedToken(ScannerContext context, char boundChar, Exception ex, Func<int, bool> isBound)
        {
            var sourceText = context.SourceText;
            var currentIndex = context.CurrentIndex;

            if (sourceText[currentIndex] != boundChar)
                return null;

            if (!TryGetBoundedValue(sourceText, currentIndex, isBound, out var lastIndex))
                throw ex;

            context.GetLexeme(currentIndex, lastIndex - currentIndex + 1, out var lexeme);

            context.CurrentIndex = lastIndex + 1;
            return lexeme;
        }

        #endregion

        #region scanners

        private IToken GetSymbolToken(ScannerContext context)
        {
            IKnownToken token;

            if (context.GetLexeme(context.CurrentIndex, 2, out var @operator))
            {
                if (symbolMap.TryGetValue(@operator, out token))
                {
                    context.CurrentIndex += 2;
                    return token;
                }
            }

            @operator = $"{context.SourceText[context.CurrentIndex]}";

            if (symbolMap.TryGetValue(@operator, out token))
            {
                context.CurrentIndex += 1;
                return token;
            }

            return null;
        }

        private IToken GetStringLiteralToken(ScannerContext context)
        {
            var lexeme = GetBoundedToken(context, '"', Errors.UnterminatedString.ToException(ErrorSource.Scanner), IsBound);

            if (lexeme == null)
                return null;

            return Tokens.StringLiteral(lexeme, (string) lexemeConverter.Convert(lexeme, KnownTypes.String));

            bool IsBound(int index) => (context.SourceText[index] == '"' && (index == 0 || context.SourceText[index - 1] != '\\'));
        }

        private IToken GetDateTimeLiteralToken(ScannerContext context)
        {
            var currentIndex = context.CurrentIndex;
            var lexeme = GetBoundedToken(context, '#', Errors.UnterminatedDateTime.ToException(ErrorSource.Scanner), IsBound);

            if (lexeme == null)
                return null;

            var value = lexemeConverter.Convert(lexeme, KnownTypes.DateTime);

            if (value == null)
                throw Errors.InvalidDateTimeFormat.ToException(ErrorSource.Scanner, currentIndex);

            return Tokens.DateTimeLiteral(lexeme, (DateTime)value);

            bool IsBound(int index) => context.SourceText[index] == '#';
        }

        private IToken GetNumericLiteralToken(ScannerContext context)
        {
            var ch = context.SourceText[context.CurrentIndex];

            if (ch != '.' && !ch.IsDigit())
                return null;

            var startIndex = context.CurrentIndex;
            var lastIndex = context.CurrentIndex;

            if (!ExtractNumber(context, ref lastIndex))
                throw Errors.InvalidNumberFormat.ToException(ErrorSource.Scanner, context.CurrentIndex);

            context.GetLexeme(startIndex, lastIndex - startIndex, out var lexeme);

            var value = lexemeConverter.Convert(lexeme, KnownTypes.Number);

            if (value == null)
                throw Errors.InvalidNumberFormat.ToException(ErrorSource.Scanner, context.CurrentIndex);

            context.CurrentIndex = lastIndex;

            return Tokens.NumericLiteral(lexeme, (decimal) value);

            static bool ExtractNumber(ScannerContext context, ref int index)
            {
                /*
                 * extract the number using the following regex format
                 *   1. [0-9]* . [0-9]+ ([Ee] [+-]? [0-9]+)?
                 *   2. [0-9]+ ([Ee] [+-]? [0-9]+)?
                 */

                // match  [0-9]*
                if (!ExtractDigits(context, ref index, true))
                    return false;

                // it is ok to stop here
                if (index == context.SourceText.Length)
                    return true;

                var haveStartDigits = context.CurrentIndex != index;

                // match .
                if (context.SourceText[index] == '.')
                {
                    // we cannot stop here
                    if (++index == context.SourceText.Length)
                        return false;

                    // match [0-9]+
                    if (!ExtractDigits(context, ref index))
                        return false;
                }
                else
                {
                    // match [0-9]+, we can stop here
                    if (!ExtractDigits(context, ref index, haveStartDigits))
                        return false;
                }

                // it is ok to stop here
                if (index == context.SourceText.Length)
                    return true;

                // match [Ee], it is OK to not have it
                if (context.SourceText[index] is not ('E' or 'e'))
                    return true;

                // match[+-], we cannot stop here
                if (++index == context.SourceText.Length || context.SourceText[index] is not ('+' or '-'))
                    return false;

                // we cannot stop here
                if (index == context.SourceText.Length)
                    return false;

                // match [0-9]+
                return ExtractDigits(context, ref index);
            }

            static bool ExtractDigits(ScannerContext context, ref int index, bool isOptional = false)
            {
                var fallbackIndex = index;

                while (index < context.SourceText.Length && context.SourceText[index].IsDigit())
                    ++index;

                return isOptional || fallbackIndex != index;
            }
        }

        private IToken GetIdentifierOrKeywordToken(ScannerContext context)
        {
            var currentIndex = context.CurrentIndex;

            if (context.SourceText[currentIndex].IsIdentifierStart())
                ++currentIndex;
            else
                return null;

            while (currentIndex < context.SourceText.Length && context.SourceText[currentIndex].IsIdentifierBody())
                ++currentIndex;

            context.GetLexeme(context.CurrentIndex, currentIndex - context.CurrentIndex, out var lexeme);
            context.CurrentIndex = currentIndex;

            if (keywordMap.TryGetValue(lexeme, out var token))
                return token;

            if (lexeme.Length > context.Options.MaxIdentifierLength)
                throw Errors.LongIdentifier.ToException(ErrorSource.Scanner, lexeme, context.Options.MaxIdentifierLength);

            return Tokens.Identifier(lexeme);
        }

        #endregion

        #region scanner helper types

        private class ScannerContext
        {
            internal string SourceText;
            internal int CurrentIndex;
            internal ScannerOptions Options;

            internal IToken SkipWhitespaces()
            {
                var index = CurrentIndex;
                var count = 0;

                while (index < SourceText.Length && char.IsWhiteSpace(SourceText[index]))
                {
                    ++index;
                    ++count;
                }

                CurrentIndex = index;

                if (count == 0 || Options.IgnoreWhitespaceToken)
                    return null;
                else
                    return Tokens.Whitespace(count);
            }

            internal bool GetLexeme(int startIndex, int length, out string value)
            {
                if (startIndex + length > SourceText.Length)
                {
                    value = null;
                    return false;
                }

                value = SourceText.Substring(startIndex, length);
                return true;
            }
        }

        #endregion
    }
}
