using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Extensions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    public class OrSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _first;
        private readonly ISpecification<T> _second;

        public OrSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            _first = first;
            _second = second;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return _first.IsSatisfied().Or(_second.IsSatisfied());
        }
    }
}