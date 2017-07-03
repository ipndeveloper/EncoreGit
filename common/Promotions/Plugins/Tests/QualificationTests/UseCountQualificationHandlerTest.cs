using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.OrderAdjustments.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using Moq;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class UseCountQualificationHandlerTest : BaseQualificationHandlerTestProxy<UseCountQualificationHandler>
    {
       [TestMethod]
        public void UseCountQualificationHandler_should_be_registered()
        {

            using (var container = Create.NewContainer())
            {
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.UseCountProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(UseCountQualificationHandler));
            }
        }

        [TestMethod]
        public void UseCountQualificationHandler_should_match_if_the_user_has_not_used_the_promotion_in_a_previous_order()
        {

            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                var mockOrderService = new Mock<IOrderService>();
                var mockPromotionProvider = new Mock<IPromotionProvider>();
                var mockUseCountRepository = new Mock<IUseCountQualificationRepository>();

                var handler = new UseCountQualificationHandler(mockOrderService.Object, mockPromotionProvider.Object, mockUseCountRepository.Object, () =>
                    {
                        var uowMock = new Mock<IEncorePromotionsPluginsUnitOfWork>().SetupAllProperties();
                        return uowMock.Object;
                    });
                IUseCountQualificationExtension extension = Create.New<IUseCountQualificationExtension>();
                extension.MaximumUseCount = 1;

                orderContext.Order.AddItem(new Random().Next(), 1);
                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void UseCountQualificationHandler_should_not_match_if_the_user_has_used_the_promotion_in_a_previous_order()
        {

            var repository = new Mock<IUseCountQualificationRepository>();

            var orderContext = GetMockOrderContext(1);

            var mockOrderService = new Mock<IOrderService>();
            var mockPromotionProvider = new Mock<IPromotionProvider>();
            var mockUseCountRepository = new Mock<IUseCountQualificationRepository>();

            var handler = new UseCountQualificationHandler(mockOrderService.Object, mockPromotionProvider.Object, mockUseCountRepository.Object, () =>
            {
                var uowMock = new Mock<IEncorePromotionsPluginsUnitOfWork>().SetupAllProperties();
                return uowMock.Object;
            });

            IUseCountQualificationExtension extension = Create.New<IUseCountQualificationExtension>();
            extension.MaximumUseCount = 1;

            repository.Object.RecordUse(extension, orderContext, null);

            var PromotionProvider = Create.New<IPromotionProvider>();
            using (var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>())
            {
                Create.New<IUseCountQualificationRepository>().RecordUse(extension, orderContext, unitOfWork);
            }
            Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
        }
    }
}
