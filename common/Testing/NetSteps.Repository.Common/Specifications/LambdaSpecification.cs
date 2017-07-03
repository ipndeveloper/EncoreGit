using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    //for general purpose one off queries that you don't think need to be written out as their own specification
    public class LambdaSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public LambdaSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return _expression;
        }
    }
}