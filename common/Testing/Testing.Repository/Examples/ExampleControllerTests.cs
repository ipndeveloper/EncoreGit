using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using NetSteps.Repository.Common.Examples;
using NetSteps.Repository.Common.Interfaces;
using NetSteps.Repository.Common.Specifications;
using Rhino.Mocks;

namespace Testing.Repository.Examples
{
    public class ExampleControllerTests : InteractionContext<ExampleController>
    {
        private IRepository _repository;

        protected override void beforeEach()
        {
            _repository = MockFor<IRepository>();
            base.beforeEach();
        }

        [Test]
        public void Get_Animal_By_Key_Uses_Specification()
        {
            const int key = 1;
            _repository.Expect(x => x.Single(Arg<GetByKeySpecification<Animal>>.Is.Anything));
            ClassUnderTest.GetAnimalByKey(key);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Find_Four_Legged_Animals_That_Starts_With_Uses_Specification_And_Returns_Results()
        {
            var results = new List<Animal>
            {
                new Animal {Name = "billyBob", NumberOfLegs = 4}
            };
            _repository.Expect(x => x.Query(Arg<ISpecification<Animal>>.Is.Anything))
                .Return(results.AsQueryable());
            var viewModel = ClassUnderTest.FindFourLeggedAnimalsThatStartWith("billy");
            _repository.VerifyAllExpectations();
            Assert.IsTrue(viewModel.Results.SequenceEqual(results));
        }
    }
}