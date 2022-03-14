using System;
using Compilador.Core.Statements;

namespace Compilador.Core.Statements
{
    public abstract class Statement
    {
        public abstract void ValidateSemantic();

        public abstract string GenerateCode();

    }
}
