using Compilador.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class LoopStatement : Statement
    {
        private readonly Dictionary<string, string> _typeMapping;

        public Environment env = null;
        public LoopStatement(Statement statement)
        {
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

        public Statement Statement { get; }

        public override string GenerateCode()
        {
            var code = $"while(true){{ {System.Environment.NewLine}";
            /*foreach (var symbol in env.GetSymbolsForCurrentContext())
            {
                var symbolType = symbol.Id.GetExpressionType();
                if (symbolType is Types.Array array)
                {
                    symbolType = array.Of;
                    code += $"List<{_typeMapping[symbolType.Lexeme]}> {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
                else
                {
                    code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
            }*/
            //EnvironmentManager.PopContext();
            code += this.Statement.GenerateCode();
            code += System.Environment.NewLine;
            code += "}";
            return code;
        }

        public override void ValidateSemantic()
        {
            if (this.env == null)
            {
                this.env = EnvironmentManager.PopContext();
            }
        }
    }
}
