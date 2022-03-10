using System;
using Compilador.Core;
using Compilador.Core.Expressions;
using Compilador.Core.Interfaces;
using Compilador.Core.Models;
using Compilador.Core.Statements;

namespace Compilador.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner scanner;
        private readonly ILogger logger;
        private Token lookAhead;
        private Environment topEnvironment;

        public Parser(IScanner scanner, ILogger logger)
        {
            this.scanner = scanner;
            this.logger = logger;
            this.Move();
        }

        public Statement Parse()
        {
            var program = Program();
            return program;
        }


        private Statement Program()
        {
            var block = Block();
            return block;
        }

        private Statement Block()
        {
            // Decls();
            /*if (this.lookAhead.TokenType == TokenType.DefPalabraReservada)
            {
                Methods();
            }
            else
            {*/
            var savedEnvironment = topEnvironment;
            topEnvironment = new Environment(topEnvironment);
            var statements = Stmts();

            topEnvironment = savedEnvironment;
            return statements;
            //}
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
             var token = this.lookAhead;
             this.Match(TokenType.identificador);
            //.
            
            if (this.lookAhead.TokenType == TokenType.Punto)
            {
                this.Match(TokenType.Punto);
                this.Match(TokenType.EachPalabraReservada);
                this.Match(TokenType.DoPalabraReservada);
                this.Match(TokenType.identificador);
                Stmt();
            }
            else
            {
                //=
                this.Match(TokenType.IgualAsignacion);

                //valor
                var valor = Valor();
                var id = new IdExpression(valor, token);
                topEnvironment.Put(token.Lexeme, id);
            }
        }
        private Core.Types.Type Valor()
        {
            switch (this.lookAhead.TokenType)
            {

                case TokenType.NumerosLiteral:
                    this.Match(TokenType.NumerosLiteral);
                    if (this.lookAhead.TokenType != TokenType.identificador )
                    {
                         AssignmentExpr();
                    }
                    return Core.Types.Type.Number;
                case TokenType.StringLiteral:
                    this.Match(TokenType.StringLiteral);
                    if (this.lookAhead.TokenType != TokenType.identificador)
                    {
                        AssignmentExpr();
                    }
                    return Core.Types.Type.String;
                case TokenType.GetsPalabraReservada:
                    this.Match(TokenType.GetsPalabraReservada);
                    return null;
                case TokenType.identificador:
                    var token = this.lookAhead;
                    this.Match(TokenType.identificador);
                    if (this.lookAhead.TokenType != TokenType.identificador )
                    {
                        AssignmentExpr();
                    }
                    var simbolo= topEnvironment.Get(token.Lexeme);
                    return simbolo.Id.GetExpressionType();
                case TokenType.CorcheteIzq:
                    this.Match(TokenType.CorcheteIzq);
                    var valor = Valor();
                    if (this.lookAhead.TokenType == TokenType.Comma)
                    {
                        this.Match(TokenType.Comma);
                        Valor();
                    }
                    else
                    {
                        this.Match( TokenType.CorcheteDer);
                    }
                    return new Core.Types.Array("[]", TokenType.ComplexType, valor);
                default:
                    throw new ApplicationException($"Syntax error! Unrecognized type in line: {this.lookAhead.Line} and column: {this.lookAhead.Column}");
            }
        }
        private Statement Stmts()
        {
            if (this.lookAhead.TokenType == TokenType.EndPalabraReservada)
            {
                return null;
            }
            Stmt();
            Stmts();
            return new SequenceStatement(Stmt(), Stmts());
        }

        private Statement Stmt()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.identificador:
                    var symbol = topEnvironment.Get(this.lookAhead.Lexeme);
                    TypedExpression index = null;
                    Decls();
                    return null;
               
                case TokenType.IfPalabraReservada:
                    this.Match(TokenType.IfPalabraReservada);
                    this.Match(TokenType.ParentesisIzq);
                    var TypedExpression = LogicalOrExpr();
                    this.Match(TokenType.ParentesisDer);
                    var trueStatement = Stmt();
                    if (this.lookAhead.TokenType != TokenType.ElsePalabraReservada)
                    {
                       
                        return new IfStatement(TypedExpression, trueStatement, null);
                    }
                    this.Match(TokenType.ElsePalabraReservada);
                    var falseStatement = Stmt();
                    return new IfStatement(TypedExpression,trueStatement,falseStatement);
                case TokenType.WhilePalabraReservada:
                    this.Match(TokenType.WhilePalabraReservada);
                  
                    TypedExpression = LogicalOrExpr();
                    
                    return new WhileStatement(TypedExpression,Stmt());
                case TokenType.PutsPalabraReservada:
                    this.Match(TokenType.PutsPalabraReservada);
                    this.Match(TokenType.StringLiteral);
                    Stmt();
                    //Ocupa Revisión
                    break;
                case TokenType.LoopPalabraReservada:
                    this.Match(TokenType.LoopPalabraReservada);
                    this.Match(TokenType.DoPalabraReservada);
                    Stmt();
                    //this.Match(TokenType.EndPalabraReservada);
                    break;
                case TokenType.ForPalabraReservada:
                    this.Match(TokenType.ForPalabraReservada);
                    var iter = this.lookAhead;
                    var simbolo = topEnvironment.Get(iter.Lexeme);
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.InPalabraReservada);
                    var arreglo = this.lookAhead;
                    var simbolo2 = topEnvironment.Get(arreglo.Lexeme);
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.DoPalabraReservada);
                    this.Match(TokenType.EndPalabraReservada);
                    return new ForStatement(simbolo.Id, simbolo2.Id, Stmt());
                case TokenType.HashtagComentario:
                    this.Match(TokenType.HashtagComentario);
                    Stmt();
                    break;
                case TokenType.BreakPalabraReservada:
                    this.Match(TokenType.BreakPalabraReservada);
                    //Ocupa Revisión
                    return null;
                default:
                    return Block();
            }
        }
        private void Methods()
        {
            if (this.lookAhead.TokenType == TokenType.EndPalabraReservada)
            {
                return;
            }
            this.Match(TokenType.DefPalabraReservada);
            this.Match(TokenType.identificador);
            if(this.lookAhead.TokenType == TokenType.ParentesisIzq)
            {
                this.Match(TokenType.ParentesisIzq);
                parametroDef();
                this.Match(TokenType.ParentesisDer);
            }
            Stmt();
            //this.Match(TokenType.EndPalabraReservada);

        }
        private void parametroDef()
        {
            if(this.lookAhead.TokenType == TokenType.identificador)
            {
                this.Match(TokenType.identificador);
                if(this.lookAhead.TokenType== TokenType.Comma)
                {
                    this.Match(TokenType.Comma);
                    parametroDef();
                }
            }
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

        private TypedExpression LogicalOrExpr()
        {
            var TypedExpression = LogicalAndExpr();
            while (this.lookAhead.TokenType == TokenType.Or)
            {
                var token = this.lookAhead;
                this.Move();
                TypedExpression = new LogicalExpression(token, TypedExpression, LogicalAndExpr());
            }
            return TypedExpression;
        }

        private TypedExpression LogicalAndExpr()
        {
            var expr = Eq();
            while (this.lookAhead.TokenType == TokenType.And)
            {
                var token = this.lookAhead;
                this.Move();
                expr = new LogicalExpression(token, expr, Eq());
            }
            return expr;
        }

        private TypedExpression Eq()
        {
            var expr = Rel();
            while (this.lookAhead.TokenType == TokenType.IgualDoble  || this.lookAhead.TokenType == TokenType.Distinto)
            {
                var token = this.lookAhead;
                this.Move();
                expr = new RelationalExpression(token, expr, Rel());
            }
            return expr;
        }


        private TypedExpression Rel()
        {
            var expr = Expr();
            while (this.lookAhead.TokenType == TokenType.Menor ||
                this.lookAhead.TokenType == TokenType.Mayor ||
                this.lookAhead.TokenType == TokenType.MenorIgual ||
                this.lookAhead.TokenType == TokenType.MayorIgual)
            {
                var token = this.lookAhead;
                this.Move();
                expr = new RelationalExpression(token, expr, Expr());
            }
            return expr;
        }

        private TypedExpression Expr()
        {
            var expr = Term();
            while (this.lookAhead.TokenType == TokenType.Mas || this.lookAhead.TokenType == TokenType.Menos)
            {
                var token = this.lookAhead;
                this.Move();
                expr = new ArithmeticExpression(token, expr,Term());
            }
            return expr;
        }

        private TypedExpression Term()
        {
            var expr = PostFixExpr();
            while (this.lookAhead.TokenType == TokenType.Multiplicacion || this.lookAhead.TokenType == TokenType.Division
                || this.lookAhead.TokenType == TokenType.Porcentaje)
            {
                var token = this.lookAhead;
                this.Move();
                expr = new ArithmeticExpression(token, expr,PostFixExpr());
            }
            return expr;
        }

        private TypedExpression PostFixExpr()
        {
            var expr = Factor();
            if (this.lookAhead.TokenType == TokenType.CorcheteIzq)
            {
                var id = expr as IdExpression;
                this.Match(TokenType.CorcheteIzq);
                var index = LogicalOrExpr();
                this.Match(TokenType.CorcheteDer);

                var type = ((Core.Types.Array)id.GetExpressionType()).Of;
                return new ArrayAccessExpression(type, this.lookAhead, id, index);
            }
            return expr;
        }

        private TypedExpression Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.ParentesisIzq:
                    this.Match(TokenType.ParentesisIzq);
                    var expr = LogicalOrExpr();
                    this.Match(TokenType.ParentesisDer);
                    return expr;
                case TokenType.NumerosLiteral:
                    var token = this.lookAhead;
                    this.Match(TokenType.NumerosLiteral);
                    return new ConstantExpression(Core.Types.Type.Number, token);
                case TokenType.StringLiteral:
                    token = this.lookAhead;
                    this.Match(TokenType.StringLiteral);
                    return new ConstantExpression(Core.Types.Type.String, token);
                case TokenType.TruePalabraReservada:
                    token = this.lookAhead;
                    this.Match(TokenType.TruePalabraReservada);
                    return new ConstantExpression(Core.Types.Type.Bool, token);
                case TokenType.FalsePalabraReservada:
                    token = this.lookAhead;
                    this.Match(TokenType.FalsePalabraReservada);
                    return new ConstantExpression(Core.Types.Type.Bool, token);
                default:
                    token = this.lookAhead;
                    this.Match(TokenType.identificador);
                    return topEnvironment.Get(token.Lexeme).Id;
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