using System;
using Compilador.Core.Expressions;
using Compilador.Core.Types;


namespace Compilador.Core.Statements
{
    public class ForStatement : Statement
    {
        public ForStatement(IdExpression arreglo, IdExpression numero, Statement statement)
        {
            Arreglo = arreglo;
            Indice = numero;
            Statement = statement;
        }

        public IdExpression Arreglo { get; }

        public IdExpression Indice { get; }

        public Statement Statement { get; }
        public override void ValidateSemantic()
        {
            if (this.Arreglo.GetExpressionType() != Types.Type.Number)
            {
                throw new ApplicationException($"Expression inside while must be Array");
            }
        }
    }
}
