## Advanced

In this section we will dive into the types that help generating the expression result.

### Introduction

ExpressionLite is a small compiler that consist of 3 stages:

  1. Scanning: is the 1st stage which convert a string into a series of tokens.
  2. Parsing: validates the scanner tokens against a set of rules and then creates the expression tree.
  3. Generators: takes the expression tree and the symbol table and generate the expression in some form. e.g. `LINQ Expressions`.

### Error Handling

The global exception class `ExpressionLiteException` is thrown in case of error while working on the expression. you can check the property `ErrorSource` to know which component throw the exception and the property `ErrorCode` to get the error code.

The `ErrorSource` property is of a type with the same name which defines 3 values `Scanner`, `Parser` and `Generator`.

Also we have the class `ExpressionLiteExceptionExtensions` which define the extension methods `Throw` and `ToException` to help working with the exception class.

All this types are defined inside the namespace `NoRealm.ExpressionLite.Error`.

#### Error Internals

Internally we have the type `ErrorDefinition` which represent a message template with error code. Also we have a static partial class `Errors` which we define it empty inside the namespace `NoRealm.ExpressionLite`. then we define overloads for `Throw` and `ToException` in the class `ExpressionLiteExceptionExtensions` for the type `ErrorDefinition` to easily convert it to `ExpressionLiteException`.

Now when we want to define some error we do it like this
```csharp
internal static partial class Errors
{
    internal static readonly ErrorDefinition UnknownTokenAtIndex =
        new(0x1100001, "unknown token found at index {0}");
}
```

And to throw this error simply we do this
```csharp
throw Errors.UnknownTokenAtIndex.ToException(ErrorSource.Scanner, 10);
```

### Stage 1: Scanning

The `Scanner` is the part of the library responsible for converting some string into a series of tokens. it consist of two components:

  1. Token: this component defines token types and a way to convert a lexeme into some native form, exist in the namespace `NoRealm.ExpressionLite.Token`.
  2. Scanner: this is the scanner which uses the tokens and the lexeme converter to create a series of tokens, exist in the namespace `NoRealm.ExpressionLite.Scanner`.

#### Tokens

First, we have the enum `TokenGroup` which define the available token types, then we have the interface `IToken` which represent a generic token, Also we have the interface `IKnownToken` which represent a predefined token such as `if` or `==`.

The class `KnownTokens` have a predefined list of known tokens, and the class `KnownTypes` have all known types we use inside the library.

The interface `ILexemeConverter` allow converting some lexeme to an input type. The implementation `DefaultLexemeConverter` only accept a type that defined inside `KnownTypes` class.

Also we have the types `IdentifierToken`, `LiteralToken` and `WhitespaceToken` which represent the tokens for their groups. and we have the class `Tokens` to define instance from those types.

#### Scanner

The scanner is the component that convert a string into a series of tokens. We have 2 interfaces to define the work of the scanner, `IScannerFactory` and `IScanner`.

As the name implies, the `IScannerFactory` represent the factory class which creates the scanner instances, and the created scanner implements the `IScanner` interface.

The type `ScannerOptions` have properties to customize the scanner working:

  1. `MaxIdentifierLength`: sets the maximum number of characters allowed for an identifier, default to 255.
  2. `IgnoreWhitespaceToken`: determine whether to ignore the whitespace token, default to false.

The type `ScannerLexemeConverter` inherits from `DefaultLexemeConverter` to allow some customization specific to the scanner, such as adding support to escape sequence to a string type.

The type `ScannerFactory` is the default implementation for `IScannerFactory` for creating scanners. you create instance from it and the call one of the `GetScanner` overlaods to get `IScanner` implementation.

The `IScanner` have a single method `Scan`, which takes a string and return a sequence of tokens which matches the input string.

> The implementation of `IScanner` used by `ScannerFactory` is a pull-based scanner, which means it will not scan the entire source at the call, rather it will return a token by token till we reach the end of string, this way the scanner is more efficient.

> The type `ExpressionScanner` is the default implementation which get returned by `ScannerFactory`.

```csharp
var factory = new ScannerFactory();
var options = new ScannerOptions{IgnoreWhitespaceToken = true, MaxIdentifierLength = 20};
var scanner = factory.GetScanner(options);
var tokens = scanner.Scan("3 * 4 + salary");

foreach (var token in tokens)
{
    Console.WriteLine($"{token.Group}: {token.Lexeme}");
}

// the result should be
// Literal: 3
// Symbol: *
// Literal: 4
// Symbol: +
// Identifier: salary
```

### Stage 2: Parsing

In this stage we take the token series and target to create expression tree which matches the input token series.

This stage have 3 components:

  1. User-Defined Names: which requires the user to provide the value source of some identifier.
  2. Expression tree: is the abstract syntax tree of that represent the expression.
  3. Parser: is the part which responsible if converting the tokens to expression tree and validate the types and optimize the final expression.

#### User-Defined Names

The identifiers used inside the expression are known to the user but not for the library, and the user must provide a source to extract the value from, and this is the responsibility for the types in the namespace `NoRealm.ExpressionLite.Naming`.

The entry point is the type `NameInfo` which have the helper methods that creates a name, The name itself is of type `INameInfo` which wraps the information about that name, one important property of it is `NameType` which have a type with same name with the values determine the type of source.

> Creating a name using `NameInfo` will not set the property `Name` in the returned object, this is done by the parser because it is the one which requesting it.

One or more objects where their type implements `INameProvider` is required by the parser in order for it to call `GetNameInfo` to get the information needed about an identifier.

