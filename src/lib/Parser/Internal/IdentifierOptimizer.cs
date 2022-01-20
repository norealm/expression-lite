// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.ExpressionTree;
using NoRealm.ExpressionLite.Naming;
using System.Collections.Generic;

namespace NoRealm.ExpressionLite.Parser
{
    internal sealed class IdentifierOptimizer : ExpressionVisitorBase
    {
        private readonly ParserOptions options;
        private readonly IExpression root;
        private readonly IDictionary<int, NameDetails> names;

        public IdentifierOptimizer(ParserOptions options, IExpression root, IDictionary<int, NameDetails> names)
        {
            this.options = options;
            this.root = root;
            this.names = names;
        }

        public IExpression Optimize()
        {
            if (!options.SubstituteIdentifierWithPlainValue)
                return root;

            return Visit(root);
        }

        public override IExpression VisitIdentifier(IdentifierExpression expression)
        {
            var details = names[expression.Id];

            if (details.Name.NameType == NameType.Plain)
            {
                details.ReferenceCount--;
                return Expressions.Constant(details.Name.Value, details.Name.StaticType);
            }

            return base.VisitIdentifier(expression);
        }
    }
}
