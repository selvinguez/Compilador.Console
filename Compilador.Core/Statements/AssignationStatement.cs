﻿using System;
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

        public override void ValidateSemantic()
        {
            if (this.Id.GetExpressionType() != Expression.GetExpressionType())
            {
                throw new ApplicationException($"Type {Expression.GetExpressionType()} is not assignable to {Id.GetExpressionType()}");
            }
        }
    }
}