using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FubuTestingSupport;
using NUnit.Framework;
using NetSteps.Repository.Common.Interfaces;
using Rhino.Mocks;
using Testing.Repository.Context;
using Testing.Repository.Specifications;

namespace Testing.Repository.Examples
{
    /// <summary>
    /// Tests the inMemoryRepository
    /// </summary>
    public class InMemoryRepositoryTests : InteractionContext<InMemoryRepository>
    {
        private IObjectContext _objContext;
        private HashSet<FakeEntity> _fakeHash;

        protected override void beforeEach()
        {
            _objContext = MockFor<IObjectContext>();
            _fakeHash = new HashSet<FakeEntity>
            {
                new FakeEntity {Age = 12, Id = 3, IsSpecial = false, Name = "Billy"}
            };
            _objContext.Expect(x => x.GetObjects<FakeEntity>()).Return(_fakeHash);
        }

        /// <summary>
        /// Tests that we can get by key
        /// </summary>
        [Test]
        public void TestGetByKeySuccess()
        {
            var result = ClassUnderTest.GetByKey<FakeEntity>(3);
            Assert.IsNotNull(result);
            _objContext.VerifyAllExpectations();
        }
        /// <summary>
        /// Tests that we can get by key
        /// </summary>
        [Test]
        public void TestGetByKeyFailure()
        {
            var result = ClassUnderTest.GetByKey<FakeEntity>(2);
            Assert.IsNull(result);
            _objContext.VerifyAllExpectations();
        }

        /// <summary>
        /// Make sure we can fetch by specification
        /// </summary>
        [Test]
        public void TestQuerySuccess()
        {
            var results = ClassUnderTest.Query(new NameEqualsSpecification("Billy"));
            Assert.AreEqual(1, results.Count());
            _objContext.VerifyAllExpectations();
        }

        /// <summary>
        /// Make sure when specification doesn't match no results are returned
        /// </summary>
        [Test]
        public void TestQueryFailure()
        {
            var results = ClassUnderTest.Query(new NameEqualsSpecification("Bill"));
            Assert.AreEqual(0, results.Count());
            _objContext.VerifyAllExpectations();
        }

        /// <summary>
        /// Ensure that when fetching unique result result is not null.
        /// </summary>
        [Test]
        public void TestSingleSuccess()
        {
            var result = ClassUnderTest.Single(new NameEqualsSpecification("Billy"));
            Assert.NotNull(result);
            _objContext.VerifyAllExpectations();
        }

        /// <summary>
        /// Tests that when fetching non unqiue single result that is throws exception
        /// </summary>
        [ExpectedException]
        [Test]
        public void TestSingleFailure()
        {
            _fakeHash.Add(new FakeEntity { Name = "Billy" });
            ClassUnderTest.Single(new NameEqualsSpecification("Billy"));
            _objContext.VerifyAllExpectations();
        }
        /// <summary>
        /// Tests when fetching the first result.
        /// </summary>
        [Test]
        public void TestFirstSuccess()
        {
            _fakeHash.Add(new FakeEntity { Name = "Billy", Age = 23 });
            var result = ClassUnderTest.First(new NameEqualsSpecification("Billy"));
            Assert.AreEqual(12, result.Age);
            _objContext.VerifyAllExpectations();
        }
        /// <summary>
        /// Tests when fetching the first result and none exisit does not throw an exception
        /// </summary>
        [Test]
        public void TestFirstNoneExisits()
        {
            var result = ClassUnderTest.First(new NameEqualsSpecification("Test"));
            Assert.IsNull(result);
            _objContext.VerifyAllExpectations();
        }
        /// <summary>
        /// Tests that we can add an entity.
        /// </summary>
        [Test]
        public void TestAdd()
        {
            var entityToAdd = new FakeEntity { Age = 33, Id = 100, Name = "new" };
            //expect the context to take the same instance created above.
            //create a callback to force the same entity added to our test set.
            _objContext.Expect(x => x.Add(Arg.Is(entityToAdd))).Callback<FakeEntity>(_fakeHash.Add);
            //call get by key for the expectation of get objects.
            ClassUnderTest.Add(entityToAdd);
            var result = ClassUnderTest.GetByKey<FakeEntity>(100);
            Assert.IsNotNull(result);
            _objContext.VerifyAllExpectations();//if get by key was not called then our expectations would fail.
        }
        /// <summary>
        /// Tests that we can update an entity.
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            var entity = new FakeEntity { Age = 13, Id = 3, IsSpecial = true, Name = "Billy" };

            _objContext.Expect(x => x.Delete(Arg.Is(entity)));
            _objContext.Expect(x => x.Add(Arg.Is(entity)));

            ClassUnderTest.Update(entity);

            _objContext.VerifyAllExpectations();
        }

        /// <summary>
        /// Tests that we can delete an entity
        /// </summary>
        [Test]
        public void TestDelete()
        {
            _objContext.Expect(x => x.Delete(Arg<FakeEntity>.Is.Anything));
            int result = ClassUnderTest.Delete(new NameEqualsSpecification("Billy"));
            _objContext.VerifyAllExpectations();
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Test helper simple test for == name
        /// </summary>
        private class NameEqualsSpecification : ISpecification<FakeEntity>
        {
            private readonly string _name;

            public NameEqualsSpecification(string name)
            {
                _name = name;
            }

            public Expression<Func<FakeEntity, bool>> IsSatisfied()
            {
                return x => x.Name == _name;
            }
        }
    }
}
