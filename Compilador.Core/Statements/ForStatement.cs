using System;
using Compilador.Core.Expressions;
using Compilador.Core.Types;


namespace Compilador.Core.Statements
{
    public class ForStatement : Statement
    {
        private readonly Dictionary<string, string> _typeMapping;

        public Environment env = null;
        public ForStatement(IdExpression identificador, IdExpression arreglo, Statement statement)
        {
            Arreglo = arreglo;
            Identificador = identificador;
            Statement = statement;
            _typeMapping = new Dictionary<string, string>
            {
                { "number", "int" },
                { "string", "string" },
                { "bool", "bool" },
                { "gets", "var" }
            };
            this.ValidateSemantic();
        }

        public IdExpression Arreglo { get; }

        public IdExpression Identificador { get; }

        public Statement Statement { get; }

        public override string GenerateCode()
        {
            var code = string.Empty;
            code += $"foreach(var {Identificador.GenerateCode()} in {Arreglo.GenerateCode()}){{{System.Environment.NewLine}";
            foreach (var symbol in env.GetSymbolsForCurrentContext())
            {
                var symbolType = symbol.Id.GetExpressionType();
                if (symbolType is Types.Array array)
                {
                    symbolType = array.Of;
                    code += $"List<{_typeMapping[symbolType.Lexeme]}> {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
                else
                {
                    if (symbol.Id.Token.Lexeme != Identificador.Token.Lexeme)
                    {
                        code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                    } 
                }
            }
            code += $"{Statement.GenerateCode()}{System.Environment.NewLine}}}";
            return code;
        }

        public override void ValidateSemantic()
        {
            var arr = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.String, 0,null,null);
            var arr2 = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.Number , 0, null, null);
            if (this.Arreglo.GetExpressionType() != arr || this.Arreglo.GetExpressionType() != arr2)
            {
                throw new ApplicationException($"Expression inside must be Array of type number or string");
            }
            if (this.env == null)
            {
                this.env = EnvironmentManager.PopContext();
            }
        }
    }
}
