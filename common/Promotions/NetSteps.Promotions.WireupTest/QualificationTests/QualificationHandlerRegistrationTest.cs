using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    [TestClass]
    public class QualificationHandlerRegistrationTest
    {
        [TestInitialize]
        public void Init()
        {
            
        }

		private void Wireup(IContainer current)
		{
            current.ForType<IPriceTypeService>()
                    .Register<IPriceTypeService>((c, p) => { return null; })
                    .ResolveAnInstancePerRequest()
                    .End();
            
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void AccountListQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.AccountListProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(AccountListQualificationHandler));
            }
        }

        [TestMethod]
        public void AccountTypeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.AccountTypeProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(AccountTypeQualificationHandler));
            }
        }

        [TestMethod]
        public void ProductInOrderQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.ProductInOrderProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(ProductInOrderQualificationHandler));
            }
        }

        [TestMethod]
        public void MarketListQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.MarketListProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(MarketListQualificationHandler));
            }
        }

        [TestMethod]
        public void OrderTypeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.OrderTypeProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(OrderTypeQualificationHandler));
            }
        }

        [TestMethod]
        public void PromotionCodeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                Wireup(container);

                IDataObjectExtensionProvider handler = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(QualificationExtensionProviderKeys.PromotionCodeProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(PromotionCodeQualificationHandler));
            }
        }

    }
}
