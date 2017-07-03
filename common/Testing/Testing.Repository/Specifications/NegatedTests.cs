using System;
using System.Linq.Expressions;
using NUnit.Framework;
using NetSteps.Repository.Common.Interfaces;
using NetSteps.Repository.Common.Specifications;
using StructureMap.Pipeline;

namespace Testing.Repository.Specifications
{
    public class NegatedTests : GenericInteractionContext<NegatedSpecification<FakeEntity>>
    {
        private NameIsCertainLength _specification;

        protected override void beforeEach()
        {
            //only match 5 letter words
            _specification = new NameIsCertainLength(5);
            base.beforeEach();
        }

        public override void ConstructUsing(SmartInstance<NegatedSpecification<FakeEntity>> instance)
        {
            instance.Ctor<ISpecification<FakeEntity>>().Is(_specification);
        }

        [Test]
        public void Entity_Matching_Original_Criteria_Returns_False()
        {
            var entity = new FakeEntity { Name = "Apple" };
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsFalse(result);
        }

        [Test]
        public void Entity_Not_Matching_Original_Criteria_Returns_True()
        {
            var entity = new FakeEntity { Name = "Apples" };
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsTrue(result);
        }

        private class NameIsCertainLength : ISpecification<FakeEntity>
        {
            private readonly int _length;

            public NameIsCertainLength(int length)
            {
                _length = length;
            }

            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.Name.Length == _length;
            }
        }
    }
}