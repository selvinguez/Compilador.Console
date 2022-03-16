using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class ConstantExpression : TypedExpression
    {
        public ConstantExpression(Type type, Token token)
            : base(type, token)
        {
        }

        public override string GenerateCode()
        {
            if (this.type == Type.String)
            {
                return Token.Lexeme.Replace("'", "\"");
            }
            if (this.type == Type.Gets)
            {
                return "Console.Readline()";
            }
            return Token.Lexeme;
        }

        public override Type GetExpressionType()
        {
            return type;
        }
    }
}
