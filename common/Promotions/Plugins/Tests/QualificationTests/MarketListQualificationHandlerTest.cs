using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using Moq;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class MarketListQualificationHandlerTest : BaseQualificationHandlerTestProxy<MarketListQualificationHandler>
    {
        [TestMethod]
        public void OrderTypeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.MarketListProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(MarketListQualificationHandler));
            }
        }

        [TestMethod]
        public void MarketListQualificationHandler_should_match_iforderContext_market_number_is_contained_in_market_number_list()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                MarketListQualificationHandler handler = new MarketListQualificationHandler();
                IMarketListQualificationExtension extension = Create.New<IMarketListQualificationExtension>();
                extension.Markets.Add(orderContext.Order.GetShippingMarketID());

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void MarketListQualificationHandler_should_not_match_iforderContext_market_number_is_not_contained_in_market_number_list()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                MarketListQualificationHandler handler = new MarketListQualificationHandler();
                IMarketListQualificationExtension extension = Create.New<IMarketListQualificationExtension>();
                int randomMarketID = 0;
                int shippingMarketID = orderContext.Order.GetShippingMarketID();
                while (randomMarketID == 0 && randomMarketID == shippingMarketID)
                {
                    randomMarketID = new Random().Next();
                }
                extension.Markets.Add(randomMarketID);

                var qual = new Mock<IPromotionQualification>().SetupAllProperties();
                qual.Object.Extension = extension;
                qual.Object.PromotionQualificationID = 0;
                qual.Object.ExtensionProviderKey = string.Empty;

                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }
    }
}
