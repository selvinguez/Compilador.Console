using System;
using Compilador.Core.Expressions;

namespace Compilador.Core.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(TypedExpression expression, Statement trueStatement, Statement falseStatement)
        {
            Expression = expression;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
            this.ValidateSemantic();
        }

        public TypedExpression Expression { get; }
        public Statement TrueStatement { get; }
        public Statement FalseStatement { get; }

        public override void ValidateSemantic()
        {
            if (this.Expression.GetExpressionType() != Types.Type.Bool)
            {
                throw new ApplicationException($"Expression inside if must be boolean");
            }
        }
    }
}
