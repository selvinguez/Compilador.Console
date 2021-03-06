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
                { (Type.Number, Type.T_Type),  Type.Bool},
                { (Type.String, Type.T_Type),  Type.Bool},
                { (Type.Gets, Type.T_Type),  Type.Bool},
                { (Type.T_Type, Type.T_Type),  Type.Bool},
                { (Type.T_Type, Type.Number),  Type.Bool},
                { (Type.T_Type, Type.String),  Type.Bool},
                { (Type.T_Type, Type.Gets),  Type.Bool},
                { (Type.Gets, Type.Gets),  Type.Bool},
                { (Type.Gets, Type.Number),  Type.Bool},
                { (Type.Gets, Type.String),  Type.Bool},
                { (Type.Number, Type.Gets),  Type.Bool},
                { (Type.String, Type.Gets),  Type.Bool},
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

            throw new System.ApplicationException($"Cannot perform relational operation on types {leftType} and {rightType}");
        }
    }
}