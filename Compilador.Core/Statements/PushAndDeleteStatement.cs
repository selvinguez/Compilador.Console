using Compilador.Core.Expressions;
using Compilador.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class PushAndDeleteStatement : Statement
    {
        public bool Action { get; }

        public Token Token { get; }

        public TypedExpression TypedExpression { get; }
        public PushAndDeleteStatement(Token token, bool action, TypedExpression typedExpression)
        {
            Action = action;
            Token = token;
            TypedExpression = typedExpression;
            this.ValidateSemantic();
        }
        public override string GenerateCode()
        {
            var code = string.Empty;
            if (Action)
            {
                code += $"{Token.Lexeme}.Add({TypedExpression.GenerateCode()});{System.Environment.NewLine}";
            }
            else
            {
                code += $"{Token.Lexeme}.Remove({TypedExpression.GenerateCode()});{System.Environment.NewLine}";
            }
            return code;
        }

        public override void ValidateSemantic()
        {
            var symbol = EnvironmentManager.Get(Token.Lexeme);
            if (symbol.Id.GetExpressionType() is Core.Types.Array)
            {
                var array = ((Core.Types.Array)symbol.Id.GetExpressionType()).Of;
                if (array == TypedExpression.GetExpressionType())
                {

                }
                else
                {
                    throw new ApplicationException($"Type {TypedExpression.GetExpressionType()} is not assignable to {array}");
                }
            }
            else
            {
                throw new ApplicationException($"Expression inside must be Array");
            }
            var arreglo = ((Core.Types.Array)symbol.Id.GetExpressionType());
            if (!Action)
            {
                if (arreglo.ListInt != null)
                {
                    var existe = arreglo.ListInt.Contains(int.Parse(TypedExpression.GenerateCode()));
                    if (!existe)
                        throw new ApplicationException($"The Value {TypedExpression.GenerateCode()} doesn't exist in array");
                }
                else if (arreglo.ListString != null)
                {
                    var existe = arreglo.ListString.Contains(TypedExpression.GenerateCode());
                    if (!existe)
                        throw new ApplicationException($"The Value {TypedExpression.GenerateCode()} doesn't exist in array");
                }
            }
            
            
        }
    }
}
