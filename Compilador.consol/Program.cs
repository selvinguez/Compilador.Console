using System;
using System.IO;
using Compilador.Infrastructure;
using Compilador.Lexer;
using Compilador.Parser;

namespace Compilador.Console
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var fileContent = File.ReadAllText("test.txt");
            var logger = new Logger();
            var scanner = new Scanner(new Input(fileContent), logger);
            var parser = new Parser.Parser(scanner, logger);
            parser.Parse();
            /*var token = scanner.GetNextToken();
            while (token.TokenType != Core.TokenType.FinaldelArchivo)
            {
                logger.Info(token.ToString());
                token = scanner.GetNextToken();
            }*/
        }
    }
}