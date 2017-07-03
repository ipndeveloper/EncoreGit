using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Tests
{
    /// <summary>
    /// Summary description for CompositeQualificationTest
    /// </summary>
    [TestClass]
    public class AccountConsistencyStatusQualificationHandlerTest : BaseQualificationHandlerTestProxy<AccountConsistencyStatusQualificationHandler>
    {
        private void MockWireup(IContainer current)
        {

        }

        [TestMethod]
        public void AccountConsistencyQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                MockWireup(container);

                // test the IOC registration to verify that we've got it correct

                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.AccountConsistencyStatusProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(AccountConsistencyStatusQualificationHandler));
            }
        }
    }
}
