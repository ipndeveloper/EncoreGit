using System;
using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Locale.Common;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Promotions.UI.Service.Impl;
using Rhino.Mocks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core;

namespace Tests
{
    [TestClass]
    public class TestPromotionInfoServiceMS : MsTestInteractionContext<PromotionInfoService>
    {

        private List<IPromotionAccountAlert> promotionAccountAlerts;
        private Dictionary<string, IPromotionQualificationExtension> allQualifications;
        private IPromotionService promotionService;
        //private IPromotionInfoContext info;
        //private IContentProxyService contentProxyService;
        //private IContentProxyData proxyData;
        //private ITermLocalizedProxy termLocalized;
        private IPromotion promotion;
        private IPromotionCodeQualificationExtension qualification;
        private DateTime endPromotion;
        private ILocalizationInfo localizationInfo;

        public void AddCouponQualification(string name)
        {
            qualification = MockFor<IPromotionCodeQualificationExtension>();
            qualification.Stub(x => x.PromotionCode).Return(name);
            allQualifications.Add("PromoCode", qualification);
        }


        protected override void beforeEach()
        {
            using (var container = Create.NewContainer())
            {
                NetSteps.Encore.Core.Wireup.WireupCoordinator.SelfConfigure();

                promotionAccountAlerts = new List<IPromotionAccountAlert>();
                allQualifications = new Dictionary<string, IPromotionQualificationExtension>();
                promotion = MockFor<IPromotion>();
                AddCouponQualification("sd9135h235lkujfsdyhgwy4");
                promotion.Stub(x => x.PromotionQualifications).Return(allQualifications);
                promotionService = MockFor<IPromotionService>();
                //info = MockFor<IPromotionInfoContext>();
                //proxyData = MockFor<IContentProxyData>();
                //termLocalized = MockFor<ITermLocalizedProxy>();
                //proxyData.Stub(x => x.AllTerms).Return(termLocalized);
                //contentProxyService = MockFor<IContentProxyService>();
                //info.Stub(x => x.ContentService).Return(contentProxyService);
                //info.Stub(x => x.PromotionService).Return(promotionService);
                endPromotion = DateTime.Today;
                promotion.Stub(x => x.EndDate).Return(endPromotion);

                localizationInfo = Create.New<ILocalizationInfo>();
                localizationInfo.CultureName = "en-US";
                localizationInfo.LanguageId = 1;

                //termLocalized.Expect(x => x.GetValue(PromotionInfoService.ConstantsAlert.AlertTitleText, "")).Return("oh alerting title");

                //termLocalized.Expect(x => x.GetValue(PromotionInfoService.ConstantsPromotion.PromotionAction, "")).Return("action of promo");
                //termLocalized.Expect(x => x.GetValue(PromotionInfoService.ConstantsPromotion.PromotionDescription, "")).Return("description of promo");
                //termLocalized.Expect(x => x.GetValue(PromotionInfoService.ConstantsPromotion.PromotionExpired, "")).Return("promo expires");
                //termLocalized.Expect(x => x.GetValue(PromotionInfoService.ConstantsPromotion.PromotionTitle, "")).Return("promotion title");
                base.beforeEach();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPromotionsAccountAlertsNullInput()
        {
            var result = ClassUnderTest.GetPromotionsAccountAlerts(null, null);
        }

        [TestMethod]
        public void TestAvailablePromotionsForAccountEmptyInput()
        {
            var result = ClassUnderTest.GetPromotionsAccountAlerts(promotionAccountAlerts, localizationInfo);
            result.ShouldHaveCount(0);
        }

        /// <summary>
        //Ensures that success case works
        /// </summary>
        //[TestMethod]
        //public void TestAvailablePromotionsForAccountAlertSuccess()
        //{
        //    contentProxyService.Stub(x => x.GetContentForAlert(20)).Return(null);
        //    contentProxyService.Stub(x => x.GetContentForPromotion(20)).Return(null);

        //    contentProxyService.Stub(x => x.GetContentForAlert(100)).Return(proxyData);
        //    contentProxyService.Stub(x => x.GetContentForPromotion(100)).Return(proxyData);

        //    promotion.Description = "description of promo";
        //    var myInfo = MockFor<IPromotionAccountAlert>();
        //    myInfo.Expect(x => x.PromotionId).Return(20).Repeat.Once();
        //    myInfo.Expect(x => x.AccountAlertId).Return(20).Repeat.Once();
        //    promotionAccountAlerts.Add(myInfo);
        //    myInfo.Expect(x => x.PromotionId).Return(100);
        //    myInfo.Expect(x => x.AccountAlertId).Return(100);
        //    promotionAccountAlerts.Add(myInfo);
        //    //I noticed that there was a predicate filter so i decicided that it was probably better to use that...i am ignoring
        //    //arguments because they are irrelevant.
        //    promotionService.Expect(x => x.GetPromotion(100)).Return(promotion);

        //    var result = ClassUnderTest.GetPromotionsAccountAlerts(promotionAccountAlerts).ToList();

        //    //which is more valid should i still yield results for content that wasn't found?
        //    Assert.AreEqual(2, result.Count());

        //    var alertContentResult = result.ElementAt(1);
        //    Assert.AreEqual("oh alerting title", alertContentResult.PartialTitle);
        //    Assert.AreEqual("promotion title", alertContentResult.Title);
        //    Assert.AreEqual("sd9135h235lkujfsdyhgwy4", alertContentResult.CouponCode);
        //    Assert.AreEqual("description of promo", alertContentResult.Description);
        //    Assert.AreEqual(alertContentResult.ExpiredDate, endPromotion);

        //}


        /// <summary>
        /// Tests the success case for getting display information about account alerts.
        /// </summary>
        //[TestMethod]
        //public void TestGetPromotionDisplayInfoSuccess()
        //{
        //    string[] imagesPaths = new[] { "path1", "path2" };
        //    proxyData.Expect(x => x.GetFilePathsRelatedTo()).Return(imagesPaths);
        //    contentProxyService.Stub(x => x.GetContentForPromotion(20)).Return(null);
        //    contentProxyService.Stub(x => x.GetContentForPromotion(100)).Return(proxyData);

        //    List<IPromotion> promotions = new List<IPromotion>();
        //    promotion.Stub(x => x.PromotionID).Return(20).Repeat.Once();
        //    promotions.Add(promotion);
        //    promotion.Stub(x => x.PromotionID).Return(100).Repeat.Once();
        //    promotions.Add(promotion);
        //    promotionService.Expect(x => x.GetPromotion(20)).Return(null);
        //    promotionService.Expect(x => x.GetPromotion(100)).Return(promotion);

        //    var result = ClassUnderTest.GetPromotionDisplayInfo(promotions).ToList();

        //    Assert.AreEqual(1, result.Count());
        //    var promoInfo = result.ElementAt(0);

        //    Assert.AreEqual(promoInfo.ActionText, "action of promo");
        //    Assert.AreEqual(promoInfo.CouponCode, "sd9135h235lkujfsdyhgwy4");
        //    Assert.AreEqual(promoInfo.Description, "description of promo");
        //    Assert.AreEqual(promoInfo.Expired, "promo expires");
        //    Assert.AreEqual(promoInfo.ExpiredDate, endPromotion);
        //    CollectionAssert.AreEqual(promoInfo.ImagePaths.ToArray(), imagesPaths);
        //    Assert.AreEqual(promoInfo.Title, "promotion title");
        //}

        [TestMethod]
        public void TestGetAvailablePromotionsForAccount()
        {
            //I don't really care about what predicate is passed in that's why i ignore the args.
            promotionService.Expect(x => x.GetPromotions<IPromotion>(y => y.PromotionID == 0)).IgnoreArguments().Return(new List<IPromotion>());
            var result = ClassUnderTest.GetAvailablePromotionsForAccount<IPromotion>(p => p.PromotionID == 1);
            Assert.IsNotNull(result);
            promotionService.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestSetDisplayValuesNull()
        {
            var display = MockFor<IDisplayInfo>();
            //if i pass a null for the promotion nothing should happen
            ClassUnderTest.SetDisplayValues(null, display);
            display.VerifyAllExpectations();// nothing on the display should be called.
        }

        [TestMethod]
        public void TestSetDisplayValuesSuccess()
        {

            var promo = MockFor<IPromotion>();
            promo.Stub(x => x.EndDate).Return(endPromotion);
            promo.Stub(x => x.PromotionQualifications).Return(allQualifications);
            var display = MockRepository.GenerateStub<IDisplayInfo>();

            ClassUnderTest.SetDisplayValues(promo, display);

            Assert.AreEqual("sd9135h235lkujfsdyhgwy4", display.CouponCode);
            Assert.AreEqual(endPromotion, display.ExpiredDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPromotionDisplayInfoNull()
        {
            var displayInfo = ClassUnderTest.GetPromotionDisplayInfo(null, null);
        }


        //[TestMethod]
        //public void TestSetDisplayValuesFromProviderPromo()
        //{
        //    ClassUnderTest.SetDisplayValuesFromProviderPromo(null, null);// nothing blows up and nothing happens.

        //    var display = MockRepository.GenerateStub<IDisplayInfo>();
        //    var paths = new[] { "path1", "path2" };
        //    proxyData.Expect(x => x.GetFilePathsRelatedTo()).Return(paths);

        //    ClassUnderTest.SetDisplayValuesFromProviderPromo(display, proxyData);

        //    Assert.AreEqual(display.Description, "description of promo");
        //    Assert.AreEqual(display.Expired, "promo expires");
        //    Assert.AreEqual(display.Title, "promotion title");
        //    Assert.AreEqual(display.ActionText, "action of promo");
        //    CollectionAssert.AreEqual(display.ImagePaths.ToArray(), paths);
        //}

        //[TestMethod]
        //public void TestSetDisplayValuesFromProviderAlert()
        //{
        //    ClassUnderTest.SetDisplayValuesFromProviderAlert(null, null);//nothing blows up and happens

        //    var display = MockRepository.GenerateStub<IAlertInfo>();

        //    //ClassUnderTest.SetDisplayValuesFromProviderAlert(display, proxyData);

        //    Assert.AreEqual(display.PartialTitle, "oh alerting title");

        //}

        [TestMethod]
        public void TestFetchCode()
        {
            var promo = MockRepository.GenerateStub<IPromotion>();
            var qualifications = new Dictionary<string, IPromotionQualificationExtension>();

            var q1 = MockRepository.GenerateStub<IProductInOrderQualificationExtension>();
            q1.Quantity = 100;

            var q2 = MockRepository.GenerateStub<IPromotionCodeQualificationExtension>();
            q2.PromotionCode = "something";
            qualifications.Add("test1", q1);
            qualifications.Add("test2", q2);

            promo.PromotionQualifications = qualifications;
            string result = ClassUnderTest.FetchCode(promo);
            Assert.AreEqual("something", result);
        }
    }
}
