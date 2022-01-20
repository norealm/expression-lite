// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NoRealm.ExpressionLite.Parser
{
    /// <summary>
    /// The expression parser options
    /// </summary>
    public sealed class ParserOptions
    {
        /// <summary>
        /// Get maximum number of level of optimizations
        /// </summary>
        public const int MaxOptimizationLevels = 10;

        private readonly byte optimizationLevels = 3;

        /// <summary>
        /// Get/set whether an identifier with plain value will be substituted with its value
        /// </summary>
        public bool SubstituteIdentifierWithPlainValue { get; init; } = true;

        /// <summary>
        /// Get whether to optimize constant operations with its value
        /// </summary>
        public bool OptimizeConstantOperations { get; init; } = true;

        /// <summary>
        /// Get level on which the optimizer will run on the code, default is 3
        /// </summary>
        public byte OptimizationLevels
        {
            get => optimizationLevels;
            init
            {
                if (value > MaxOptimizationLevels)
                    throw new ArithmeticException($"OptimizationLevels must be in range 0 to {MaxOptimizationLevels}");

                optimizationLevels = value;
            }
        }
    }
}
