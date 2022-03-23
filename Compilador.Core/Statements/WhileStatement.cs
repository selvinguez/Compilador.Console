using System;
using Compilador.Core.Expressions;
using Compilador.Core.Types;

namespace Compilador.Core.Statements
{
    public class WhileStatement : Statement
    {
        private readonly Dictionary<string, string> _typeMapping;

        public Environment env = null;
        public WhileStatement(TypedExpression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
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
        public Statement Statement { get; }

        public override string GenerateCode()
        {

            var code = $"while({this.Expression.GenerateCode()}){{ {System.Environment.NewLine}";

            foreach (var symbol in env.GetSymbolsForCurrentContext())
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
            }
            //EnvironmentManager.PopContext();

            code += this.Statement.GenerateCode();
             code += System.Environment.NewLine;
             code += "}";
            return code;
        }

        public override void ValidateSemantic()
        {
            
            if (this.Expression.GetExpressionType() != Types.Type.Bool)
            {
                throw new ApplicationException($"Expression inside while must be boolean");
            }
            if (this.env == null)
            {
                this.env = EnvironmentManager.PopContext();
            }
            
        }
    }
}