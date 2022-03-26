using System;
using Compilador.Core.Expressions;

namespace Compilador.Core.Statements
{
    public class AssignationStatement : Statement
    {
        public AssignationStatement(IdExpression id, TypedExpression expression)
        {
            Id = id;
            Expression = expression;
            this.ValidateSemantic();
        }

        public IdExpression Id { get; }
        public TypedExpression Expression { get; }

        public override string GenerateCode()
        {
            return $"{this.Id.GenerateCode()} = {this.Expression.GenerateCode()};{System.Environment.NewLine}";
        }
        public override void ValidateSemantic()
        {
            if (this.Id.GetExpressionType() != Expression.GetExpressionType() && this.Id.GetExpressionType() != Types.Type.Gets && this.Id.GetExpressionType() != Types.Type.T_Type && Expression.GetExpressionType() != Types.Type.Gets && Expression.GetExpressionType() != Types.Type.T_Type)
            {
                throw new ApplicationException($"Type {Expression.GetExpressionType()} is not assignable to {Id.GetExpressionType()}");
            }
        }
    }
}