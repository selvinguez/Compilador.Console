using System;
using Compilador.Core.Expressions;
using Compilador.Core.Types;


namespace Compilador.Core.Statements
{
    public class ForStatement : Statement
    {
        public ForStatement(IdExpression identificador, IdExpression arreglo, Statement statement)
        {
            Arreglo = arreglo;
            Identificador = identificador;
            Statement = statement;
        }

        public IdExpression Arreglo { get; }

        public IdExpression Identificador { get; }

        public Statement Statement { get; }

        public override string GenerateCode()
        {
            var code = string.Empty;
            code += $"foreach(var {Identificador.GenerateCode()} in {Arreglo.GenerateCode()}){{{System.Environment.NewLine}";
            code += $"{Statement.GenerateCode()}{System.Environment.NewLine}}}";
            return code;
        }

        public override void ValidateSemantic()
        {
            var arr = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.String, 0);
            var arr2 = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.Number , 0);
            if (this.Arreglo.GetExpressionType() != arr || this.Arreglo.GetExpressionType() != arr2)
            {
                throw new ApplicationException($"Expression inside must be Array of type number or string");
            }
        }
    }
}
