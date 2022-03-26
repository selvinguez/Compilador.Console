using System;
using Compilador.Core.Expressions;

namespace Compilador.Core.Statements
{
    public class IfStatement : Statement
    {
        private readonly Dictionary<string, string> _typeMapping;
        public Environment env = null;
        public Environment envFalse = null;
        public IfStatement(TypedExpression expression, Statement trueStatement, Statement falseStatement)
        {
            Expression = expression;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
            _typeMapping = new Dictionary<string, string>
            {
                { "number", "int" },
                { "string", "string" },
                { "bool", "bool" },
                { "gets", "var" }
            };
            this.ValidateSemantic();
        }

        public TypedExpression Expression { get; }
        public Statement TrueStatement { get; }
        public Statement FalseStatement { get; }

        public override string GenerateCode()
        {
            var code = $"if({this.Expression.GenerateCode()}){{ {System.Environment.NewLine}";
            /*foreach (var symbol in env.GetSymbolsForCurrentContext())
            {
                var symbolType = symbol.Id.GetExpressionType();
                if (symbolType is Types.Array array)
                {
                    symbolType = array.Of;
                    code += $"List<{_typeMapping[symbolType.Lexeme]}> {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
                else
                {
                    code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                }
            }*/
            //EnvironmentManager.PopContext();
            code += this.TrueStatement.GenerateCode();
            if (this.FalseStatement != null)
            {
                code += $"}}else{{{System.Environment.NewLine}";
                /*foreach (var symbol in envFalse.GetSymbolsForCurrentContext())
                {
                    var symbolType = symbol.Id.GetExpressionType();
                    if (symbolType is Types.Array array)
                    {
                        symbolType = array.Of;
                        code += $"List<{_typeMapping[symbolType.Lexeme]}> {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                    }
                    else
                    {
                        code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                    }
                }*/
                code += $"{this.FalseStatement.GenerateCode()}{System.Environment.NewLine}";
            }
            code += "}";
            return code;
        }

        public override void ValidateSemantic()
        {
            if (this.Expression.GetExpressionType() != Types.Type.Bool)
            {
                throw new ApplicationException($"Expression inside if must be boolean");
            }
            if (this.FalseStatement != null)
            {
                if (this.envFalse == null)
                {
                    this.envFalse = EnvironmentManager.PopContext();
                }
            }
            if (this.env == null)
            {
                this.env = EnvironmentManager.PopContext();
            }
            
        }
    }
}
