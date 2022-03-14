using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class IdExpression : TypedExpression
    {
        public IdExpression(Type type, Token token)
            : base(type, token)
        {
        }

        public override string GenerateCode()
        {
            return Token.Lexeme;
        }
        public override Type GetExpressionType()
        {
            return type;
        }
    }
}
