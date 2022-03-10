using Compilador.Core.Statements;
using System;
namespace Compilador.Core.Interfaces
{
    public interface IParser
    {
        Statement Parse();
    }
}