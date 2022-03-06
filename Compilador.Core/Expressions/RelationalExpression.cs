using System.Collections.Generic;
using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class RelationalExpression : BinaryExpression
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;

        public RelationalExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression)
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                { (Type.Number, Type.Number),  Type.Bool},
                { (Type.String, Type.String),  Type.Bool},
            };
        }

        public override Type GetExpressionType()
        {
            var leftType = LeftExpression.GetExpressionType();
            var rightType = RightExpression.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }

            throw new System.ApplicationException($"Cannot perform relational operation on types {leftType} and {rightType}");
        }
    }
}