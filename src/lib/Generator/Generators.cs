// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Generator;
using NoRealm.ExpressionLite.Naming;
using NoRealm.ExpressionLite.Parser;
using NoRealm.ExpressionLite.Scanner;
using System;
using System.Linq.Expressions;

namespace NoRealm.ExpressionLite
{
    /// <summary>
    /// a generators for common tasks
    /// </summary>
    public static class Generators
    {
        private static readonly IScanner scanner = new ScannerFactory().GetScanner(new ScannerOptions());
        private static readonly LinqExpressionGenerator linqGenerator = new();
        private static readonly RuntimeMethodGenerator methodGenerator = new();

        /// <summary>
        /// Generate a LINQ Expression from non-identifier input expression
        /// </summary>
        /// <typeparam name="TR">the expression return type</typeparam>
        /// <param name="expression">string expression to convert to LINQ</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <returns>the LINQ expression that match input expression</returns>
        public static Expression<Func<TR>> ToLinqExpression<TR>(this string expression, string name = null)
        {
            var parser = new ParserFactory().GetParser(new ParserOptions());

            return linqGenerator.Generate<TR>(parser.Parse(scanner.Scan(expression), name));
        }

        /// <summary>
        /// Generate a LINQ Expression from input expression which may have identifiers
        /// </summary>
        /// <typeparam name="T">the expression input type</typeparam>
        /// <typeparam name="TR">the expression return type</typeparam>
        /// <param name="expression">string expression to convert to LINQ</param>
        /// <param name="provider">a provider to supply the identifier information</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <returns>the LINQ expression that match input expression</returns>
        public static Expression<Func<T, TR>> ToLinqExpression<T, TR>(this string expression, Func<string, INameInfo> provider, string name = null)
            => ToLinqExpression<T, TR>(expression, name, new InternalNameProvider(provider));

        /// <summary>
        /// Generate a LINQ Expression from input expression which may have identifiers
        /// </summary>
        /// <typeparam name="T">the expression input type</typeparam>
        /// <typeparam name="TR">the expression return type</typeparam>
        /// <param name="expression">string expression to convert to LINQ</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <param name="providers">a multiple name providers to scan for identifier information</param>
        /// <returns>the LINQ expression that match input expression</returns>
        public static Expression<Func<T, TR>> ToLinqExpression<T, TR>(this string expression, string name, params INameProvider[] providers)
        {
            if (providers.Length == 0)
                throw new ArgumentException("at least one name provider must be supplied");

            var parser = new ParserFactory().GetParser(new ParserOptions(), scanner, providers);

            return linqGenerator.Generate<T, TR>(parser.Parse(scanner.Scan(expression), name));
        }

        /// <summary>
        /// Generate a Runtime method from non-identifier input expression
        /// </summary>
        /// <typeparam name="TR">the method return type</typeparam>
        /// <param name="expression">string expression to convert to runtime method</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <returns>the runtime method that match input expression</returns>
        public static Func<TR> ToRuntimeMethod<TR>(this string expression, string name = null)
        {
            var parser = new ParserFactory().GetParser(new ParserOptions());

            return methodGenerator.Generate<TR>(parser.Parse(scanner.Scan(expression), name));
        }

        /// <summary>
        /// Generate a Runtime method from non-identifier input expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR">the method return type</typeparam>
        /// <param name="expression">string expression to convert to runtime method</param>
        /// <param name="provider">a provider to supply the identifier information</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <returns>the runtime method that match input expression</returns>
        public static Func<T, TR> ToRuntimeMethod<T, TR>(this string expression, Func<string, INameInfo> provider, string name = null)
            => ToRuntimeMethod<T, TR>(expression, name, new InternalNameProvider(provider));

        /// <summary>
        /// Generate a Runtime method from non-identifier input expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR">the method return type</typeparam>
        /// <param name="expression">string expression to convert to runtime method</param>
        /// <param name="name">a name to identify this expression in self-reference</param>
        /// <param name="providers">a multiple name providers to scan for identifier information</param>
        /// <returns>the runtime method that match input expression</returns>
        public static Func<T, TR> ToRuntimeMethod<T, TR>(this string expression, string name, params INameProvider[] providers)
        {
            if (providers.Length == 0)
                throw new ArgumentException("at least one name provider must be supplied");

            var parser = new ParserFactory().GetParser(new ParserOptions(), scanner, providers);

            return methodGenerator.Generate<T, TR>(parser.Parse(scanner.Scan(expression), name));
        }

        /// <summary>
        /// An internal name provider
        /// </summary>
        private class InternalNameProvider : INameProvider
        {
            private readonly Func<string, INameInfo> provider;

            public InternalNameProvider(Func<string, INameInfo> provider)
                => this.provider = provider ?? throw new ArgumentNullException(nameof(provider));

            public INameInfo GetNameInfo(string name)
                => provider(name);
        }
    }
}
