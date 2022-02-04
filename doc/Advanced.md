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

As the name implies, the `IScannerFactory` represent the factory class which creates the scanner instances, and the created scanner is implementing the `IScanner` interface.

The type `ScannerOptions` have properties to customize the scanner working:

  1. `MaxIdentifierLength`: sets the maximum number of characters allowed for an identifier, default to 255.
  2. `IgnoreWhitespaceToken`: determine whether to ignore the whitespace token, default to false.

The type `ScannerLexemeConverter` inherits from `DefaultLexemeConverter` to allow some customization specific to the scanner, such as adding support to escape sequence to a string type.

The type `ScannerFactory` is the default implementation for `IScannerFactory` for creating scanners. you create instance from it and the call one of the `GetScanner` overlaods to get `IScanner` implementation.

The `IScanner` have a single method `Scan`, which takes a string and return a sequence of tokens which matches the input string.

> The implementation of `IScanner` used by `ScannerFactory` is a pull-based scanner, which means it will not scan the entire source at the call, rather it will return a token by token till we reach the end of string, this way the scanner is more efficient.

> The type `Scanner` is the default implementation which get returned by `ScannerFactory`.

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

// TODO
