using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class BreakStatement : Statement
    {
        public Statement Statement { get;}
        public BreakStatement()
        {
            //Statement = statement;
        }
        public override string GenerateCode()
        {
            var code = string.Empty;
            code += $"break;{System.Environment.NewLine}";
            //code += this.Statement.GenerateCode();
            code += System.Environment.NewLine;
            return code;
        }

        public override void ValidateSemantic()
        {
            
        }
    }
}
