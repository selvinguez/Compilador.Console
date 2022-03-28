using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Compilador.Core.Expressions;

namespace Compilador.Core.Statements
{
    public class PrintStatement : Statement
    {
        public PrintStatement(IEnumerable<TypedExpression> parameters)
        {
            Parameters = parameters;
            this.ValidateSemantic();
        }

        public IEnumerable<TypedExpression> Parameters { get; }

        public override string GenerateCode()
        {
            var code = "Console.WriteLine(";
            
            foreach (var param in this.Parameters)
            {
                if (param.GenerateCode().Contains('{'))
                {
                    code += "$";
                }
                code += $"{param.GenerateCode()}";
            }

            code += ");";
            return code;
        }

        public override void ValidateSemantic()
        {
            /*if (Parameters.Any(x => x.GetExpressionType() != Types.Type.String))
            {
                throw new ApplicationException("All parameters for print method must be string");
            }*/
        }
    }
}
