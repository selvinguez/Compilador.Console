using Compilador.Core.Models;

namespace Compilador.Core.Interfaces
{
    public interface IScanner
    {
        Token GetNextToken();
    }
}