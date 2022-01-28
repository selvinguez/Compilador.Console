using System;
using System.IO;
using Compilador.Infrastructure;
using Compilador.Lexer;

namespace Compilador.Console
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            var fileContent = File.ReadAllText("test.txt");
            var logger = new Logger();
            var scanner = new Scanner(new Input(fileContent), logger);
            var token = scanner.GetNextToken();
            while (token.TokenType != Core.TokenType.FinaldelArchivo)
            {
                logger.Info(token.ToString());
                token = scanner.GetNextToken();
            }
        }
    }
}