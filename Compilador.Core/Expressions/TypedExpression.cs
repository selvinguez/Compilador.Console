using Compilador.Core.Models;
using Compilador.Core.Types;
using Type = Compilador.Core.Types.Type;

namespace Compilador.Core.Expressions
{
    public abstract class TypedExpression : Node
    {
        protected readonly Type type;

        public Token Token { get; }

        public TypedExpression(Type type, Token token)
        {
            Token = token;
            this.type = type;
        }

        public abstract string GenerateCode();
        public abstract Type GetExpressionType();
    }
}
