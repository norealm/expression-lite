## Expression Lite

Expression Lite is a powerful expression compiler for .NET

it compiles dynamic expressions into some form, the library have two type of predefined forms:

  1. Expression builder: which convert you dynamic expression to `System.Linq.Expression.Expression<T>` and it is usful for passing it `IQueryable` reducers.
  2. Delegate: which can be executed to get results directly.

The library support 4 basic data types:

  1. `number`: which based on .NET `decimal` data type.
  2. `boolean`: either `true` keyword or `false` keyword.
  3. `string`: any string between double quotes, also the following escape sequences are supported (`\\` `\"` `\n`).
  4. `date and time`: any value based on .NET `DateTime` data type surrounded by the `#`.

each data type have a set of allowed operators to act on it, please refer to the docs for more information.

The power of the library comes from interfacing with .NET through identifiers where the following types of identifiers are allowed:

  1. an identifier with plain value from one of the basic data types.
  2. an identifier with a value comes from a property/field in an instance.
  3. an identifier with a value comes from a property/fieled/constant in a static type.
  4. an identifier with a value comes from another string expression.
  5. a lambda expression which get embedded later into the final expression as a part of it.

The compiler will do type checking and validation, and then will optimize the expression before generating the final code.

## Examples

The following are some usage exmaples, for more detailed examples please refer to the docs directory.

TBD ...

## License
This library is licensed to under MIT License.

Brought to you by [NoRealm](https://github.com/norealm)
