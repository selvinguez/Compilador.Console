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
                if (Token.Lexeme.Contains("#"))
                {
                    var getHT = Token.Lexeme.IndexOf('#');
                    var newLex = Token.Lexeme.Remove(getHT, 1);
                    Token.Lexeme = newLex;
                }
                return Token.Lexeme.Replace("'", "\"");
            }
            if (this.type == Type.Gets)
            {
                return "Console.ReadLine()";
            }
            return Token.Lexeme;
        }

        public override Type GetExpressionType()
        {
            return type;
        }
    }
}
