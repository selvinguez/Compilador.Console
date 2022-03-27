using Compilador.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class MethodUsingStatement : Statement
    {
        public string Name { get; set; }
        public List<Token> Tokens { get; set; }
        public MethodUsingStatement(string lexeme, List<Token> tokens)
        {
            Name = lexeme;
            Tokens = tokens;
        }
        public override string GenerateCode()
        {
            var code = string.Empty;
            code += $"{Name}(";
            int conteo = 0;
            foreach (var token in Tokens)
            {
                code += token.Lexeme;
                conteo++;
                if (conteo != Tokens.Count)
                {
                    code += ", ";
                }
            }
            code += $");{System.Environment.NewLine}";
            return code;

        }

        public override void ValidateSemantic()
        {
            
        }
    }
}
