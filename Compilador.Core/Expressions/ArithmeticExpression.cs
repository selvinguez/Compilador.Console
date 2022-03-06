using System.Collections.Generic;
using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class ArithmeticExpression : BinaryExpression
    {
        private readonly Dictionary<(Type, Type, TokenType), Type> _typeRules;

        public ArithmeticExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression)
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(Type, Type, TokenType), Type>
            {
                { (Type.Number, Type.Number, TokenType.Mas), Type.Number },
                { (Type.String, Type.String, TokenType.Mas), Type.String },
                { (Type.Number, Type.Number, TokenType.Menos), Type.Number },
                { (Type.Number, Type.Number, TokenType.Multiplicacion), Type.Number },
                { (Type.Number, Type.Number, TokenType.Division), Type.Number },
                { (Type.Number, Type.Number, TokenType.Porcentaje), Type.Number },
            };
        }

        // a + b
        public override Type GetExpressionType()
        {
            var leftType = LeftExpression.GetExpressionType();
            var rightType = RightExpression.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType, Token.TokenType), out var resultType))
            {
                return resultType;
            }

            throw new System.ApplicationException($"Cannot perform {Token.Lexeme} operation on types {leftType} and {rightType}");
        }
    }
}
