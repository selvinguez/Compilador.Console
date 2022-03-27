using System;
using System.Numerics;
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
            EnvironmentManager.PushContext();
            var statements = Stmts();

            var blockStatement = new BlockStatement(statements);
            //EnvironmentManager.PopContext();
            return blockStatement;
            //}
            //Methods();
           //this.Match(TokenType.FinaldelArchivo);
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
                EnvironmentManager.Put(token.Lexeme, id, null);
            }
        }
        private Core.Types.Type ValorArreglo()
        {
            switch (this.lookAhead.TokenType)
            {

                case TokenType.NumerosLiteral:
                    this.Match(TokenType.NumerosLiteral); 
                    /*if (this.lookAhead.TokenType != TokenType.identificador )
                    {
                         AssignmentExpr();
                    }*/
                    return Core.Types.Type.Number;
                case TokenType.StringLiteral:
                    this.Match(TokenType.StringLiteral);
                    /*if (this.lookAhead.TokenType != TokenType.identificador)
                    {
                        AssignmentExpr();
                    }*/
                    return Core.Types.Type.String;
                default:
                    throw new ApplicationException($"Syntax error! Unrecognized type in line: {this.lookAhead.Line} and column: {this.lookAhead.Column}");
            }
        }
        private Core.Types.Type Valor()
        {
            switch (this.lookAhead.TokenType)
            {

                case TokenType.NumerosLiteral:
                    //this.Match(TokenType.NumerosLiteral); 
                    /*if (this.lookAhead.TokenType != TokenType.identificador )
                    {
                         AssignmentExpr();
                    }*/
                    return Core.Types.Type.Number;
                case TokenType.StringLiteral:
                    //this.Match(TokenType.StringLiteral);
                    /*if (this.lookAhead.TokenType != TokenType.identificador)
                    {
                        AssignmentExpr();
                    }*/
                    return Core.Types.Type.String;
                case TokenType.TruePalabraReservada:
                    //this.Match(TokenType.StringLiteral);
                    /*if (this.lookAhead.TokenType != TokenType.identificador)
                    {
                        AssignmentExpr();
                    }*/
                    return Core.Types.Type.Bool;
                case TokenType.FalsePalabraReservada:
                    //this.Match(TokenType.StringLiteral);
                    /*if (this.lookAhead.TokenType != TokenType.identificador)
                    {
                        AssignmentExpr();
                    }*/
                    return Core.Types.Type.Bool;
                case TokenType.GetsPalabraReservada:
                    //this.Match(TokenType.GetsPalabraReservada);
                    return Core.Types.Type.Gets;
                case TokenType.identificador:
                    var token = this.lookAhead;
                    //this.Match(TokenType.identificador);
                    if (this.lookAhead.TokenType != TokenType.identificador )
                    {
                        //AssignmentExpr();
                    }
                    var simbolo= EnvironmentManager.Get(token.Lexeme);
                    return simbolo.Id.GetExpressionType();
                case TokenType.CorcheteIzq:
                    this.Match(TokenType.CorcheteIzq);
                    var valor = ValorArreglo();
                    if (this.lookAhead.TokenType == TokenType.Comma)
                    {
                        this.Match(TokenType.Comma);
                        ValorArreglo();
                    }
                    else
                    {
                        //this.Match( TokenType.CorcheteDer);
                    }
                    return new Core.Types.Array("[]", TokenType.ComplexType, valor, 0, null, null);
                default:
                    throw new ApplicationException($"Syntax error! Unrecognized type in line: {this.lookAhead.Line} and column: {this.lookAhead.Column}");
            }
        }
        private Statement Stmts()
        {
            /*if (this.lookAhead.TokenType == TokenType.EndPalabraReservada || this.lookAhead.TokenType == TokenType.FinaldelArchivo)
            {
                return null;
            }*/
            if (this.lookAhead.TokenType == TokenType.ElsePalabraReservada)
            {
                //this.Match(TokenType.EndPalabraReservada);
                return null;
            }
            if (this.lookAhead.TokenType == TokenType.EndPalabraReservada)
            {
                this.Match(TokenType.EndPalabraReservada);
                return null;
            }
            if (this.lookAhead.TokenType == TokenType.FinaldelArchivo)
            {
                return null;
            }
            return new SequenceStatement(Stmt(), Stmts());
        }
        private Statement Stmt()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.identificador:
                    //var symbol = EnvironmentManager.Get(this.lookAhead.Lexeme);
                    var token = this.lookAhead;
                    
                    this.Match(TokenType.identificador);
                    TypedExpression index = null;
                    if (this.lookAhead.TokenType == TokenType.Punto)
                    {
                       
                        this.Match(TokenType.Punto);
                        if (this.lookAhead.TokenType == TokenType.PushPalabraReservada)
                        {
                            this.Match(TokenType.PushPalabraReservada);
                            this.Match(TokenType.ParentesisIzq);
                            var valorPush = LogicalOrExpr();
                            this.Match(TokenType.ParentesisDer);
                            var arrPush = EnvironmentManager.Get(token.Lexeme);
                            var arrPushType = ((Core.Types.Array)arrPush.Id.GetExpressionType());
                            if (arrPushType.ListInt!=null)
                            {
                                if (int.TryParse(valorPush.GenerateCode(), out int val))
                                {
                                    arrPushType.ListInt.Add(val);
                                }
                                else
                                {
                                    arrPushType.ListInt.Add(0);
                                }
                                
                            }
                            else if(arrPushType.ListString != null)
                            {
                                arrPushType.ListString.Add(valorPush.GenerateCode());
                            }
                            else
                            {
                                arrPushType.ListInt = new List<int>();
                                if (int.TryParse(valorPush.GenerateCode(), out int val))
                                {
                                    arrPushType.ListInt.Add(val);
                                }
                                else
                                {
                                    arrPushType.ListInt.Add(0);
                                }
                                

                            }
                            return new PushAndDeleteStatement(token, true, valorPush);
                        }
                        else if(this.lookAhead.TokenType == TokenType.DeletePalabraReservada)
                        {
                            this.Match(TokenType.DeletePalabraReservada);
                            this.Match(TokenType.ParentesisIzq);
                            var valorDelete = LogicalOrExpr();
                            this.Match(TokenType.ParentesisDer);
                            return new PushAndDeleteStatement(token, false, valorDelete);
                        }
                        EnvironmentManager.PushContext();
                        this.Match(TokenType.EachPalabraReservada);
                        this.Match(TokenType.DoPalabraReservada);
                        this.Match(TokenType.pipe);
                        var eachId = this.lookAhead;
                        this.Match(TokenType.identificador);
                        this.Match(TokenType.pipe);
                        Symbol simboloEach = null;
                        try
                        {
                            simboloEach = EnvironmentManager.Get(eachId.Lexeme);
                        }
                        catch (Exception)
                        {


                        }

                        if (simboloEach != null)
                        {
                            //simbolo = EnvironmentManager.Get(iter.Lexeme);
                            throw new ApplicationException($"Symbol {eachId.Lexeme} was previously defined in this scope");
                        }
                        var arregloEach = EnvironmentManager.Get(token.Lexeme);
                        var tipoEachArreglo = ((Core.Types.Array)arregloEach.Id.GetExpressionType()).Of;
                        var iddEach = new IdExpression(tipoEachArreglo, eachId);
                        EnvironmentManager.Put(eachId.Lexeme, iddEach, null);
                        simboloEach = EnvironmentManager.Get(eachId.Lexeme);
                        return new ForStatement(simboloEach.Id,arregloEach.Id,Stmts());
                    }

                    if (this.lookAhead.TokenType == TokenType.CorcheteIzq)
                    {
                        this.Match(TokenType.CorcheteIzq);
                        index = LogicalOrExpr();
                        this.Match(TokenType.CorcheteDer);
                    }
                    if (this.lookAhead.TokenType == TokenType.ParentesisIzq)
                    {
                        this.Match(TokenType.ParentesisIzq);
                        var defId = EnvironmentManager.Get(token.Lexeme);
                        if (defId.Id.GetExpressionType() == Core.Types.Type.T_Type)
                        {
                            var tokenTipos = parametroDefUsing();
                            this.Match(TokenType.ParentesisDer);
                            if (defId.Id.GetExpressionType().dynamicParameters != tokenTipos.Count)
                            {
                                throw new ApplicationException("There is no implementation of the method with those parameters");
                            }
                            return new MethodUsingStatement(token.Lexeme, (List<Token>)tokenTipos);
                        }
                        else
                        {
                            throw new ApplicationException($"The identifier {token.Lexeme} isn't a method");
                        }
                    }

                    this.Match(TokenType.IgualAsignacion);
                    if (this.lookAhead.TokenType == TokenType.CorcheteIzq)
                    {
                        List<int> list = null;
                        List<string> listS = null;
                        this.Match(TokenType.CorcheteIzq);
                        if (this.lookAhead.TokenType == TokenType.NumerosLiteral)
                        {
                            list = new List<int>();
                            while (true)
                            {
                                var tokenArr = this.lookAhead;
                                this.Match(TokenType.NumerosLiteral);
                                list.Add(int.Parse(tokenArr.Lexeme));
                                if (this.lookAhead.TokenType == TokenType.Comma)
                                {
                                    this.Match(TokenType.Comma);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            this.Match(TokenType.CorcheteDer);
                            var tipo_arr = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.Number, 0,list,listS);
                            var id2 = new IdExpression(tipo_arr, token);
                            EnvironmentManager.Put(token.Lexeme, id2, null);
                            var symbol2 = EnvironmentManager.Get(token.Lexeme);
                            return new ArrayStatement(id2, list, null);
                        }
                        else if (this.lookAhead.TokenType == TokenType.StringLiteral)
                        {
                            listS = new List<string>();
                            while (true)
                            {
                                var tokenArr = this.lookAhead;
                                this.Match(TokenType.StringLiteral);
                                listS.Add(tokenArr.Lexeme);
                                if (this.lookAhead.TokenType == TokenType.Comma)
                                {
                                    this.Match(TokenType.Comma);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            this.Match(TokenType.CorcheteDer);
                            var tipo_arr = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.String, 0, list,listS);
                            var id2 = new IdExpression(tipo_arr, token);
                            EnvironmentManager.Put(token.Lexeme, id2, null);
                            var symbol2 = EnvironmentManager.Get(token.Lexeme);
                            return new ArrayStatement(id2, null, listS);
                        }
                        else if (this.lookAhead.TokenType == TokenType.CorcheteDer)
                        {
                            this.Match(TokenType.CorcheteDer);
                            var tipo_arr = new Core.Types.Array("[]", TokenType.ComplexType, Core.Types.Type.Number, 0, list, listS);
                            var id2 = new IdExpression(tipo_arr, token);
                            EnvironmentManager.Put(token.Lexeme, id2, null);
                            var symbol2 = EnvironmentManager.Get(token.Lexeme);
                            return new ArrayStatement(id2, null, null);
                        }
                        
                    }
                    var valor = Valor();
                    var id = new IdExpression(valor, token);
                    EnvironmentManager.Put(token.Lexeme, id, null);
                    var symbol = EnvironmentManager.Get(token.Lexeme);

                    var stmt = AssignmentStmt(symbol.Id, index);
                    return stmt;

                case TokenType.IfPalabraReservada:
                    EnvironmentManager.PushContext();
                    this.Match(TokenType.IfPalabraReservada);
                    //this.Match(TokenType.ParentesisIzq);
                    var TypedExpression = LogicalOrExpr();
                    //this.Match(TokenType.ParentesisDer);
                    var trueStatement = Stmts();
                    if (this.lookAhead.TokenType != TokenType.ElsePalabraReservada)
                    {
                        
                        return new IfStatement(TypedExpression, trueStatement, null);
                    }
                    EnvironmentManager.PushContext();
                    this.Match(TokenType.ElsePalabraReservada);
                    var falseStatement = Stmts();
                    //EnvironmentManager.PopContext();
                    return new IfStatement(TypedExpression,trueStatement,falseStatement);
                case TokenType.WhilePalabraReservada:
                    EnvironmentManager.PushContext();
                    this.Match(TokenType.WhilePalabraReservada);
                  
                    TypedExpression = LogicalOrExpr();
                    //EnvironmentManager.PopContext();
                    return new WhileStatement(TypedExpression,Stmts());
                case TokenType.PutsPalabraReservada:
                    this.Match(TokenType.PutsPalabraReservada);
                    var @params = Params();

                    //Ocupa Revisión
                    return new PrintStatement(@params);
                case TokenType.LoopPalabraReservada:
                    EnvironmentManager.PushContext();
                    this.Match(TokenType.LoopPalabraReservada);
                    this.Match(TokenType.DoPalabraReservada);
                    //EnvironmentManager.PopContext();
                    //this.Match(TokenType.EndPalabraReservada);
                    return new LoopStatement(Stmts());//arreglar
                case TokenType.ForPalabraReservada:
                    EnvironmentManager.PushContext();
                    this.Match(TokenType.ForPalabraReservada);
                    var iter = this.lookAhead;
                    Symbol simbolo = null;
                    try
                    {
                        simbolo = EnvironmentManager.Get(iter.Lexeme);
                    }
                    catch (Exception)
                    {

                        
                    }
                    
                    if (simbolo!=null)
                    {
                        //simbolo = EnvironmentManager.Get(iter.Lexeme);
                        throw new ApplicationException($"Symbol {iter.Lexeme} was previously defined in this scope");
                    }
                    
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.InPalabraReservada);
                    var arreglo = this.lookAhead;
                    var simbolo2 = EnvironmentManager.Get(arreglo.Lexeme);
                    var tipo = ((Core.Types.Array)simbolo2.Id.GetExpressionType()).Of;
                    var idd = new IdExpression(tipo, iter);
                    EnvironmentManager.Put(iter.Lexeme, idd, null);
                    simbolo = EnvironmentManager.Get(iter.Lexeme);
                    this.Match(TokenType.identificador);
                    this.Match(TokenType.DoPalabraReservada);
                    //this.Match(TokenType.EndPalabraReservada);
                    return new ForStatement(simbolo.Id, simbolo2.Id, Stmts());
                case TokenType.HashtagComentario:
                    var commentToken = this.lookAhead;
                    this.Match(TokenType.HashtagComentario);
                    return new HashtagComentarioStatement(commentToken);//arreglar
                case TokenType.MultiLinCommentPalabraReservada:
                    var commentMultiToken = this.lookAhead;
                    this.Match(TokenType.MultiLinCommentPalabraReservada);
                    return new MultiComentarioStatement(commentMultiToken, Stmts());
                case TokenType.BreakPalabraReservada:
                    this.Match(TokenType.BreakPalabraReservada);
                    //Ocupa Revisión
                    return new BreakStatement();
                case TokenType.DefPalabraReservada:
                    if (EnvironmentManager.contexts.Count() == 1)
                    {
                        this.Match(TokenType.DefPalabraReservada);
                        token = this.lookAhead;
                        var valorMet = Core.Types.Type.T_Type;
                        var idMet = new IdExpression(valorMet, token);
                        EnvironmentManager.Put(token.Lexeme, idMet, null);
                        EnvironmentManager.PushContext();
                        this.Match(TokenType.identificador);
                        var idexpressions = new List<IdExpression>();
                        if (this.lookAhead.TokenType == TokenType.ParentesisIzq)
                        {
                            this.Match(TokenType.ParentesisIzq);
                            idexpressions = (List<IdExpression>)parametroDef();
                            this.Match(TokenType.ParentesisDer);
                        }
                        idMet.GetExpressionType().dynamicParameters = idexpressions.Count;
                        return new MethodStatement(idMet,idexpressions, Stmts());
                    }
                    throw new ApplicationException("Methods can't be declared here.");
                default:
                    return Block();
            }
        }
        private void Methods()
        {
            /*if (this.lookAhead.TokenType == TokenType.EndPalabraReservada)
            {
                return;
            }*/
            EnvironmentManager.PushContext();
            this.Match(TokenType.DefPalabraReservada);
            this.Match(TokenType.identificador);
            if(this.lookAhead.TokenType == TokenType.ParentesisIzq)
            {
                this.Match(TokenType.ParentesisIzq);
                parametroDef();
                this.Match(TokenType.ParentesisDer);
            }
            Stmts();
            //this.Match(TokenType.EndPalabraReservada);

        }
         
        private IList<IdExpression> parametroDef()
        {
            var idexpressions = new List<IdExpression>();
            if(this.lookAhead.TokenType == TokenType.identificador)
            {
                var identificador = this.lookAhead;
                this.Match(TokenType.identificador);
                var valor = Core.Types.Type.T_Type;
                var id = new IdExpression(valor, identificador);
                EnvironmentManager.Put(identificador.Lexeme, id, null);
                idexpressions.Add(id);
                if (this.lookAhead.TokenType == TokenType.Comma)
                {
                    this.Match(TokenType.Comma);
                    idexpressions.AddRange(parametroDef());
                }
            }
            return idexpressions;
        }
        private IList<Token> parametroDefUsing()
        {
            var idexpressions = new List<Token>();
            if (this.lookAhead.TokenType == TokenType.identificador || this.lookAhead.TokenType == TokenType.StringLiteral || this.lookAhead.TokenType == TokenType.NumerosLiteral || this.lookAhead.TokenType == TokenType.TruePalabraReservada || this.lookAhead.TokenType == TokenType.FalsePalabraReservada)
            {
                if (this.lookAhead.TokenType == TokenType.identificador)
                {
                    EnvironmentManager.Get(this.lookAhead.Lexeme);
                }
                idexpressions.Add(this.lookAhead);
                this.Match(this.lookAhead.TokenType);
                if (this.lookAhead.TokenType == TokenType.Comma)
                {
                    this.Match(TokenType.Comma);
                    idexpressions.AddRange(parametroDefUsing());
                }
            }
            else
            {
                throw new ApplicationException("Invalid Parameter entered");
            }
            return idexpressions;
        }
        private IEnumerable<TypedExpression> Params()
        {
            var @params = new List<TypedExpression>();
            @params.Add(LogicalOrExpr());
            @params.AddRange(ParamsPrime());
            return @params;
        }

        private IEnumerable<TypedExpression> ParamsPrime()
        {
            var @params = new List<TypedExpression>();
            if (this.lookAhead.TokenType == TokenType.Comma)
            {
                this.Match(TokenType.Comma);
                @params.Add(LogicalOrExpr());
                @params.AddRange(ParamsPrime());
            }
            return @params;
        }

        private Statement AssignmentStmt(IdExpression id, TypedExpression index)
        {
            var expr = LogicalOrExpr();
            if (index == null)
            {
                return new AssignationStatement(id, expr);
            }

            var type = ((Core.Types.Array)id.GetExpressionType()).Of;
            var access = new ArrayAccessExpression(type, this.lookAhead, id, index);
            return new ArrayAssignationStatement(access, expr);
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
                case TokenType.GetsPalabraReservada:
                    token = this.lookAhead;
                    this.Match(TokenType.GetsPalabraReservada);
                    return new ConstantExpression (Core.Types.Type.Gets, token);
                case TokenType.SpecialString:
                    token = this.lookAhead;
                    this.Match(TokenType.SpecialString);
                    var getHT = token.Lexeme.IndexOf("#") + 2;
                    var getCorDer = token.Lexeme.IndexOf('}');
                    var resultado = token.Lexeme.Substring(getHT, getCorDer - getHT);
                    var getSymbolString = EnvironmentManager.Get(resultado);
                    return new ConstantExpression(Core.Types.Type.String, token);
                case TokenType.CorcheteDer:
                    token = this.lookAhead;
                    this.Match(TokenType.CorcheteDer);
                    //return new ConstantExpression(Core.Types.Array.Number, token);
                    return null;
                default:
                    token = this.lookAhead;
                    this.Match(TokenType.identificador);
                    return EnvironmentManager.Get(token.Lexeme).Id;
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