# Expression Generation

In this section we will learn how to generate LINQ Expressions and Runtime functions using the extension methods `ToLinqExpression` and `ToRuntimeMethod`.

## A short story

Both `ToLinqExpression` and `ToRuntimeMethod` have 3 overloads and we will give each one of them a name to reference them later.

* **The Simple**: `ToLinqExpression<TR>(this string expression, string name = null)`
* **The Simple**: `ToRuntimeMethod<TR>(this string expression, string name = null)`
* **The Common**: `ToLinqExpression<T, TR>(this string expression, Func<string, INameInfo> provider, string name = null)`
* **The Common**: `ToRuntimeMethod<T, TR>(this string expression, Func<string, INameInfo> provider, string name = null)`
* **The Tough**: `ToLinqExpression<T, TR>(this string expression, string name, params INameProvider[] providers)`
* **The Tough**: `ToRuntimeMethod<T, TR>(this string expression, string name, params INameProvider[] providers)`

The goal from these methods is to make your life easier during your common tasks where you will most likely want to focus on implementing the business for your application rather than taking the long road of converting the expression to one of these two forms.

If this methods are not enough for you then you will need to go to the advanced chapter after you finish this one.

For now please ignore the parameter `name` and pass `null` to it, it will get explained later in the end of this chapter.


### The Simple