> Internally all names created by `NameInfo` are of type `InternalNameInfo` which is a simple POCO class

#### Expression tree

This is the abstract syntax tree which model the parsed expression, the it is returned by the parser after finishing analyzing the tokens.

The expression tree have two main types `IExpression` and `IExpressionVisitor`.

The `IExpression` represent expression abstraction which contains the properties:

  1. `NodeType`: which have type with same name and the value is expression node content type.
  2. `StaticType`: represent the static type of this expression node.
  3. `Accept`: is a method which takes instance of `IExpressionVisitor` to transform this node and return the result.

The `IExpressionVisitor` have a base implementation `ExpressionVisitorBase` with basic visiting for all nodes in an expression.

The `IExpression` have a predefined implementation in the following types:

  * `ArrayExpression`: represent an expression with a sequence of expressions.
  * `BinaryExpression`: represent an expression with two operands.
  * `ConstantExpression`: represent an expression with a constant value of one of the basic types.
  * `IdentifierExpression`: represent an expression that is an identifier.
  * `IfExpression`: represent an `if` expression.
  * `UnaryExpression`: represent an expression with one operand.

Finally, the class `Expressions` contains a set of functions to create instances of all supported expressions.

> Note that all functions inside `Expressions` validate input types

```csharp
// 3 * 4 + 5

Expressions.Add(
    Expressions.Multiply(
        Expressions.Constant(3),
        Expressions.Constant(4)
        ),
    Expressions.Constant(5)
);
```

#### Parser

The parser is responsible of taking the tokens and create the expression tree, also it validate the inputs and finally optimize the tree.

The parser contains three main types `IParser`, `IParserFactory` and `IParsingResult`.

The type `IParsingResult` represent the final result of parsing and it contains the properties:

   1. `Expression`: is the root expression tree of parsed expression.
   2. `Names`: a table of names that is referenced inside the `Expression`, the table key is a hash value of referenced identifier.

The `IParserFactory` creates instance of `IParser` using input `ParserOptions`. another overload takes the `ParserOptions` as well as an instance of `IScanner` and a sequence of `INameProvider`.

The difference between the 1st and 2nd overloads that the 1st overload don't allow identifiers in the expression.

The `ParserOptions` have the following properties:

  * `SubstituteIdentifierWithPlainValue`: when set to true, any identifier with a value of a primitive type get substituted the the value directly.
  * `OptimizeConstantOperations`: when set to true, allow expression optimizer to evaluate all nodes with primitive types.
  * `OptimizationLevels`: is the number of iteration to loop over the node to optimize them. default to `3`.
  * `MaxOptimizationLevels`: a constant define the maximum number of optimization iterations.

The `ParserFactory` is the default implementation for `IParserFactory`.

> The type `ExpressionParser` is the default parser implementation which get returned by `ParserFactory`.

The parser responsible for the following:

  * check the expression against expression grammar.
  * validate the expression content data types.
  * validate circular reference for identifiers
  * construct the expression tree.
  * optimize the expression tree.

```csharp
var scannerOptions = new ScannerOptions();
var scannerFactory = new ScannerFactory();
var scanner = scannerFactory.GetScanner(scannerOptions);

var parserOptions = new ParserOptions();
var parserFactory = new ParserFactory();
var parser = parserFactory.GetParser(parserOptions, scanner, Enumerable.Empty<INameProvider>());

var tokens = scanner.Scan("3 * 4 + 5");
var result = parser.Parse(tokens);
```

### Stage 3: Generation

In this stage we take the parser result in `IParserResult` and generate the final result. ExpressionLite have two code generators bundled with it.

   1. Runtime: this generator convert the parser result to a runtime function.
   2. LINQ: this generator convert the parser result to a `LINQ Expression` tree.

The generic interface `IGenerator<T>` represent the generator interface where `T` is the type of the generated result, e.g. the runtime generator have `T = System.Delegate` and the LINQ expression generator have `T = System.Linq.Expressions.LambdaExpression`.

The `IGenerator<T>` have the following methods:

  1. `Generate(IParsingResult)`: takes object of type `IParsingResult` and return object of type `T`.
  2. `Generate(IParsingResult, Type)`: takes object of type `IParsingResult` and `Type` to bind the result to then return object of type `T`.

The type `RuntimeMethodGenerator` implements the logic for runtime method generation, and `LinqExpressionGenerator` implements the logic for LINQ Expression generation.

```csharp
var scannerOptions = new ScannerOptions();
var scannerFactory = new ScannerFactory();
var scanner = scannerFactory.GetScanner(scannerOptions);

var parserOptions = new ParserOptions();
var parserFactory = new ParserFactory();
var parser = parserFactory.GetParser(parserOptions, scanner, Enumerable.Empty<INameProvider>());

var tokens = scanner.Scan("3 * 4 + 5");
var result = parser.Parse(tokens);

var methodGenerator = new RuntimeMethodGenerator();
var method = (Func<decimal>)methodGenerator.Generate(result);
// method() ==> 17

var linqGenerator = new LinqExpressionGenerator();
var linq = linqGenerator.Generate(result);
```

These two types also define the generic `Generate` which take the expected input type and expected return type:

  * `Generate<T>(IParsingResult)`: produce a result with `T` as return type and no inputs.
  * `Generate<T, TR>(IParsingResult)`: produce a result with return type `TR` and input type `T`.

To make your life easier the `Generator` static class have the extension methods `ToLinqExpression` and `ToRuntimeMethod` which do exactly as the above code snippet.
