using System;
using System.Collections.Generic;
using System.Text;
using Compilador.Core;
using Compilador.Core.Interfaces;
using Compilador.Core.Models;

namespace Compilador.Lexer
{
    public class Scanner : IScanner
    {
        private Input input;
        private Input inputAux;
        private readonly ILogger logger;
        private readonly Dictionary<string, TokenType> keywords;

        public Scanner(Input input, ILogger logger)
        {
            this.input = input;
            this.logger = logger;
            this.keywords = new Dictionary<string, TokenType>
            {
                ["if"] = TokenType.IfPalabraReservada,
                ["else"] = TokenType.ElsePalabraReservada,
                ["while"] = TokenType.WhilePalabraReservada,
                ["true"] = TokenType.TruePalabraReservada,
                ["false"] = TokenType.FalsePalabraReservada,
                ["end"] = TokenType.EndPalabraReservada,
                ["for"] = TokenType.ForPalabraReservada,
                ["in"] = TokenType.InPalabraReservada,
                ["do"] = TokenType.DoPalabraReservada,
                ["break"] = TokenType.BreakPalabraReservada,
                ["loop"] = TokenType.LoopPalabraReservada,
                ["each"] = TokenType.EachPalabraReservada,
                ["begin"] = TokenType.BeginPalabraReservada,
                ["def"] = TokenType.DefPalabraReservada,
                ["class"] = TokenType.ClassPalabraReservada,
                ["initialize"] = TokenType.InitializePalabraReservada,
                ["new"] = TokenType.NewPalabraReservada,
                ["puts"] = TokenType.PutsPalabraReservada,
                ["gets"] = TokenType.GetsPalabraReservada,
                ["=begin"] = TokenType.MultiLinCommentPalabraReservada,
                ["=end"] = TokenType.MultiLinEndPalabraReservada,
                ["chomp"] = TokenType.chompPalabraReservada,
                ["to_i"] = TokenType.to_iPalabraReservada,
                ["to_s"] = TokenType.to_SPalabraReservada,
                ["push"] = TokenType.PushPalabraReservada,
                ["delete"] = TokenType.DeletePalabraReservada,
            };
        }

        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentChar = this.GetNextChar();
            while (currentChar != '\0')
            {
                while (char.IsWhiteSpace(currentChar) || currentChar == '\n')
                {
                    currentChar = this.GetNextChar();
                }

                if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = this.PeekNextChar();
                    while (char.IsLetterOrDigit(currentChar) || currentChar=='_')
                    {
                        currentChar = this.GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = this.PeekNextChar();
                    }

                    var tokenLexeme = lexeme.ToString();
                    if (this.keywords.ContainsKey(tokenLexeme))
                    {
                        return BuildToken(tokenLexeme, this.keywords[tokenLexeme]);
                    }

                    return BuildToken(tokenLexeme, TokenType.identificador);
                }
                else if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (currentChar == '.')
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                        while (char.IsDigit(currentChar))
                        {
                            currentChar = GetNextChar();
                            lexeme.Append(currentChar);
                            currentChar = PeekNextChar();
                        }
                    }

