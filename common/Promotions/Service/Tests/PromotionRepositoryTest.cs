using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Service.Repository;
using NetSteps.Promotions.Common.Cache;
using System.Collections.Generic;
using Moq;

namespace NetSteps.Promotions.Service.Tests
{
	[TestClass]
	public class PromotionRepositoryTest
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		private void mockWireup(IContainer container)
		{
            var promotionUnitOfWorkMock = new Mock<IPromotionUnitOfWork>();
			container.ForType<IPromotionUnitOfWork>()
				.Register<IPromotionUnitOfWork>()
				.ResolveAnInstancePerScope()
				.End();

            var scopedTestPromotionDataProvider  = new mockProvider();
            // this really needs to be removed from this test - the promotions provider doesn't do this stuff anymore so really we're not testing that functionality and
            // really ought not here.  In the interest of time I'm leaving it, but it needs to be refactored.
            scopedTestPromotionDataProvider
                .Setup(x => x.AddPromotion(It.IsAny<IPromotion>(), It.IsAny<IUnitOfWork>()))
                .Returns((IPromotion promo, IUnitOfWork unitOfWork) =>
                {
                    promo.PromotionID = scopedTestPromotionDataProvider.promotionList.Count() + 1;
                    scopedTestPromotionDataProvider.promotionList.Add(promo);
                    return promo;
                });
            scopedTestPromotionDataProvider
                .Setup(x => x.FindPromotions(It.IsAny<IPromotionUnitOfWork>(), It.IsAny<PromotionStatus>(), It.IsAny<IPromotionInterval>(), It.IsAny<Predicate<IPromotion>>(), It.IsAny<IEnumerable<string>>()))
                .Returns((IPromotionUnitOfWork unitOfWork, PromotionStatus promotionStatus, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds) =>
                {
                    return scopedTestPromotionDataProvider.promotionList.Where(promo => (
                                                                                (((int)promo.PromotionStatusTypeID & (int)promotionStatus) > 0) &&
                                                                                (!promo.StartDate.HasValue || !searchInterval.StartDate.HasValue || promo.StartDate.Value <= searchInterval.StartDate.Value) &&
                                                                                (!promo.EndDate.HasValue || !searchInterval.EndDate.HasValue || promo.EndDate.Value <= searchInterval.EndDate.Value) &&
                                                                                (!ofKinds.Any() || ofKinds.Contains(promo.PromotionKind)) &&
                                                                                filter(promo)
                                                                            )
                                                                    );
                });
            scopedTestPromotionDataProvider
                .Setup(x => x.FindPromotion(It.IsAny<int>(), It.IsAny<IUnitOfWork>()))
                .Returns((int promotionID, IUnitOfWork unitOfWork) =>
                {
                    return scopedTestPromotionDataProvider.promotionList.SingleOrDefault(x => x.PromotionID == promotionID);
                });

            Container.Current.ForType<IPromotionDataProvider>().Register<IPromotionDataProvider>((c, p) => { return scopedTestPromotionDataProvider.Object; }).ResolveAnInstancePerScope().End();
		}



        private class mockProvider : Mock<IPromotionDataProvider>
        {
            public IList<IPromotion> promotionList = new List<IPromotion>();
        }

		[TestMethod]
		public void PromotionRepository_should_insert_a_promotion()
		{
			using (var container = Create.NewContainer())
			{
				mockWireup(container); 
				var promotionUnitOfWorkMock = new Mock<IPromotionUnitOfWork>();
				var dataProvider = Create.New<IPromotionDataProvider>();
                
                var promotion = new Mock<IPromotion>();
                promotion.SetupAllProperties();
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                promotion.Object.StartDate = new DateTime(1991, 2, 3);
                promotion.Object.EndDate = DateTime.Today;
                promotion.Object.Description = DateTime.Now.ToString();

                dataProvider.AddPromotion(promotion.Object, promotionUnitOfWorkMock.Object);

                IPromotion foundProfile = dataProvider.FindPromotion(promotion.Object.PromotionID, promotionUnitOfWorkMock.Object);
                Assert.AreEqual(foundProfile.PromotionID, promotion.Object.PromotionID);
			}
		}

		[TestMethod]
		public void PromotionRepository_should_update_a_promotion()
		{
			using (var container = Create.NewContainer())
			{
				mockWireup(container); 
				var promotionUnitOfWorkMock = new Mock<IPromotionUnitOfWork>();
				var dataProvider = Create.New<IPromotionDataProvider>();
                
                var promotion = new Mock<IPromotion>();
                promotion.SetupAllProperties();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Disabled;
                
                var saved = dataProvider.AddPromotion(promotion.Object, promotionUnitOfWorkMock.Object);
				Assert.IsNotNull(saved);

                IPromotion foundProfile = dataProvider.FindPromotion(saved.PromotionID, promotionUnitOfWorkMock.Object);
				Assert.AreEqual(foundProfile.PromotionID, saved.PromotionID);
				Assert.IsFalse(foundProfile.PromotionStatusTypeID == (int)PromotionStatus.Enabled);

				foundProfile.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                dataProvider.UpdatePromotion(foundProfile, promotionUnitOfWorkMock.Object);
                IPromotion foundAgainProfile = dataProvider.FindPromotion(promotion.Object.PromotionID, promotionUnitOfWorkMock.Object);
				Assert.AreEqual(foundProfile.PromotionID, foundAgainProfile.PromotionID);
				Assert.IsTrue(foundProfile.PromotionStatusTypeID == (int)PromotionStatus.Enabled);
			}
		}
	}
}
