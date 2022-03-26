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
                //number gets
                { (Type.Number, Type.Gets, TokenType.Mas), Type.Number },
                { (Type.Number, Type.Gets, TokenType.Menos), Type.Number },
                { (Type.Number, Type.Gets, TokenType.Multiplicacion), Type.Number },
                { (Type.Number, Type.Gets, TokenType.Division), Type.Number },
                { (Type.Number, Type.Gets, TokenType.Porcentaje), Type.Number },
                //number dynamic
                { (Type.Number, Type.T_Type, TokenType.Mas), Type.Number },
                { (Type.Number, Type.T_Type, TokenType.Menos), Type.Number },
                { (Type.Number, Type.T_Type, TokenType.Multiplicacion), Type.Number },
                { (Type.Number, Type.T_Type, TokenType.Division), Type.Number },
                { (Type.Number, Type.T_Type, TokenType.Porcentaje), Type.Number },
                //gets number
                { (Type.Gets, Type.Number, TokenType.Mas), Type.Number },
                { (Type.Gets, Type.Number, TokenType.Menos), Type.Number },
                { (Type.Gets, Type.Number, TokenType.Multiplicacion), Type.Number },
                { (Type.Gets, Type.Number, TokenType.Division), Type.Number },
                { (Type.Gets, Type.Number, TokenType.Porcentaje), Type.Number },
                //dynamic number
                { (Type.T_Type, Type.Number, TokenType.Mas), Type.Number },
                { (Type.T_Type, Type.Number, TokenType.Menos), Type.Number },
                { (Type.T_Type, Type.Number, TokenType.Multiplicacion), Type.Number },
                { (Type.T_Type, Type.Number, TokenType.Division), Type.Number },
                { (Type.T_Type, Type.Number, TokenType.Porcentaje), Type.Number },
            };
        }

        public override string GenerateCode()
        {
            return $"{this.LeftExpression.GenerateCode()} {this.Token.Lexeme} {this.RightExpression.GenerateCode()}";
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
