using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public class ArrayAccessExpression : TypedExpression
    {
        public IdExpression Id { get; }

        public TypedExpression Index { get; }

        public ArrayAccessExpression(Type type, Token token, IdExpression id, TypedExpression index)
            : base(type, token)
        {
            Id = id;
            Index = index;
        }

        public override Type GetExpressionType()
        {
            return type;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}