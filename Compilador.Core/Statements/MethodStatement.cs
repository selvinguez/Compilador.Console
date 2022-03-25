using Compilador.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class MethodStatement : Statement
    {
        private readonly Dictionary<string, string> _typeMapping;

        public Environment env = null;
        public MethodStatement(IdExpression method_Id, List<IdExpression> expressions, Statement statement)
        {
            MethodId = method_Id;

            IdExpressions = expressions;

            Statement = statement;

            _typeMapping = new Dictionary<string, string>
            {
                { "number", "int" },
                { "string", "string" },
                { "bool", "bool" },
                { "gets", "var" },
                { "T", "dynamic" },
            };
            this.ValidateSemantic();
        }
        public List<IdExpression> IdExpressions = null;
        public Statement Statement { get; }

        public IdExpression MethodId { get; }
        public override string GenerateCode()
        {
            var methodType = MethodId.GetExpressionType();
            var code = string.Empty;
            code += $"public {_typeMapping[methodType.Lexeme]} {MethodId.GenerateCode()}(";
            int conteo = 0;
            if (IdExpressions!=null)
            {
                foreach (var item in IdExpressions)
                {
                    var itemType = item.GetExpressionType();
                    code += $"{_typeMapping[itemType.Lexeme]} {item.GenerateCode()}";
                    conteo++;
                    if (conteo != IdExpressions.Count())
                    {
                        code += ", ";
                    }
                }
            }
            
            code += $"){System.Environment.NewLine}{{{System.Environment.NewLine}";
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
                    if (!IdExpressions.Contains(symbol.Id) && !(symbol.Id.Token.Lexeme == MethodId.Token.Lexeme))
                    {
                        code += $"{_typeMapping[symbolType.Lexeme]} {symbol.Id.Token.Lexeme};{System.Environment.NewLine}";
                    }
                }
            }
            code += this.Statement.GenerateCode();
            code += System.Environment.NewLine;
            code += $"return null{System.Environment.NewLine}";
            code += $"}}{System.Environment.NewLine}";
            return code;
        }

        public override void ValidateSemantic()
        {
            if (this.env == null)
            {
                this.env = EnvironmentManager.PopContext();
            }
            var blockCTX = EnvironmentManager.GetSymbolsForBlockContext();
            foreach (var item in IdExpressions)
            {
                var symbol = env.Get(item.Token.Lexeme);
                if (blockCTX.Contains(symbol))
                {
                    throw new ApplicationException($"Symbol { item.Token.Lexeme } was previously defined");
                }
            }
            //var symbolMet = EnvironmentManager.Get(MethodId.Token.Lexeme);
            /*if (blockCTX.Contains(symbolMet))
            {
                throw new ApplicationException($"Symbol { MethodId.Token.Lexeme } was previously defined");
            }*/
            
            
        }
    }
}
