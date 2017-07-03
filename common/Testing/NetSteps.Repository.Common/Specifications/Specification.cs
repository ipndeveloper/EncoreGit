using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    /// <summary>
    /// Base abstract class of specification most will use this instead of implementing the interface directly.
    /// Open/Close principle open for extension closed for modification.
    /// The most common specifications are defined here using fluid interface to and/or/negate several together.
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// meets requirement predicate.
        /// </summary>
        /// <returns></returns>
        public abstract Expression<Func<T, bool>> IsSatisfied();

        /// <summary>
        /// Combines two Specifications together 
        /// </summary>
        /// <param name="otherSpecification"></param>
        /// <returns></returns>
        public Specification<T> And(ISpecification<T> otherSpecification)
        {
            return new AndSpecification<T>(this, otherSpecification);
        }

        /// <summary>
        /// Either or Specification
        /// </summary>
        /// <param name="otherSpecification"></param>
        /// <returns></returns>
        public Specification<T> Or(ISpecification<T> otherSpecification)
        {
            return new OrSpecification<T>(this, otherSpecification);
        }

        /// <summary>
        /// The opposite of the specification passed in. 
        /// </summary>
        /// <returns></returns>
        public Specification<T> Negated()
        {
            return new NegatedSpecification<T>(this);
        }
    }
}