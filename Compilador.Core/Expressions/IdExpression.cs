using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class IdExpression : TypedExpression
    {
        private readonly Dictionary<string, string> _typeMapping;
        public IdExpression(Type type, Token token)
            : base(type, token)
        {
            declared = false;
            _typeMapping = new Dictionary<string, string>
            {
                { "number", "int" },
                { "string", "string" },
                { "bool", "bool" },
                { "gets", "var" },
                { "T", "dynamic" },
                { "[]", "var" }
            };
        }
        public bool declared { get; set; }

        public override string GenerateCode()
        {
            /*Console.WriteLine("Chekear");
            foreach (var symbol in EnvironmentManager.GetSymbolsForCurrentContext())
            {
                Console.WriteLine("Lexema es: "+symbol.Id.Token.Lexeme);
                if (symbol.Id.Token.Lexeme == Token.Lexeme)
                {
                    Console.WriteLine("Llego");
                    return Token.Lexeme;
                }
            }
            return $"{_typeMapping[type.Lexeme]} {Token.Lexeme}";*/
            if (!declared)
            {
                declared=true;
                return $"{_typeMapping[type.Lexeme]} {Token.Lexeme}";
            }
            return Token.Lexeme;
        }
        public override Type GetExpressionType()
        {
            return type;
        }
    }
}
