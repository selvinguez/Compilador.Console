using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compilador.Core.Expressions;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class EachStatement : Statement
    {
        public EachStatement(IdExpression arreiglo, Statement statement)
        {
            this.Arreiglo = arreiglo;
            Statement = statement;
            this.ValidateSemantic();
        }

       public IdExpression Arreiglo { get;  }
        public Statement Statement { get; }

        public override void ValidateSemantic()
        {
            if (this.Arreiglo.GetExpressionType() == Types.Type.Number)
            {
                throw new ApplicationException($"Expression inside while must be boolean");
            }
        }
    }
}
