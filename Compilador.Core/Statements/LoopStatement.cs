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
        public LoopStatement(Statement statement)
        {
            Statement = statement;
            this.ValidateSemantic();
        }

        public Statement Statement { get; }

        public override string GenerateCode()
        {
            var code = $"while(true){{ {System.Environment.NewLine}";
            code += this.Statement.GenerateCode();
            code += System.Environment.NewLine;
            code += "}";
            return code;
        }

        public override void ValidateSemantic()
        {
            
        }
    }
}
