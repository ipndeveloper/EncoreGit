using System;
using System.Linq.Expressions;
using NetSteps.Repository.Common.Specifications;

namespace NetSteps.Repository.Common.Examples
{
    public class AnimalsWithFirstNameStartingWith : Specification<Animal>
    {
        private readonly string _startsWith;

        public AnimalsWithFirstNameStartingWith(string startsWith)
        {
            _startsWith = startsWith;
        }

        public override Expression<Func<Animal, bool>> IsSatisfied()
        {
            return x => x.Name.StartsWith(_startsWith);
        }
    }
}