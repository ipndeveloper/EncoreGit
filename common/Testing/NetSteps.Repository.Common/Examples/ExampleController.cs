using System.Linq;
using NetSteps.Repository.Common.Interfaces;
using NetSteps.Repository.Common.Specifications;

namespace NetSteps.Repository.Common.Examples
{
    public class ExampleController
    {
        private readonly IRepository _repository;

        public ExampleController(IRepository repository)
        {
            _repository = repository;
        }

        public Animal GetAnimalByKey(int key)
        {
            var specification = new GetByKeySpecification<Animal>(key);
            return _repository.Single(specification);
        }

        public ExampleViewModel FindFourLeggedAnimalsThatStartWith(string startsWith)
        {
            var isFourLegged = new LambdaSpecification<Animal>(x => x.NumberOfLegs == 4);
            var specification = new AnimalsWithFirstNameStartingWith(startsWith).And(isFourLegged);
            var results = _repository.Query(specification);

            return new ExampleViewModel
            {
                Results = results.ToList()
            };
        }

        public void GetFirstAnimalThatDoesntStartWith(string startsWith)
        {
            var specification = new AnimalsWithFirstNameStartingWith(startsWith).Negated();
            var result = _repository.First(specification);
            //do something with result
        }
    }
}