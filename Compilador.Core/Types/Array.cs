using System;
namespace Compilador.Core.Types
{
    public class Array : Type
    {
        public Type Of { get; }

        public int Size { get; set; }

        public List<int> ListInt { get; set; }

        public List<string> ListString { get; set; }
        public Array(string lexeme, TokenType tokenType, Type of, int size, List<int> listInt, List<string> listString)
            : base(lexeme, tokenType)
        {
            Of = of;
            Size = size;
            ListInt = listInt;
            ListString = listString;
        }
    }
}
