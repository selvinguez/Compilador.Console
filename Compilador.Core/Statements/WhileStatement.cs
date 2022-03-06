using System;
using Compilador.Core.Expressions;
using Compilador.Core.Types;

namespace Compilador.Core.Statements
{
    public class WhileStatement : Statement
    {
        public WhileStatement(TypedExpression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
            this.ValidateSemantic();
        }

        public TypedExpression Expression { get; }
        public Statement Statement { get; }

        public override void ValidateSemantic()
        {
            if (this.Expression.GetExpressionType() != Types.Type.Bool)
            {
                throw new ApplicationException($"Expression inside while must be boolean");
            }
        }
    }
}