[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/NoRealm.ExpressionLite.svg)](https://www.nuget.org/packages/NoRealm.ExpressionLite)
[![release](https://img.shields.io/github/v/tag/norealm/expression-lite?label=release)](//github.com/norealm/expression-lite/releases/latest)

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

1. LINQ Expression Example

```csharp
var exp = "3*4".ToLinqExpression<decimal>();
```

This is the simplest form, where you use an expression which don't depend on any external dependencies - i.e. identifiers.

because the library treat all numbers as `decimal` if you expect from the expression to return a number then you should pass `decimal`, otherwise you should pass `object` and the library will wrap the final result in `object` as following:

```csharp
var exp = "3*4".ToLinqExpression<object>();
```

2. Runtime function Example

```csharp
var func = "3*4".ToRuntimeMethod<decimal>();
var result = func()
```

This example is the same as the 1st except that we create a runtime function instead of a LINQ expression. Then we executed the function delegate to get the result.


3. LINQ Expression with identifiers

```csharp
var exp = "salary + salary * tax".ToLinqExpression<Employee, decimal>(
	name =>
	{
		if (name == "salary")
			return NameInfo.FromOtherMember<Employee>(e => e.Salary);
		else if (name == "tax")
			return NameInfo.FromOtherMember(() => Employee.TaxRate);
		else
			return null;
	});

class Employee
{
	public const double TaxRate = .2;

	public int Salary;
}
```

In this example we have an expression which calculate an employee total salary, the input expression have 2 identifiers `salary` and `tax`.

This time the method `ToLinqExpression` takes two types, 1st type is the input type to get values from, 2nd type is the result type.

In this case, the input type is `Employee`, and the result type is `decimal`, the method `ToLinqExpression` expects an input method which resolve identifiers to some value.

As you see the `salary` maps to an instance member - a property - so we get this member using the generic method `NameInfo.FromOtherMember`, and the `tax` maps to constant, that is why we get it by using the non-generic method `NameInfo.FromOtherMember`.

We can rewrite this example as follow:

```csharp
var exp = "salary + tax".ToLinqExpression<Employee, decimal>(
	name =>
	{
		if (name == "salary")
			return NameInfo.FromOtherMember<Employee>(e => e.Salary);
		else if (name == "tax")
			return NameInfo.FromStringExpression("salary + tax_rate");
		else if (name == "tax_rate")
			return NameInfo.FromOtherMember(() => Employee.TaxRate);
		else
			return null;
	});
```

In fact, we can rewrite it again as follow:
```csharp
var exp = "salary + tax".ToLinqExpression<Employee, decimal>(
	name =>
	{
		if (name == "salary")
			return NameInfo.FromOtherMember<Employee>(e => e.Salary);
		else if (name == "tax")
			return NameInfo.FromLinqExpression<Employee, decimal>(e => e.Salary * (decimal)Employee.TaxRate);
		else
			return null;
	});
```

All these versions return the excat result and depending in your needs.

4. Runtime functions with identifiers

You can execute the 3rd example but instead of using `ToLinqExpression` use `ToRuntimeMethod`.

```csharp
// the func is the result of calling ToRuntimeMethod
var result = func(new Employee{Salary = 100});
```

The class `NameInfo` have static members to get values from different sources, please see the docs for more details.

## License
This library is licensed to under MIT License.

Brought to you by [NoRealm](https://github.com/norealm)
