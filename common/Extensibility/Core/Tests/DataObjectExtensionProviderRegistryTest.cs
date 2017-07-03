using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.Extensibility.Test
{
    [TestClass]
    public class DataObjectExtensionProviderRegistryTest
    {
        [TestMethod]
        public void DataObjectExtensionProviderRegistry_should_be_registered()
        {
            // test the IOC registration to verify that we've got it correct
            var dataObjectProviderRegistry = Create.New<IDataObjectExtensionProviderRegistry>();
            Assert.IsNotNull(dataObjectProviderRegistry);
            Assert.IsInstanceOfType(dataObjectProviderRegistry, typeof(IDataObjectExtensionProviderRegistry));
        }
    }
}
