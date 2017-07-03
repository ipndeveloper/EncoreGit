using System;
using System.Linq.Expressions;
using NUnit.Framework;
using NetSteps.Repository.Common.Interfaces;
using NetSteps.Repository.Common.Specifications;
using StructureMap.Pipeline;

namespace Testing.Repository.Specifications
{
    public class OrTests : GenericInteractionContext<OrSpecification<FakeEntity>>
    {
        private int _minimumAge;
        private MinimumAgeSpecification _minimumAgeSpecification;
        private IsSpecialSpecification _isSpecialSpecification;

        protected override void beforeEach()
        {
            _minimumAge = 21;
            _minimumAgeSpecification = new MinimumAgeSpecification(_minimumAge);
            _isSpecialSpecification = new IsSpecialSpecification();
            base.beforeEach();
        }

        public override void ConstructUsing(SmartInstance<OrSpecification<FakeEntity>> instance)
        {
            instance.Ctor<ISpecification<FakeEntity>>("first").Is(_minimumAgeSpecification);
            instance.Ctor<ISpecification<FakeEntity>>("second").Is(_isSpecialSpecification);
        }

        [Test]
        public void Entity_Matching_First_Criteria_Returns_True()
        {
            var entity = new FakeEntity { Age = 22 };
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsTrue(result);
        }

        [Test]
        public void Entity_Matching_Second_Criteria_Returns_True()
        {
            var entity = new FakeEntity { IsSpecial = true };
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsTrue(result);
        }

        [Test]
        public void Entity_Not_Matching_Either_Criteria_Returns_False()
        {
            var entity = new FakeEntity { IsSpecial = false, Age = 20 };
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsFalse(result);
        }

        private class MinimumAgeSpecification : ISpecification<FakeEntity>
        {
            private readonly int _minimumAge;

            public MinimumAgeSpecification(int minimumAge)
            {
                _minimumAge = minimumAge;
            }

            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.Age >= _minimumAge;
            }
        }

        private class IsSpecialSpecification : ISpecification<FakeEntity>
        {
            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.IsSpecial;
            }
        }
    }
}