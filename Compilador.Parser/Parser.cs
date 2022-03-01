using System;
using Compilador.Core;
using Compilador.Core.Interfaces;
using Compilador.Core.Models;

namespace Compilador.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner scanner;
        private readonly ILogger logger;
        private Token lookAhead;

        public Parser(IScanner scanner, ILogger logger)
        {
            this.scanner = scanner;
            this.logger = logger;
            this.Move();
        }

        public void Parse()
        {
            Program();
        }


        private void Program()
        {
            Block();
        }

        private void Block()
        {
          //  Decls();
            Stmts();
            //Methods();
           // this.Match(TokenType.FinaldelArchivo);
        }
        private void Decls()
        {
            if (this.lookAhead.TokenType == TokenType.identificador)
            {
                Decl();
                Decls();
            }
        }
      
        private void Decl()
        {
            //id
            this.Match(TokenType.identificador);
            //=
            this.Match(TokenType.IgualAsignacion);
            
            //valor0
            Valor();

        }
        private void Valor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.NumerosLiteral:
                    this.Match(TokenType.NumerosLiteral);
                    break;
                case TokenType.StringLiteral:
                    this.Match(TokenType.StringLiteral);
                    break;
                case TokenType.GetsPalabraReservada:
                    this.Match(TokenType.GetsPalabraReservada);
                    break;
               
                case TokenType.CorcheteIzq:
                    this.Match(TokenType.CorcheteIzq);
                    Valor();
                    if (this.lookAhead.TokenType == TokenType.Comma)
                    {
                        this.Match(TokenType.Comma);
                        Valor();
                    }
                    else
                    {
                        this.Match( TokenType.CorcheteDer);
                    }
                    break;
                default:
                    this.logger.Error($"Syntax error! Unrecognized type in line: {this.lookAhead.Line} and column: {this.lookAhead.Column}");
                    break;
            }
        }
        private void Stmts()
        {
            if (this.lookAhead.TokenType == TokenType.EndPalabraReservada)
            {
                return;
            }
            Stmt();
            Stmts();
        }

        private void Stmt()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.identificador:
                    // this.Match(TokenType.identificador);
                     /*if (this.lookAhead.TokenType == TokenType.Punto)
                     {
                         this.Match(TokenType.Punto);
                         this.Match(TokenType.EachPalabraReservada);
                         this.Match(TokenType.DoPalabraReservada);
                         this.Match(TokenType.identificador);
                         Stmt();
                         //this.Match(TokenType.EndPalabraReservada);
                     }
                     else
                     {*/
                        Decls();    
                    // }
                        


                    break;
                case TokenType.IfPalabraReservada:
                    this.Match(TokenType.IfPalabraReservada);
                    this.Match(TokenType.ParentesisIzq);
                    LogicalOrExpr();
                    this.Match(TokenType.ParentesisDer);
                    Stmt();
                    if (this.lookAhead.TokenType != TokenType.ElsePalabraReservada)
                    {
                        //this.Match(TokenType.EndPalabraReservada);
                        break;
                    }
                    this.Match(TokenType.ElsePalabraReservada);
                    Stmt();
                    break;
                case TokenType.WhilePalabraReservada:
                    this.Match(TokenType.WhilePalabraReservada);
                   // this.Match(TokenType.ParentesisIzq);
                    LogicalOrExpr();
                    //this.Match(TokenType.ParentesisDer);
                    Stmt();
                    //this.Match(TokenType.EndPalabraReservada);
                    break;
                case TokenType.PutsPalabraReservada:
                    this.Match(TokenType.PutsPalabraReservada);
                    Params();
                    break;
                case TokenType.LoopPalabraReservada:
                    this.Match(TokenType.LoopPalabraReservada);
                    this.Match(TokenType.DoPalabraReservada);
                    Stmt();
                    //this.Match(TokenType.EndPalabraReservada);
                    break;
                case TokenType.ForPalabraReservada:
                    this.Match(TokenType.ForPalabraReservada);
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.InPalabraReservada);
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.DoPalabraReservada);
                    Stmt();
                    //this.Match(TokenType.EndPalabraReservada);
                    break;
                case TokenType.HashtagComentario:
                    this.Match(TokenType.HashtagComentario);
                    Stmt();
                    break;
                default:
                    Block();
                    break;
            }
        }
        private void Methods()
        {
            if (this.lookAhead.TokenType == TokenType.FinaldelArchivo)
            {
                return;
            }
            this.Match(TokenType.DefPalabraReservada);
            this.Match(TokenType.identificador);
            Stmt();
            this.Match(TokenType.EndPalabraReservada);

        }
        private void Params()
        {
            LogicalOrExpr();
            ParamsPrime();
        }

        private void ParamsPrime()
        {
            if (this.lookAhead.TokenType == TokenType.Comma)
            {
                this.Match(TokenType.Comma);
                LogicalOrExpr();
                ParamsPrime();
            }
        }

        private void AssignmentExpr()
        {
            if (this.lookAhead.TokenType == TokenType.CorcheteIzq)
            {
                this.Match(TokenType.CorcheteIzq);
                Params();
                this.Match(TokenType.CorcheteDer);
                return;
            }
            LogicalOrExpr();
        }

        private void LogicalOrExpr()
        {
            LogicalAndExpr();
            while (this.lookAhead.TokenType == TokenType.Or)
            {
                this.Move();
                LogicalAndExpr();
            }
        }

        private void LogicalAndExpr()
        {
            Eq();
            while (this.lookAhead.TokenType == TokenType.And)
            {
                this.Move();
                Eq();
            }
        }

        private void Eq()
        {
            Rel();
            while (this.lookAhead.TokenType == TokenType.IgualDoble)
            {
                this.Move();
                Rel();
            }
        }


        private void Rel()
        {
            Expr();
            while (this.lookAhead.TokenType == TokenType.Menor ||
                this.lookAhead.TokenType == TokenType.Mayor ||
                this.lookAhead.TokenType == TokenType.MenorIgual ||
                this.lookAhead.TokenType == TokenType.MayorIgual)
            {
                this.Move();
                Expr();
            }
        }

        private void Expr()
        {
            Term();
            while (this.lookAhead.TokenType == TokenType.Mas || this.lookAhead.TokenType == TokenType.Menos)
            {
                this.Move();
                Term();
            }
        }

        private void Term()
        {
            PostFixExpr();
            while (this.lookAhead.TokenType == TokenType.Multiplicacion || this.lookAhead.TokenType == TokenType.Division)
            {
                this.Move();
                PostFixExpr();
            }
        }

        private void PostFixExpr()
        {
            Factor();
            if (this.lookAhead.TokenType == TokenType.CorcheteIzq)
            {
                this.Match(TokenType.CorcheteIzq);
                LogicalOrExpr();
                this.Match(TokenType.CorcheteDer);
            }
        }

        private void Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.ParentesisIzq:
                    this.Match(TokenType.ParentesisIzq);
                    LogicalOrExpr();
                    this.Match(TokenType.ParentesisDer);
                    break;
                case TokenType.identificador:
                    this.Match(TokenType.identificador);
                    break;
                case TokenType.NumerosLiteral:
                    this.Match(TokenType.NumerosLiteral);
                    break;
                case TokenType.StringLiteral:
                    this.Match(TokenType.StringLiteral);
                    break;
                default:
                    break;
            }
        }
        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        private void Match(TokenType expectedTokenType)
        {
            if (this.lookAhead.TokenType != expectedTokenType)
            {
                this.logger.Error($"Syntax Error! expected token {expectedTokenType} but found {this.lookAhead.TokenType} on line {this.lookAhead.Line} and column {this.lookAhead.Column}");
                throw new ApplicationException($"Syntax Error! expected token {expectedTokenType} but found {this.lookAhead.TokenType} on line {this.lookAhead.Line} and column {this.lookAhead.Column}");
            }
            this.Move();
            
        }
    }
}