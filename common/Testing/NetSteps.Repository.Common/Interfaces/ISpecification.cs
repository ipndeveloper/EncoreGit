using System;
using System.Linq.Expressions;

namespace NetSteps.Repository.Common.Interfaces
{
    /// <summary>
    /// Open generic specification
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Predicate that returns if specification is met.
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> IsSatisfied();
    }
}
