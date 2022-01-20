// NoRealm licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRealm.ExpressionLite.Token;
using static NoRealm.ExpressionLite.Token.KnownTokens;

namespace NoRealm.ExpressionLite.ExpressionTree
{
    internal static class DebugHelper
    {
        internal static string Render(this IExpression expression) =>
            expression switch
            {
                ArrayExpression ae => ae.RenderArray(),
                BinaryExpression be => be.RenderBinary(),
                ConstantExpression ce => ce.RenderConstant(),
                IdentifierExpression ie => ie.RenderIdentifier(),
                UnaryExpression ue => ue.RenderUnary(),
                IfExpression ife => ife.RenderIf(),
                _ => string.Empty
            };

        internal static string RenderArray(this ArrayExpression expression)
        {
            var builder = new System.Text.StringBuilder(OpenBracket.Lexeme);

            for (var i = 0; i < expression.Operand.Count - 1; i++)
            {
                builder.Append(expression.Operand[i].Render());
                builder.Append($"{Comma.Lexeme} ");
            }

            builder.Append(expression.Operand[^1].Render());
            builder.Append(CloseBracket.Lexeme);
            return builder.ToString();
        }

        internal static string RenderBinary(this BinaryExpression expression)
        {
            var left = expression.Left.Render();
            var right = expression.Right.Render();

            return expression.NodeType switch
            {
                NodeType.LogicAnd => $"{left} {LogicAnd.Lexeme} {right}",
                NodeType.LogicOr => $"{left} {LogicOr.Lexeme} {right}",
                NodeType.Multiply => $"({left} {Multiply.Lexeme} {right})",
                NodeType.Divide => $"({left} {Divide.Lexeme} {right})",
                NodeType.Reminder => $"({left} {Reminder.Lexeme} {right})",
                NodeType.Add => $"({left} {Plus.Lexeme} {right})",
                NodeType.Subtract => $"({left} {Minus.Lexeme} {right})",
                NodeType.In => $"{left} {In.Lexeme} {right}",
                NodeType.NotIn => $"{left} {LogicNot.Lexeme}{In.Lexeme} {right}",
                NodeType.Have => $"{left} {Have.Lexeme} {right}",
                NodeType.NotHave => $"{left} {LogicNot.Lexeme}{Have.Lexeme} {right}",
                NodeType.GreaterThan => $"{left} {GreaterThan.Lexeme} {right}",
                NodeType.GreaterThanOrEquals => $"{left} {GreaterThanEqual.Lexeme} {right}",
                NodeType.LessThan => $"{left} {LessThan.Lexeme} {right}",
                NodeType.LessThanOrEquals => $"{left} {LessThanEqual.Lexeme} {right}",
                NodeType.Equals => $"{left} {Equal.Lexeme} {right}",
                _ => $"{left} {NotEqual.Lexeme} {right}"
            };
        }

        internal static string RenderConstant(this ConstantExpression expression) =>
            expression switch
            {
                _ when expression.StaticType == KnownTypes.String => $"\"{expression.Value}\"",
                _ when expression.StaticType == KnownTypes.Boolean => ((bool) expression.Value).ToString().ToLower(),
                _ => expression.Value.ToString()
            };

        internal static string RenderIdentifier(this IdentifierExpression expression)
            => $"<id:{expression.Id:x}>";

        internal static string RenderUnary(this UnaryExpression expression) =>
            expression.NodeType switch
            {
                NodeType.UnaryMinus => $"{Minus.Lexeme}{expression.Operand.Render()}",
                NodeType.UnaryPlus => $"{Plus.Lexeme}{expression.Operand.Render()}",
                _ => $"{LogicNot.Lexeme}{expression.Operand.Render()}"
            };

        internal static string RenderIf(this IfExpression expression)
            => $"{If.Lexeme}({expression.Condition.Render()}, {expression.TruePart.Render()}, {expression.FalsePart.Render()})";
    }
}