Well, this one called the simple for a reason it is a [pure function](https://en.wikipedia.org/wiki/Pure_function) without input

Let's break it:

 1. No inputs are allowed.
 2. All operations must be on constant elements i.e. literals (decimal, boolean, string, date-time).

Valid expression

```
3 * 4 / 5
if(true, "one", "two")
#1990-6-16# == #1991-6-16#
"some string"
false
5 in [3 + 2, 1, 0]
```

Invalid expression
```
m * 4
value
if(a == b, 3.14, x)
```

### The Common

Let me tell you a short story, While developing a payroll application the client asked for equation builder to help define the entries to calculate the employee salary.

and there was some points we need to get it covered:

  1. we need to define a set of names which will be mapped to some properties in our `Employee` class.
  2. user can define a custom name which can reference the predefined `Employee` properties or reference custom name.
  3. we need to be able to detect circular reference, i.e. a name can not reference itself directly or indirectly.
  4. we need to have a set of predefined names for each module, i.e. a set of names for payroll, others for accounting ...etc
  5. we need to provide the list of names that available in the current scope of the expression, meaning accounting module must not see the names defined for payroll or its custom names.

To solve these problems we introduce this method - and the next - to help us achieve that.

This method takes as its input the type which later will take the value from its instance. and the method result will be one of the predefined types or `object` to wrap the result.

so let's define our example type
```csharp
public class Employee
{
    public int Id { get; set; }
    public int BasicSalary { get; set; }
    public int NumOfAbsenceDays { get; set; }
}
```

now let's define an expression
```csharp
var netSalaryFn = "basic_salary - salary_per_day * absence"
    .ToRuntimeMethod<Employee, decimal>(
        name =>
        {
            if (name == "basic_salary") // a predefined name
                return NameInfo.FromOtherMember<Employee>(e => e.BasicSalary);
            else if (name == "absence") // a predefined name
                return NameInfo.FromOtherMember<Employee>(e => e.NumOfAbsenceDays);
            else if (name == "salary_per_day") // user-defined custom name (we can get it from database)
                return NameInfo.FromStringExpression("basic_salary / 30");
            return null; // input name is not defined here
        });

var netSalary = netSalaryFn(new Employee {BasicSalary = 1000, NumOfAbsenceDays = 2});
```

This example covered the points 1, 2 and 5. Well  that is sound great for now.

### The Tough

As you can see from previous example, we provided the name provider as a function, however for larger application this is not suitable and will be considered a  bad design, so we need to define the interface `INameProvider`.

This is what is looks like:
```csharp
public interface INameProvider
{
    INameInfo GetNameInfo(string name);
}
```

All you need is to implement your own `INameProvider` and pass the instance to the method, At least you need to pass only one.

if you return `null` from `GetNameInfo` it will pass the name to next provider till we exhaust them all, then an exception will be thrown.

the above example can be rewritten as following:
```csharp

var netSalaryFn = "basic_salary - salary_per_day * absence"
    .ToRuntimeMethod<Employee, decimal>(null, new PayrollPredefinedNames(), new PayrollCustomNames());

var netSalary = netSalaryFn(new Employee {BasicSalary = 1000, NumOfAbsenceDays = 3});


public class PayrollPredefinedNames : INameProvider
{
    public INameInfo GetNameInfo(string name)
    {
        if (name == "basic_salary")
            return NameInfo.FromOtherMember<Employee>(e => e.BasicSalary);
        else if (name == "absence")
            return NameInfo.FromOtherMember<Employee>(e => e.NumOfAbsenceDays);
        return null;
    }
}

public class PayrollCustomNames : INameProvider
{
    private IDictionary<string, string> names;

    public PayrollCustomNames()
    {
        // simulate getting names from database
        names = new Dictionary<string, string>
        {
            ["salary_per_day"] = "basic_salary / 30"
        };
    }

    public INameInfo GetNameInfo(string name)
    {
        if (names.TryGetValue(name, out var value))
            return NameInfo.FromStringExpression(value);

        return null;
    }
}
```

In fact, The method you pass to **The Common** method is passed to an internal type which implements `INameProvider`.

And now we covered the 4th point, and what remain is the 3rd point.

## The `name` parameter

The `name` parameter is used to detect self-referencing directly or indirectly by giving the input expression a name, which if it detected the the parser that this name is used inside the expression or sub-expression it will flag an error.

Try the following examples
```csharp

// direct circular reference
var exp = "3 * net_salary".ToLinqExpression<Employee, decimal>(_ => null, "net_salary");
// will throw
// NoRealm.ExpressionLite.Error.ExpressionLiteException : the identifier 'net_salary' has a reference to itself directly or indirectly.

// indirect circular reference
var exp = "3 * net_salary".ToLinqExpression<Employee, decimal>(_ => NameInfo.FromStringExpression("salary"), "salary");
// will throw
// NoRealm.ExpressionLite.Error.ExpressionLiteException : the identifier 'net_salary' has a reference to itself directly or indirectly.
```

## Is that it

Well, yes and no.

Yes we have covered pretty much everything we need to cover at this point, And No we didn't cover everything yet, and up to this point you can do many things, for example see the next example, Imagine that `employees` is adatabase table and we passed a dynamic expression to `IQueryable` to filter the table:

```csharp
var employees = new Employee[]
{
    new(){Id = 1, NumOfAbsenceDays = 3, BasicSalary = 2100},
    new(){Id = 2, NumOfAbsenceDays = 0, BasicSalary = 4200},
    new(){Id = 3, NumOfAbsenceDays = 5, BasicSalary = 1650},
    new(){Id = 4, NumOfAbsenceDays = 7, BasicSalary = 3400},
    new(){Id = 5, NumOfAbsenceDays = 6, BasicSalary = 3720},
    new(){Id = 6, NumOfAbsenceDays = 1, BasicSalary = 1990},
    new(){Id = 7, NumOfAbsenceDays = 1, BasicSalary = 4320},
    new(){Id = 8, NumOfAbsenceDays = 2, BasicSalary = 1120},
    new(){Id = 9, NumOfAbsenceDays = 7, BasicSalary = 3540}
};

var filter_1 = "net_salary >= 2000 && net_salary <= 3000"
    .ToLinqExpression<Employee, bool>(name =>
    {
        if (name == "net_salary")
            return NameInfo.FromLinqExpression<Employee, decimal>
            (e => e.BasicSalary - ((e.BasicSalary / 30m) * e.NumOfAbsenceDays));

        return null;
    });

var emp_list_1 = employees.AsQueryable().Where(filter_1).ToList();

var filter_2 = "absence == 0"
    .ToLinqExpression<Employee, bool>(name =>
    {
        if (name == "absence")
            return NameInfo.FromOtherMember<Employee>(e => e.NumOfAbsenceDays);

        return null;
    });

var emp_list_2 = employees.AsQueryable().Where(filter_2).ToList();

public class Employee
{
    public int Id { get; set; }
    public int BasicSalary { get; set; }
    public int NumOfAbsenceDays { get; set; }
}
```
