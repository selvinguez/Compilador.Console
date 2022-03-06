using System;
using Compilador.Core.Expressions;

namespace Compilador.Core
{
    public class Symbol
    {
        public Symbol(IdExpression id)
        {
            Id = id;
        }

        public IdExpression Id { get; }
    }
}