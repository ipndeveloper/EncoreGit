using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Common.CoreImplementations;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using Moq;
using NetSteps.Promotions.Common.Cache;
using System.Collections.Generic;
using NetSteps.Data.Common;
using NetSteps.Promotions.Common.Repository;

namespace NetSteps.Promotions.Service.Tests
{
    [TestClass]
    public class NonCachingPromotionDataProviderTest
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
        }



        private class MockRepository : Mock<IPromotionRepository>
        {
            public IList<IPromotion> storedPromotions = new List<IPromotion>();

            public MockRepository()
            {
                this
                    .Setup(x => x.InsertPromotion(It.IsAny<IPromotion>(), It.IsAny<IUnitOfWork>()))
                    .Returns((IPromotion promo, IUnitOfWork unitOfWork) =>
                    {
                        promo.PromotionID = storedPromotions.Count() + 1;
                        storedPromotions.Add(promo);
                        return promo;
                    });

                this
                    .Setup(x => x.UpdateExistingPromotion(It.IsAny<IPromotion>(), It.IsAny<IUnitOfWork>()))
                    .Returns((IPromotion promotion, IUnitOfWork unitOfWork) =>
                    {
                        var found = storedPromotions.SingleOrDefault(x => x.PromotionID == promotion.PromotionID);
                        if (found != null)
                        {
                            storedPromotions.Remove(found);
                        }
                        storedPromotions.Add(promotion);
                        return promotion;
                    });

                this
                    .Setup(x => x.RetrievePromotion(It.IsAny<int>(), It.IsAny<IUnitOfWork>()))
                    .Returns((int promotionID, IUnitOfWork unitOfWork) =>
                    {
                        return storedPromotions.SingleOrDefault(x => x.PromotionID == promotionID);
                    });
            }
        }

        //[TestMethod]
        //public void NonCachingPromotionDataProvider_should_insert_a_promotion()
        //{
        //    using (var container = Create.NewContainer())
        //    {
        //        mockWireup(container);
        //        var promotionUnitOfWorkMock = new Mock<IPromotionUnitOfWork>();

        //        var promotionRepositoryMock = new MockRepository();

        //        var dataProvider = new NonCachingPromotionDataProvider(promotionRepositoryMock.Object);

        //        var promotion = new Mock<IPromotion>();
        //        promotion.SetupAllProperties();
        //        promotion.Object.PromotionID = new Random().Next();
        //        promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
        //        promotion.Object.StartDate = new DateTime(1991, 2, 3);
        //        promotion.Object.EndDate = DateTime.Today;
        //        promotion.Object.Description = DateTime.Now.ToString();

        //        dataProvider.AddPromotion(promotion.Object, promotionUnitOfWorkMock.Object);

        //        IPromotion foundProfile = dataProvider.FindPromotion(promotion.Object.PromotionID, promotionUnitOfWorkMock.Object);
        //        Assert.AreEqual(foundProfile.PromotionID, promotion.Object.PromotionID);
        //    }
        //}

        //[TestMethod]
        //public void NonCachingPromotionDataProvider_should_update_a_promotion()
        //{
        //    using (var container = Create.NewContainer())
        //    {
        //        mockWireup(container);
        //        var promotionUnitOfWorkMock = new Mock<IPromotionUnitOfWork>();

        //        var promotionRepositoryMock = new MockRepository();
                
        //        var dataProvider = new NonCachingPromotionDataProvider(promotionRepositoryMock.Object);

        //        var newPromotion = new Mock<IPromotion>();
        //        newPromotion.SetupAllProperties();
        //        newPromotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Disabled;

        //        var saved = dataProvider.AddPromotion(newPromotion.Object, promotionUnitOfWorkMock.Object);
        //        Assert.IsNotNull(saved);

        //        IPromotion foundProfile = dataProvider.FindPromotion(saved.PromotionID, promotionUnitOfWorkMock.Object);
        //        Assert.AreEqual(foundProfile.PromotionID, saved.PromotionID);
        //        Assert.IsFalse(foundProfile.PromotionStatusTypeID == (int)PromotionStatus.Enabled);

        //        foundProfile.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
        //        dataProvider.UpdatePromotion(foundProfile, promotionUnitOfWorkMock.Object);
        //        IPromotion foundAgainProfile = dataProvider.FindPromotion(newPromotion.Object.PromotionID, promotionUnitOfWorkMock.Object);
        //        Assert.AreEqual(foundProfile.PromotionID, foundAgainProfile.PromotionID);
        //        Assert.IsTrue(foundProfile.PromotionStatusTypeID == (int)PromotionStatus.Enabled);
        //    }
        //}
    }
}
