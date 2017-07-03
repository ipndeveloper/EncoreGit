using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Extensions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    /// <summary>
    /// Concrete implementation using an open generic for And Specification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _first;
        private readonly ISpecification<T> _second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            _first = first;
            _second = second;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return _first.IsSatisfied().And(_second.IsSatisfied());
        }
    }
}