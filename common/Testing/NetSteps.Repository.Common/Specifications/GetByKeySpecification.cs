using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Specifications
{
    //this is a common enough operation it probably deserves to just be a method in IRepository -> GetByKey(object key)
    public class GetByKeySpecification<T> : ISpecification<T> where T : IEntity
    {
        private readonly int _key;

        public GetByKeySpecification(int key)
        {
            _key = key;
        }

        public Expression<Func<T, bool>> IsSatisfied()
        {
            return x => x.Id == _key;
        }
    }
}