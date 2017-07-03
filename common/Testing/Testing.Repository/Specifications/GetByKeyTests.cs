using System;
using NUnit.Framework;
using NetSteps.Repository.Common.Specifications;
using StructureMap.AutoMocking;
using StructureMap.Pipeline;

namespace Testing.Repository.Specifications
{
    public class GetByKeyTests : GenericInteractionContext<GetByKeySpecification<FakeEntity>>
    {
        private FakeEntity _entity;
        private int _key;

        protected override void beforeEach()
        {
            _key = 1;
            base.beforeEach();
        }

        public override void ConstructUsing(SmartInstance<GetByKeySpecification<FakeEntity>> instance)
        {
            instance.Ctor<int>().Is(_key);
        }

        [Test]
        public void Matching_Key_Returns_True()
        {
            _entity = new FakeEntity {Id = _key};
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(_entity);
            Assert.IsTrue(result);
        }

        [Test]
        public void Non_Matching_Key_Returns_False()
        {
            const int otherKey = 2;
            _entity = new FakeEntity {Id = otherKey};
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(_entity);
            Assert.IsFalse(result);
        }
    }
}