// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRealm.ExpressionLite.ExpressionTree
{
    /// <summary>
    /// Type of nodes
    /// </summary>
    public enum NodeType
    {
        /// <summary>unary minus operator</summary>
        UnaryMinus,

        /// <summary>unary plus operator</summary>
        UnaryPlus,
        
        /// <summary>unary logical not operator</summary>
        LogicNot,
        
        /// <summary>binary logical and operator</summary>
        LogicAnd,
        
        /// <summary>binary logical or operator</summary>
        LogicOr,
        
        /// <summary>binary multiplication operator</summary>
        Multiply,
        
        /// <summary>binary division operator</summary>
        Divide,
        
        /// <summary>binary reminder operator</summary>
        Reminder,
        
        /// <summary>binary addition operator</summary>
        Add,
        
        /// <summary>binary append operator</summary>
        Append,

        /// <summary>binary subtract operator</summary>
        Subtract,
        
        /// <summary>binary greater than operator</summary>
        GreaterThan,
        
        /// <summary>binary greater than or equals operator</summary>
        GreaterThanOrEquals,
        
        /// <summary>binary less than operator</summary>
        LessThan,
        
        /// <summary>binary less than or equals operator</summary>
        LessThanOrEquals,

        /// <summary>binary equals operator</summary>
        Equals,

        /// <summary>binary not equals operator</summary>
        NotEquals,

        /// <summary>binary in operator</summary>
        In,

        /// <summary>binary not in operator</summary>
        NotIn,

        /// <summary>binary have operator</summary>
        Have,

        /// <summary>binary not have operator</summary>
        NotHave,

        /// <summary>if operator</summary>
        If,

        /// <summary>identifier node</summary>
        Identifier,

        /// <summary>constant node</summary>
        Constant,

        /// <summary>array node</summary>
        Array
    }
}
