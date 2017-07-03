using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Foundation.Entity;

namespace NetSteps.Foundation.Entity.Tests
{
    [TestClass]
    public class EntityModelRepositoryTests
    {
        [TestMethod]
        public void Add_AddsAndReturnsMatchingEntity()
        {
            var test = new
            {
                ModelName = "The name of my model object"
            };
            var context = new SampleContext();
            var model = new SampleModel { ModelName = test.ModelName };
            var repository = new SampleEntityModelRepository();

            var entity = repository.Add(context, model);

            Assert.IsNotNull(entity);
            Assert.AreEqual(test.ModelName, entity.EntityName);
            Assert.AreEqual(1, context.SampleEntities.Count());
            Assert.AreEqual(test.ModelName, context.SampleEntities.Single().EntityName);
        }
    }
}
