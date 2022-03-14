using System.Collections.Generic;
using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class LogicalExpression : BinaryExpression
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;

        public LogicalExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression)
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                { (Type.Bool, Type.Bool), Type.Bool},
            };
        }

        public override string GenerateCode()
        {
            var leftCode = this.LeftExpression.GenerateCode();
            var rightCode = this.RightExpression.GenerateCode();
            return $"{leftCode} {this.Token.Lexeme} {rightCode}";
        }

        public override Type GetExpressionType()
        {
            var leftType = LeftExpression.GetExpressionType();
            var rightType = RightExpression.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }

            throw new System.ApplicationException($"Cannot perform logical operation on types {leftType} and {rightType}");
        }
    }
}