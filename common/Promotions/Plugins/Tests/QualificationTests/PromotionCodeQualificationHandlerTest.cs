using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Context;
using Moq;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
	[TestClass]
	public class PromotionCodeQualificationHandlerTest : BaseQualificationHandlerTestProxy<PromotionCodeQualificationHandler>
	{
		private void mockWireup()
		{
		}

		[TestMethod]
		public void PromotionCodeQualificationHandler_should_be_registered()
		{
			using (var container = Create.NewContainer())
			{
				mockWireup();
				// test the IOC registration to verify that we've got it correct
				var registry = Create.New<IDataObjectExtensionProviderRegistry>();
				var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.PromotionCodeProviderKey);
				Assert.IsNotNull(handler);
				Assert.IsInstanceOfType(handler, typeof(PromotionCodeQualificationHandler));
			}
		}

		[TestMethod]
		public void PromotionCodeQualificationHandler_should_match_if_order_contains_matching_promo_code()
		{
			using (var container = Create.NewContainer())
			{
				mockWireup(); 
                
                var orderContext = GetMockOrderContext(1);

                var couponCode = new Mock<ICouponCode>().SetupAllProperties();
                couponCode.Object.AccountID = 1;
                couponCode.Object.CouponCode = "test1";

				orderContext.CouponCodes.Add(couponCode.Object);

				PromotionCodeQualificationHandler handler = new PromotionCodeQualificationHandler();
				IPromotionCodeQualificationExtension extension = Create.New<IPromotionCodeQualificationExtension>();
				extension.PromotionCode = "test1";

				Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
			}
		}

		[TestMethod]
		public void PromotionCodeQualificationHandler_should_not_match_if_order_does_not_contain_matching_promo_code()
		{
			using (var container = Create.NewContainer())
			{
                var orderContext = GetMockOrderContext(1);

                var couponCode = new Mock<ICouponCode>().SetupAllProperties();
                couponCode.Object.AccountID = 1;
                couponCode.Object.CouponCode = "test1";

                orderContext.CouponCodes.Add(couponCode.Object);

				PromotionCodeQualificationHandler handler = new PromotionCodeQualificationHandler();
				IPromotionCodeQualificationExtension extension = Create.New<IPromotionCodeQualificationExtension>();

				Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
			}
		}

		[TestMethod]
		public void PromotionCodeQualificationHandler_should_not_match_if_order_contains_alternate_promo_code()
		{
			using (var container = Create.NewContainer())
			{
                var orderContext = GetMockOrderContext(1);

                var couponCode = new Mock<ICouponCode>().SetupAllProperties();
                couponCode.Object.AccountID = 1;
                couponCode.Object.CouponCode = "test1";

                orderContext.CouponCodes.Add(couponCode.Object);

				PromotionCodeQualificationHandler handler = new PromotionCodeQualificationHandler();
				IPromotionCodeQualificationExtension extension = Create.New<IPromotionCodeQualificationExtension>();
				extension.PromotionCode = "test2";

				Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
			}
		}
	}
}
