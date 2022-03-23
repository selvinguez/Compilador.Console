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
        public EachStatement(IdExpression arreglo, Statement statement)
        {
            this.Arreglo = arreglo;
            Statement = statement;
            this.ValidateSemantic();
        }

       public IdExpression Arreglo { get;  }
        public Statement Statement { get; }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            if (this.Arreglo.GetExpressionType() == Types.Type.Number)
            {
                throw new ApplicationException($"Expression inside while must be Array");
            }
        }
    }
}
