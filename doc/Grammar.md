# Expression Grammar

This is the grammar used by internal component `ExpressionParser`, which is derived from `C` language expressions.

```
expression
    or-expression

or-expression
    and-expression
    or-expression '||' and-expression

and-expression
    equality-expression
    and-expression '&&' equality-expression

equality-expression
    relational-expression
    equality-expression '==' relational-expression
    equality-expression '!=' relational-expression

relational-expression
    additive-expression
    relational-expression '<' additive-expression
    relational-expression '>' additive-expression
    relational-expression '<=' additive-expression
    relational-expression '>=' additive-expression
    relational-expression ['!'] 'in' '[' additive-expression {',' additive-expression} ']'
    relational-expression ['!'] 'have' additive-expression

additive-expression
    multiplicative-expression
    additive-expression '+' multiplicative-expression
    additive-expression '-' multiplicative-expression

multiplicative-expression:
    unary-expression
    multiplicative-expression '*' unary-expression
    multiplicative-expression '/' unary-expression
    multiplicative-expression '%' unary-expression

unary-operator
    '+' '-' '!'

unary-expression
    primary-expression
    unary-operator unary-expression

if-expresssion
    'if' '(' or-expression ',' expression ',' expression ')'

primary-expression
    identifier
    constant
    if-expression
    '(' expression ')'

constant
    string
    number
    date-time
    boolean

string
    '"' <any characters except \ and "> '""

number
    <dot-net decimal number format>

date-time
   '#' <dot-net date-time format> '#'

boolean
    'true'
    'false'
```
