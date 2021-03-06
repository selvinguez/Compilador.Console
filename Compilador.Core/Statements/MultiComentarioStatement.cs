using Compilador.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class MultiComentarioStatement : Statement
    {
        public Token Token { get; }

        public Statement Statement { get; }
        public MultiComentarioStatement(Token token, Statement statement)
        {
            Token = token;
            Statement = statement;

        }
        public override string GenerateCode()
        {
            var code = string.Empty;
            var reemplazo = "/*";
            reemplazo += Token.Lexeme.Substring(6,Token.Lexeme.Length-10);
            reemplazo += "*/";
            code += $"{reemplazo}{System.Environment.NewLine}";
            code += this.Statement.GenerateCode();
            code += System.Environment.NewLine;
            return code;
        }

        public override void ValidateSemantic()
        {

        }
    }
}
