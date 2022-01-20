// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.Token;
using System.Collections.Generic;
using System.Linq;

namespace NoRealm.ExpressionLite.Parser
{
    internal sealed class ParsingContext
    {
        public readonly Dictionary<string, NameDetails> Names = new();
        public IToken[] Tokens;
        public int CurrentIndex = 0;

        public void Advance()
            => ++CurrentIndex;

        public IToken CurrentToken
            => CurrentIndex >= Tokens.Length ? null : Tokens[CurrentIndex];

        public bool IsEOS => CurrentIndex >= Tokens.Length;

        public bool MatchAny(params IToken[] tokens)
        {
            var t = CurrentToken;

            if (t is IKnownToken known)
            {
                foreach (var token in tokens)
                {
                    if (token is IKnownToken kt && known.Group == kt.Group && known.Id == kt.Id)
                        return true;

                    if (token.Group == t.Group && token.Lexeme == t.Lexeme) return true;
                }

                return false;
            }

            return tokens.Any(token => token.Group == t.Group && token.Lexeme == t.Lexeme);
        }

        public bool Match(IToken token)
            => MatchAny(token);

        public void MatchThrow(IToken token)
        {
            if (!MatchAny(token))
                throw Errors.Expected.ToException(ErrorSource.Parser, token.Lexeme, CurrentToken.Lexeme);

            Advance();
        }

        public IDictionary<int, NameDetails> GetUsedNames()
            => Names.Values.Where(details => details.ReferenceCount > 0).ToDictionary(details => details.Id);
    }
}
