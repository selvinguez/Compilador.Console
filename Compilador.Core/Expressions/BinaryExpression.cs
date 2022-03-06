using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public abstract class BinaryExpression : TypedExpression
    {
        public TypedExpression LeftExpression { get; }

        public TypedExpression RightExpression { get; }

        public BinaryExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression, Type type)
            : base(type, token)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }
    }
}
