# Basics

In this section we will learn the basic principles of composing expressions for ExpressionLite


## Data types

ExpressionLite have the following basic types `number`, `boolean`, `string` and `date-time`.

### `number` data type

All numeric values are basic on `System.Decimal` data type, all numeric types get upgraded to `System.Decimal`.

The types `InPtr` and `UIntPtr` are not supported, also they used in C# under the names `nint` and `unint`.

Example on valid `number` data type values

```
3
3.14
-1
3e5
3+e7
```


### `boolean` data type

This type is based on `System.Boolean` and the only values allowed for it are from the keywords `true` and `false` respectively.


### `string` data type

This type is based on `System.String` data type. There are support for escape sequences `\"` and `\\` which expand to `"` and `\` respectively, `\n`  which expand to new line.

For the library to identify string literal it needs to have the symbol `"` to mark the start and the end of the `string` literal.



### `datetime` data type

This type is based on `System.DateTime` data type.

For the library to identify `datetime` literal it needs to have the symbol `#` to mark the start and the end of a `datetime` literal.

The content of the `datetime `literal must be one of the accepted `System.DateTime.Parse` formats.

Example on valid `datetime` data type values

```
#05/29/2015#
#05/29/2015 05:50 AM#
#Friday, 29 May 2015#
#Friday, 29 May 2015 05:50 AM#
```


### `set` data type

The `set` data type is used to represent multiple values of the same basic data type, and an empty set is not allowed.

To define a `set` literal use the symbol `[` to identify its start and the symbol `]` to identify its end.

This is the only data type which you cannot define an identifier with it.

Example on valid `set` data type values

```
[3, 4, 5, -1]
[true, false, false]
["some string", "another\nstring", ""]
[#2015-01-01#, #3:14 AM#]
```

## Identifiers

Identifiers are names which later expanded to some other values.

There are some rules for identifier:

  1. must not exceed 255 letter (can changed from options, see advanced section).
  2. can start with `_` or `@` or `$` or `A-Z` or `a-z`.
  3. the rest characters can be any letter from 2nd rule or digits `0-9`.

> The letters `@` and `$` are reserved for future use, please try not to use them in order to make your expressions compatible with later releases.

example for valid identifiers:

```
name
_key
some_other_name
pi$314
phi@
epsilon@
```

### Identifier Value

An identifier value come from 5 different source, and we use the member functions from `NoRealm.ExpressionLite.Naming.NameInfo` to define an identifier from these sources


#### Plain

A plain value is an explicit value which have a data type from the basic data type.

Use the function `NameInfo.FromValue` to define this type of name.

example usage:

```csharp
var id = NameInfo.FromValue(3);

id = NameInfo.FromValue(3.14);

id = NameInfo.FromValue("test");

id = NameInfo.FromValue(true);
```


#### Expression

An expression value is not an actual value but an embedded expression that an identifier represent.

Use the function `NameInfo.FromStringExpression` to define this type of name.

example usage:

```csharp
var id = NameInfo.FromStringExpression("3 * -4");

id = NameInfo.FromStringExpression("isValid == true");

id = NameInfo.FromStringExpression("salary + bouns");
```

#### Member

When identifier value is a member then the value is copied from a property, field or a constant inside an object or a static class.

Use the function `NameInfo.FromOtherMember` to define this type of name. this function have two overloads:

  1. `FromOtherMember<T>(Expression<Func<T, object>> propertySelector)`.
  2. `FromOtherMember(Expression<Func<object>> propertySelector)`.

The 1st overload takes an instance of type `T` and return the member. The 2nd overload doesn't take any input and it is used with static members.

> for any overload no operation is allowed, Only access the member you want to be the source of the value.

example usage:

```csharp
var id = NameInfo.FromOtherMember<Employee>(e => e.BasicSalary);

id = NameInfo.FromOtherMember(() => Employee.NumberOfVacationDays);

class Employee
{
  public const int NumberOfVacationDays;

  public int BasicSalary;
}
```


#### LINQ Expression

All the above sources return explicit value, What if you want to have an identifier that represent a complex operation. This source is for this exact situation, here you get some input type and you use it to do some complex operation and return a result in a form of one of the basic data types.

Use the function `NameInfo.FromLinqExpression` to define this type of identifier.

example usage:

```csharp
var id = NameInfo.FromLinqExpression<Employee, decimal>(e => e.BasicSalary + (e.BasicSalary * 10) / 100);

class Employee
{
  public int BasicSalary;
}
```


## Operations

Every data type has a predefined set of operations, here we list them.


### `decimal` data type

| Operation             |  Example        | Result  |
|-----------------------|-----------------|---------|
| Addition              | 3 + 4           | 7       |
| Subtraction           | 3 - 4           | -1      |
| Multiplication        | 3 * 4           | 12      |
| Division              | 3 / 4           | 0.75    |
| Modulus               | 3 % 4           | 3       |
| Equality              | 3 == 4          | false   |
| Inequality            | 3 != 4          | true    |
| Greater Than          | 3 > 4           | false   |
| Greater Than Or Equal | 3 >= 4          | false   |
| Less Than             | 3 < 4           | true    |
| Less Than Or Equal    | 3 <= 4          | true    |
| In                    | 3 in [1, 2, 3]  | true    |
| Not In                | 3 !in [1, 2, 3] | false   |

> The `in` operator is not limited to literal values, you can have identifiers on both sides and in the `set`, also the `set` can have identifier or operation or both together or separated, e.g. [3 + a / 4, x] and same goes for other data types.


### `string` data type

| Operation   |  Example                | Result  |
|-------------|-------------------------|---------|
| Append      | "a" + "b"               | "ab"    |
| Equality    | "a" == "b"              | false   |
| Inequality  | "a" != "b"              | true    |
| In          | "a" in ["a", "b", "c"]  | true    |
| Not In      | "a" !in ["a", "b", "c"] | false   |
| Have        | "expression" have "x"   | true    |
| Not Have    | "expression" !have "x"  | false   |

> the `have` operator can have the append operator to combine a string and it can appear on both sides.


### `datetime` data type

| Operation             |  Example                                             | Result  |
|-----------------------|------------------------------------------------------|---------|
| Equality              | #2015-01-01# == #2015-02-03#                         | false   |
| Inequality            | #2015-01-01# == #2015-02-03#                         | true    |
| Greater Than          | #2015-01-01# > #2015-02-03#                          | false   |
| Greater Than Or Equal | #2015-01-01# >= #2015-02-03#                         | false   |
| Less Than             | #2015-01-01# < #2015-02-03#                          | true    |
| Less Than Or Equal    | #2015-01-01# <= #2015-02-03#                         | true    |
| In                    | #2015-01-01# in [#2015-02-03#, #2015-01-01#, date$]  | true    |
| Not In                | #2015-01-01# !in [#2015-02-03#, #2015-01-01#, date$] | false   |


### `boolean` data type

| Operation   |  Example               | Result  |
|-------------|------------------------|---------|
| And         | true && false          | false   |
| Or          | true || false          | true    |
| Not         | !true                  | false   |
| Equality    | true == false          | false   |
| Inequality  | true != false          | true    |
| In          | true in [true, false]  | true    |
| Not In      | true !in [true, false] | false   |


### `if` operator

For all data type there an `if` operator which take the form `if ( condition , true-part , false-part )`

The `condition` is any operation which have a `boolean` result.

And the `true-part` and `false-part` must have the same data type.

## Final note

The operators `in` and `have` are exist in this version because functions are not yet existed, and once it implemented in later releases they will be replaced with functions with same name but with different way of invoking.

