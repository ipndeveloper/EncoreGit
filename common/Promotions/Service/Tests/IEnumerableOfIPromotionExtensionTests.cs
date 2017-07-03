using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using Moq;

namespace NetSteps.Promotions.Service.Tests
{
    [TestClass]
    public class IEnumerableOfIPromotionExtensionTests
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
        public void IPromotionEnumerableExtensions_WithQualification_should_filter_out_promotions_not_having_the_requested_qualification_type()
        {

            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                
                var promotions = new List<IPromotion>();

                var promotionMock1 = new Mock<IPromotion>();
                promotionMock1.SetupAllProperties();
                promotionMock1.Object.PromotionStatusTypeID = (int)PromotionStatus.Archived;
                promotionMock1.Object.PromotionQualifications = new Dictionary<string, IPromotionQualificationExtension>();
                promotionMock1.Object.PromotionQualifications.Add(Guid.NewGuid().ToString(), new Mock<IMockPromotionQualificationExtension>().Object);
                promotions.Add(promotionMock1.Object);

                var promotionMock2 = new Mock<IPromotion>();
                promotionMock2.SetupAllProperties();
                promotionMock2.Object.PromotionStatusTypeID = (int)PromotionStatus.Archived;
                promotionMock2.Object.PromotionQualifications = new Dictionary<string, IPromotionQualificationExtension>();
                promotions.Add(promotionMock2.Object);

                var filtered = promotions.WithQualification<IMockPromotionQualificationExtension>();
                Assert.IsTrue(promotions.Contains(promotionMock1.Object));
                Assert.IsTrue(promotions.Contains(promotionMock2.Object));
                Assert.IsTrue(filtered.Contains(promotionMock1.Object));
                Assert.IsFalse(filtered.Contains(promotionMock2.Object));
            }
        }

        public interface IMockPromotionQualificationExtension : IPromotionQualificationExtension { }
    }

}
