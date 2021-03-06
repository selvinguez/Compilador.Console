using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compilador.Core.Statements
{
    public class BlockStatement : Statement
    {
        public Environment env = null;
        private readonly Dictionary<string, string> _typeMapping;
        public BlockStatement(Statement statement)
        {
            Statement = statement;
            _typeMapping = new Dictionary<string, string>
            {
                { "number", "int" },
                { "string", "string" },
                { "bool", "bool" },
                { "gets", "var" },
                { "T", "dynamic" }
            };
            this.ValidateSemantic();
        }
        public Statement Statement { get; }

        public override string GenerateCode()
        {
            var code = string.Empty;
            /*foreach (var symbol in env.GetSymbolsForBlockContext())
            {
                var symbolType = symbol.Id.GetExpressionType();
                if (symbolType is Types.Array array)
                {
                    symbolType = array.Of;
                    code += $"List<{_typeMapping[symbolType.Lexeme]}> {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
                else
                {
                    if (symbolType != Types.Type.T_Type)
                    {
                        code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                    }
                }
            }*/

            code += this.Statement.GenerateCode();
            return code;
        }

        public override void ValidateSemantic()
        {
            this.Statement?.ValidateSemantic();
            if (this.env == null)
            {
                this.env = EnvironmentManager.getContextBlock();
            }
        }
    }
}