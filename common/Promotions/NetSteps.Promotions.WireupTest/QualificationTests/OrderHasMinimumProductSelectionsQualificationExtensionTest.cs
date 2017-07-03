using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    /// <summary>
    /// Summary description for OrderHasMinimumProductSelectionsQualificationExtensionTest
    /// </summary>
    [TestClass]
    public class OrderHasMinimumProductSelectionsQualificationExtensionTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }
        private void mockWireup()
        {
        }

        [TestMethod]
        public void OrderHasMinimumProductSelectionsQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.OrderHasMinimumProductOptionsProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(OrderHasMinimumProductSelectionsQualificationHandler));
            }
        }
    }
}