                    return BuildToken(lexeme.ToString(), TokenType.NumerosLiteral);
                }

                switch (currentChar)
                {
                    case '\0':
                        return BuildToken("\0", TokenType.FinaldelArchivo);
                    case '.':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Punto);
                    case '+':
                        lexeme.Append(currentChar);
                        var nextChar = this.PeekNextChar();
                        if (nextChar == '+')
                        {
                            currentChar = this.GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.Incremento);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.Mas);
                    case '-':
                        lexeme.Append(currentChar);
                         nextChar = this.PeekNextChar();
                        if (nextChar == '-')
                        {
                            currentChar = this.GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.Decremento);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.Menos);
                    case '*':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Multiplicacion);
                    case '/':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Division);
                    case '<':
                        lexeme.Append(currentChar);
                        nextChar = this.PeekNextChar();
                        if (nextChar == '=')
                        {
                            currentChar = this.GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.MenorIgual);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.Menor);
                    case '>':
                        lexeme.Append(currentChar);
                        nextChar = this.PeekNextChar();
                        if (nextChar == '=')
                        {
                            currentChar = this.GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.MayorIgual);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.Mayor);
                    
                   
                    case '(':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.ParentesisIzq);
                    case ')':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.ParentesisDer);
                    case '[':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.CorcheteIzq);
                    case ']':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.CorcheteDer);
                    case ',':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Comma);
                    case '%':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Porcentaje);
                    case '=':
                        lexeme.Append(currentChar);
                        nextChar = this.PeekNextChar();
                        if (nextChar == '=')
                        {
                            currentChar = this.GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.IgualDoble);
                        }
                        if (PeekIfComment()=="begin")
                        {
                            lexeme.Append(GetIfComment());
                            return BuildToken(lexeme.ToString(), TokenType.MultiLinCommentPalabraReservada);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.IgualAsignacion);
                    case '$':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        while (char.IsLetterOrDigit(currentChar)  )
                        { 
                            lexeme.Append(currentChar);
                            currentChar = GetNextChar();
                        }
                        //lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.DolarVariableGlobal);
                    case '"':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        while (currentChar != '"')
                        {
                            lexeme.Append(currentChar);
                            currentChar = GetNextChar();
                            //if(currentChar == '#')
                            //{

                            //}
                        }
                        if (lexeme.ToString().Contains("#{"))
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.SpecialString);
                        }
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.StringLiteral);
                    case '#':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        //final del archivo no esta considerado
                        while (currentChar.ToString() != "\n" && currentChar.ToString() != "\0")
                        {
                            lexeme.Append(currentChar);
                            currentChar = GetNextChar();
                        }
                        if (currentChar.ToString() != "\0")
                        {
                            lexeme.Append(currentChar);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.HashtagComentario);

                    case '&':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        if (currentChar == '&')
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.And);
                        }
                        lexeme.Clear();
                        logger.Error($"Expected & but {currentChar} was found, line ine: {this.input.Position.Line} and column: {this.input.Position.Column}");
                        continue;
                    case '!':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        if (currentChar == '=')
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.Distinto);
                        }
                        lexeme.Clear();
                        logger.Error($"Expected = but {currentChar} was found, line ine: {this.input.Position.Line} and column: {this.input.Position.Column}");
                        continue;
                    case '|':
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                        if (currentChar == '|')
                        {
                            currentChar = GetNextChar();
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.Or);
                        }
                        return BuildToken(lexeme.ToString(), TokenType.pipe);
                       
                    default:
                        break;
                }

                logger.Error($"Invalid character {currentChar} in line: {this.input.Position.Line} and column: {this.input.Position.Column}");
                currentChar = this.GetNextChar();
            }
            return BuildToken("\0", TokenType.FinaldelArchivo);
        }

        private Token BuildToken(string lexeme, TokenType tokenType)
        {
            return new Token
            {
                Column = this.input.Position.Column,
                Line = this.input.Position.Line,
                Lexeme = lexeme,
                TokenType = tokenType,
            };
        }

        private char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }

        private void CopyInput()
        {
            inputAux = input;
        }

        private char GetNextCharAux()
        {
            var next = inputAux.NextChar();
            inputAux = next.Reminder;
            return next.Value;
        }

        private char PeekNextCharAux()
        {
            var next = inputAux.NextChar();
            return next.Value;
        }

        private string PeekIfComment()
        {
            CopyInput();
            char currentChar;
            var lexeme = new StringBuilder();
            var nextChar = this.PeekNextCharAux();
            if (nextChar == 'b')
            {
                var lexAux = new StringBuilder();
                currentChar = this.GetNextCharAux();
                lexAux.Append(currentChar);
                for (int i = 0; i < 4; i++)
                {
                    currentChar = this.GetNextCharAux();
                    lexAux.Append(currentChar);
                }
                if (lexAux.ToString() == "begin")
                {
                    lexeme.Append(lexAux.ToString());
                }
            }
            return lexeme.ToString();
        }

        private string GetIfComment()
        {
            char currentChar;
            var lexeme = new StringBuilder();
            //lexeme.Append('=');
            var nextChar = this.PeekNextChar();
            string lex = lexeme.ToString();
            while (!lex.Contains("=end"))
            {
                currentChar = this.GetNextChar();
                lexeme.Append(currentChar);
                lex = lexeme.ToString();
            }

            return lexeme.ToString();
        }
    }
}