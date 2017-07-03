using System;
using System.Linq.Expressions;
using NUnit.Framework;
using NetSteps.Repository.Common.Interfaces;
using NetSteps.Repository.Common.Specifications;
using StructureMap.Pipeline;

namespace Testing.Repository.Specifications
{
    public class AndTests : GenericInteractionContext<AndSpecification<FakeEntity>>
    {
        private NameStartsWithTest _startsWithTest;
        private NameEndsWithModel _endsWithModel;

        protected override void beforeEach()
        {
            _startsWithTest = new NameStartsWithTest();
            _endsWithModel = new NameEndsWithModel();
            base.beforeEach();
        }
        
        private class NameStartsWithTest : ISpecification<FakeEntity>
        {
            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.Name.StartsWith("Test");
            }
        }

        private class NameEndsWithModel : ISpecification<FakeEntity>
        {
            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.Name.EndsWith("Model");
            }
        }

        /// <summary>
        /// Makes sure that our constructor is called with the arguments we want
        /// Normally you don't need to be this specific, but when you take in two things of the same type
        /// the container needs more information to be able to resolve the correct dependencies
        /// </summary>
        /// <param name="instance"></param>
        public override void ConstructUsing(SmartInstance<AndSpecification<FakeEntity>> instance)
        {
            instance.Ctor<ISpecification<FakeEntity>>("first").Is(_startsWithTest);
            instance.Ctor<ISpecification<FakeEntity>>("second").Is(_endsWithModel);
        }

        [Test]
        public void Entity_Matching_Both_Criteria_Returns_True()
        {
            var entity = new FakeEntity {Name = "TestSomethingModel"};
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsTrue(result);
        }

        [Test]
        public void Entity_Matching_First_But_Not_Second_Criteria_Returns_False()
        {
            var entity = new FakeEntity {Name = "TestSomething"};
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsFalse(result);
        }

        [Test]
        public void Entity_Matching_Second_But_Not_First_Criteria_Returns_False()
        {
            var entity = new FakeEntity {Name = "SomethingModel"};
            var expression = ClassUnderTest.IsSatisfied();
            var result = expression.Compile().Invoke(entity);
            Assert.IsFalse(result);
        }


    
    }
}