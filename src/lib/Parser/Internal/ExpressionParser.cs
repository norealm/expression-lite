// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Error;
using NoRealm.ExpressionLite.ExpressionTree;
using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Scanner;
using NoRealm.ExpressionLite.Token;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoRealm.ExpressionLite.Parser
{
    internal sealed class ExpressionParser : IParser
    {
        private readonly INameProvider[] nameProviders;
        private readonly IScanner scanner;
        private readonly IReadOnlyDictionary<IToken, Func<IExpression, IExpression, IExpression>> binaryOp;

        /// <summary>
        /// Initialize new instance
        /// </summary>
        /// <param name="options">parser options</param>
        /// <param name="nameProviders">a sequence of name providers</param>
        /// <param name="scanner">the scanner factory</param>
        public ExpressionParser(ParserOptions options, IEnumerable<INameProvider> nameProviders, IScanner scanner)
        {
            Options = options;
            this.nameProviders = nameProviders?.ToArray();
            this.scanner = scanner;
            binaryOp = BuildBinaryOperators();
        }

        /// <inheritdoc />
        public ParserOptions Options { get; }

        /// <inheritdoc />
        public IParsingResult Parse(IEnumerable<IToken> tokens, string name)
        {
            var set = new HashSet<string>();
            if (name != null)
                set.Add(name);

            var context = new ParsingContext();
            context.Tokens = Expand(tokens.ToArray(), set, context.Names);

            var root = GetExpressionTree(context);
            root = new IdentifierOptimizer(Options, root, context.GetUsedNames()).Optimize();
            root = new GlobalOptimizer(Options, root).Optimize();

            return new ParsingResult
            {
                Expression = root,
                Names = context.GetUsedNames()
            };
        }

        #region sub-expression expansion

        private IToken[] Expand(IToken[] tokens, ISet<string> scopeNames, IDictionary<string, NameDetails> knownNames)
        {
            var range = new List<IToken>();

            foreach (var token in tokens)
            {
                if (token.Group == TokenGroup.Whitespace) continue;
                if (token.Group != TokenGroup.Identifier)
                {
                    range.Add(token);
                    continue;
                }

                if (scanner == null)
                    throw new NotSupportedException("illegal use of identifiers without a name provider and a scanner.");

                var identifierToken = (IdentifierToken)token;

                if (knownNames.TryGetValue(identifierToken.Lexeme, out var details))
                {
                    if (scopeNames.Contains(identifierToken.Lexeme))
                        throw Errors.IdentifierSelfReference.ToException(ErrorSource.Parser, identifierToken.Lexeme);

                    range.AddRange(details.Expanded ?? new[] {identifierToken});
                }
                else
                {
                    if (scopeNames.Contains(identifierToken.Lexeme))
                        throw Errors.IdentifierSelfReference.ToException(ErrorSource.Parser, identifierToken.Lexeme);

                    details = new NameDetails {Id = identifierToken.Lexeme.GetHashCode(), Name = GetName(identifierToken.Lexeme)};

                    if (details.Name == null)
                        throw Errors.MissingIdentifierDefinition.ToException(ErrorSource.Parser, identifierToken.Lexeme);

                    knownNames.Add(identifierToken.Lexeme, details);

                    if (details.Name.NameType != NameType.Expression)
                    {
                        range.Add(identifierToken);
                        continue;
                    }
                    else
                    {
                        details.Original = scanner.Scan((string)details.Name.Value).ToArray();

                        var finalTokens = new List<IToken> {KnownTokens.OpenParen};
                        finalTokens.AddRange(Expand(details.Original, new HashSet<string>(scopeNames) {identifierToken.Lexeme}, knownNames));
                        finalTokens.Add(KnownTokens.CloseParen);

                        details.Expanded = finalTokens.ToArray();
                    }

                    range.AddRange(details.Expanded);
                }
            }

            return range.ToArray();

            INameInfo GetName(string name)
            {
                foreach (var provider in nameProviders)
                {
                    var nameInfo = provider.GetNameInfo(name);
                    if (nameInfo != null)
                    {
                        if (nameInfo is not InternalNameInfo info)
                            throw Errors.UnknownNameInfoType.ToException(ErrorSource.Parser);

                        info.Name = name;

                        return info;
                    }
                }

                throw Errors.UnknownIdentifier.ToException(ErrorSource.Parser, name);
            }
        }

        #endregion

        #region expression tree generator

        private static IReadOnlyDictionary<IToken, Func<IExpression, IExpression, IExpression>> BuildBinaryOperators()
        {
            return new Dictionary<IToken, Func<IExpression, IExpression, IExpression>>
            {
                [KnownTokens.Multiply] = Expressions.Multiply,
                [KnownTokens.Divide] = Expressions.Divide,
                [KnownTokens.Reminder] = Expressions.Reminder,
                [KnownTokens.GreaterThan] = Expressions.GreaterThan,
                [KnownTokens.LessThan] = Expressions.LessThan,
                [KnownTokens.GreaterThanEqual] = Expressions.GreaterThanOrEquals,
                [KnownTokens.LessThanEqual] = Expressions.LessThanOrEquals,
                [KnownTokens.Equal] = Expressions.Equals,
                [KnownTokens.NotEqual] = Expressions.NotEquals
            };
        }

        private IExpression GetExpressionTree(ParsingContext context)
        {
            var exp = GetExpression(context);

            if (!context.IsEOS)
                throw Errors.ExpectedEndOfStream.ToException(ErrorSource.Parser);

            return exp;
        }

        private IExpression GetExpression(ParsingContext context)
            => GetOrExpression(context);

        private IExpression GetOrExpression(ParsingContext context)
        {
            var exp = GetAndExpression(context);

            while (!context.IsEOS && context.Match(KnownTokens.LogicOr))
            {
                context.Advance();
                exp = Expressions.Or(exp, GetAndExpression(context));
            }

            return exp;
        }

        private IExpression GetAndExpression(ParsingContext context)
        {
            var exp = GetEqualityExpression(context);

            while (!context.IsEOS && context.Match(KnownTokens.LogicAnd))
            {
                context.Advance();
                exp = Expressions.And(exp, GetEqualityExpression(context));
            }

            return exp;
        }

        private IExpression GetEqualityExpression(ParsingContext context)
        {
            var exp = GetRelationalExpression(context);

            while (!context.IsEOS && context.MatchAny(KnownTokens.Equal, KnownTokens.NotEqual))
            {
                var token = context.CurrentToken;
                context.Advance();
                exp = binaryOp[token](exp, GetRelationalExpression(context));
            }

            return exp;
        }

        private IExpression GetRelationalExpression(ParsingContext context)
        {
            var exp = GetAdditiveExpression(context);
            if (context.IsEOS) return exp;

            var inOrHave = GetInOrHaveExpression(context, exp);
            if (inOrHave != null) return inOrHave;

            while (!context.IsEOS && context.MatchAny(KnownTokens.GreaterThan, KnownTokens.GreaterThanEqual,
                       KnownTokens.LessThan, KnownTokens.LessThanEqual))
            {
                var token = context.CurrentToken;
                context.Advance();
                exp = binaryOp[token](exp, GetAdditiveExpression(context));
            }

            return exp;
        }

        private IExpression GetInOrHaveExpression(ParsingContext context, IExpression left)
        {
            var isNot = context.Match(KnownTokens.LogicNot);
            if (isNot) context.Advance();

            if (context.IsEOS)
                throw Errors.UnexpectedEndOfTokens.ToException(ErrorSource.Parser);

            if (context.Match(KnownTokens.In))
            {
                var list = new List<IExpression>();

                context.Advance();
                context.MatchThrow(KnownTokens.OpenBracket);

                while (true)
                {
                    var t = GetAdditiveExpression(context);
                    list.Add(t);

                    if (context.Match(KnownTokens.Comma))
                    {
                        context.Advance();
                        continue;
                    }

                    break;
                }

                context.MatchThrow(KnownTokens.CloseBracket);
                return Expressions.InNotIn(isNot ? NodeType.NotIn : NodeType.In, left, Expressions.Array(list));
            }
            
            if (context.Match(KnownTokens.Have))
            {
                context.Advance();
                return Expressions.HaveNotHave(isNot? NodeType.NotHave: NodeType.Have, left, GetAdditiveExpression(context));
            }
            
            if (isNot)
            {
                throw Errors.Expected.ToException(ErrorSource.Parser, "in or have",
                    context.IsEOS ? "<end of tokens>" : context.CurrentToken.Lexeme);
            }

            return null;
        }

        private IExpression GetAdditiveExpression(ParsingContext context)
        {
            var exp = GetMultiplicativeExpression(context);

            while (!context.IsEOS && context.MatchAny(KnownTokens.Plus, KnownTokens.Minus))
            {
                var token = context.CurrentToken;
                context.Advance();
                var exp2 = GetMultiplicativeExpression(context);

                if (token == KnownTokens.Minus)
                {
                    exp = Expressions.Subtract(exp, exp2);
                    continue;
                }

                exp = exp.StaticType == KnownTypes.String ?
                    Expressions.Append(exp, exp2) :
                    Expressions.Add(exp, exp2);
            }

            return exp;
        }

        private IExpression GetMultiplicativeExpression(ParsingContext context)
        {
            var exp = GetUnaryExpression(context);

            while (!context.IsEOS && context.MatchAny(KnownTokens.Multiply, KnownTokens.Divide, KnownTokens.Reminder))
            {
                var token = context.CurrentToken;
                context.Advance();
                exp = binaryOp[token](exp, GetUnaryExpression(context));
            }

            return exp;
        }

        private IExpression GetUnaryExpression(ParsingContext context)
        {
            IExpression exp = null;

            while(!context.IsEOS && context.MatchAny(KnownTokens.Plus, KnownTokens.Minus, KnownTokens.LogicNot))
            {
                var token = context.CurrentToken;
                context.Advance();
                var exp2 = GetUnaryExpression(context);

                if (exp2.NodeType != NodeType.UnaryMinus && exp2.NodeType != NodeType.UnaryPlus &&
                    exp2.NodeType != NodeType.LogicNot)
                {
                    if (token == KnownTokens.Plus)
                        exp = Expressions.Plus(exp2);
                    else if (token == KnownTokens.Minus)
                        exp = Expressions.Negate(exp2);
                    else
                        exp = Expressions.Not(exp2);

                    break;
                }
            }

            return exp ?? GetPrimaryExpression(context);
        }

        private IExpression GetIfExpression(ParsingContext context)
        {
            context.MatchThrow(KnownTokens.If);
            context.MatchThrow(KnownTokens.OpenParen);

            var conditionExp = GetOrExpression(context);
            context.MatchThrow(KnownTokens.Comma);

            var trueExp = GetExpression(context);
            context.MatchThrow(KnownTokens.Comma);

            var falseExp = GetExpression(context);
            context.MatchThrow(KnownTokens.CloseParen);

            return Expressions.If(conditionExp, trueExp, falseExp);
        }

        private IExpression GetPrimaryExpression(ParsingContext context)
        {
            IExpression exp;
            var token = context.CurrentToken;

            if (token == KnownTokens.If)
                return GetIfExpression(context);

            if (token == KnownTokens.OpenParen)
            {
                context.Advance();
                exp = GetExpression(context);
                context.MatchThrow(KnownTokens.CloseParen);
                return exp;
            }

            if (token.Group == TokenGroup.Identifier)
            {
                context.Advance();

                var details = context.Names[((IdentifierToken)token).Lexeme];
                ++details.ReferenceCount;
                return Expressions.Identifier(details.Id, details.Name.StaticType);
            }

            exp = GetConstant(context);

            if (exp == null)
                throw Errors.Expected.ToException(ErrorSource.Parser, "<expression, constant, identifier>", token.Lexeme);

            return exp;
        }

        private IExpression GetConstant(ParsingContext context)
        {
            var token = context.CurrentToken;

            if (token.Group == TokenGroup.Literal)
            {
                context.Advance();

                var t = (LiteralToken)token;
                return Expressions.Constant(t.Value, t.Value.GetKnownType());
            }

            if (context.MatchAny(KnownTokens.True, KnownTokens.False))
            {
                context.Advance();

                var value = token == KnownTokens.True;
                return Expressions.Constant(value);
            }

            return null;
        }

        #endregion

        #region result

        private class ParsingResult : IParsingResult
        {
            public IExpression Expression { get; init; }
            public IDictionary<int, NameDetails> Names { get; init; }
        }        

        #endregion
    }
}
