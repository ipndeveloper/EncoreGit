using System;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    public class NegatedSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _original;

        public NegatedSpecification(ISpecification<T> original)
        {
            _original = original;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            var originalExpression = _original.IsSatisfied();
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(originalExpression.Body),
                originalExpression.Parameters.Single()
                );
        }
    }
}