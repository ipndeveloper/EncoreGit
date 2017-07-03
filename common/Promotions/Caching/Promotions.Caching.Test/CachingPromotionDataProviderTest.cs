using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Cache;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Caching;
using Moq;

namespace Promotions.Caching.Test
{
	[TestClass]
	public class CachingPromotionDataProviderTest
	{
        private void MockWireup(IContainer container, IPromotionRepository repository)
        {
            container.ForType<IPromotionUnitOfWork>()
                .Register<IPromotionUnitOfWork>((c, p) => { return new Mock<IPromotionUnitOfWork>().Object; })
                .ResolveAnInstancePerRequest()
                .End();

            container.ForType<IPromotionRepository>()
                .Register<IPromotionRepository>((c, p) => { return repository; })
                .ResolveAnInstancePerScope()
                .End();
        }

        [Serializable]
        private class MockPromotion : IPromotion
        {
            public IEnumerable<string> AssociatedPropertyNames
            {
                get { throw new NotImplementedException(); }
            }

            public string Description { get; set; }

            public DateTime? EndDate
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int PromotionID { get; set; }

            public string PromotionKind
            {
                get { throw new NotImplementedException(); }
            }

            public IDictionary<string, IPromotionQualificationExtension> PromotionQualifications
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IDictionary<string, IPromotionReward> PromotionRewards
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int PromotionStatusTypeID { get; set; }

            public DateTime? StartDate
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool ValidFor<TProperty>(string propertyName, TProperty value)
            {
                throw new NotImplementedException();
            }
        }

        private class MockPromotionRepository : Mock<IPromotionRepository>
        {
            public int StoreCount { get; set;}
            public int RetrieveCount { get; set; }
            public List<IPromotion> PromotionList { get; set; }

            public MockPromotionRepository()
            {
                PromotionList = new List<IPromotion>();

                this.Setup(x => x.InsertPromotion(It.IsAny<IPromotion>(), It.IsAny<IPromotionUnitOfWork>()))
                    .Returns((IPromotion promo, IPromotionUnitOfWork unitOfWork) =>
                        {
                            while (promo.PromotionID == 0 || PromotionList.Any(x => x.PromotionID == promo.PromotionID))
                            {
                                promo.PromotionID = new Random().Next();
                            }
                            PromotionList.Add(promo);
                            StoreCount++;
                            return promo;
                        });

                this.Setup(x => x.RetrievePromotion(It.IsAny<int>(), It.IsAny<IPromotionUnitOfWork>()))
                    .Returns((int promotionID, IPromotionUnitOfWork unitOfWork) =>
                        {
                            RetrieveCount++;
                            return PromotionList.SingleOrDefault(x => x.PromotionID == promotionID);
                        });

                this.Setup(x => x.RetrievePromotions(It.IsAny<IPromotionUnitOfWork>(), It.IsAny<PromotionStatus>(), It.IsAny<Predicate<IPromotion>>(), It.IsAny<IEnumerable<string>>()))
                    .Returns((IPromotionUnitOfWork unitOfWork, PromotionStatus promotionStatus, Predicate<IPromotion> filter, IEnumerable<string> ofKinds) =>
                        {
                            RetrieveCount++;
                            return PromotionList;
                        });

                this.Setup(x => x.UpdateExistingPromotion(It.IsAny<IPromotion>(), It.IsAny<IPromotionUnitOfWork>()))
                    .Returns((IPromotion promotion, IPromotionUnitOfWork unitOfWork) =>
                        {
                            if (PromotionList.Any(x => x.PromotionID == promotion.PromotionID))
                            {
                                StoreCount++;
                                PromotionList.Remove(PromotionList.Single(x => x.PromotionID == promotion.PromotionID));
                                PromotionList.Add(promotion);
                                return promotion;
                            }
                            else
                            {
                                return promotion;
                            }
                        });
            }
        }

        private class MockPromotionUnitOfWork : Mock<IPromotionUnitOfWork>
        {
        }

        [TestMethod]
        public void CachingPromotionDataProvider_should_add_a_new_promotion()
        {
            using (var container = Create.NewContainer())
            {
                var repository = new MockPromotionRepository();

                MockWireup(container, repository.Object);

                var provider = new CachingPromotionDataProvider(repository.Object);

                var inserted = Add(provider);
                var saved = Retrieve(provider, inserted.PromotionID);
                saved = Retrieve(provider, inserted.PromotionID);
                Assert.IsNotNull(saved);
                Assert.AreNotEqual(0, saved.PromotionID);
                Assert.AreEqual((int)PromotionStatus.Enabled, saved.PromotionStatusTypeID);

                Assert.AreEqual(1, repository.StoreCount);
                Assert.AreEqual(0, repository.RetrieveCount);
            }
        }

		[TestMethod]
		public void CachingPromotionDataProvider_should_retrieve_promotion_in_persistence_layer()
		{
			using (var container = Create.NewContainer())
			{
                var repository = new MockPromotionRepository();

                MockWireup(container, repository.Object);

                var provider = new CachingPromotionDataProvider(repository.Object);

                var bypassed = InsertDirectly(repository.Object);
                var bypassedFound = Retrieve(provider, bypassed.PromotionID);
                var bypassedFound2 = Retrieve(provider, bypassed.PromotionID);
                var bypassedFound3 = Retrieve(provider, bypassed.PromotionID);
				Assert.AreEqual(bypassed.PromotionID, bypassedFound.PromotionID);

				Assert.AreEqual(1, repository.StoreCount);
				Assert.AreEqual(1, repository.RetrieveCount);
			}
		}

		[TestMethod]
		public void CachingPromotionDataProvider_should_update_promotion()
		{
			using (var container = Create.NewContainer())
			{
                var repository = new MockPromotionRepository();

                MockWireup(container, repository.Object);

                var provider = new CachingPromotionDataProvider(repository.Object);

                var inserted = Add(provider);
                var saved = Retrieve(provider, inserted.PromotionID);
				Assert.IsNotNull(saved);
				Assert.AreNotEqual(0, saved.PromotionID);
				Assert.AreEqual((int)PromotionStatus.Enabled, saved.PromotionStatusTypeID);
				UpdatePromotion(provider, saved.PromotionID);
                var found = Retrieve(provider, saved.PromotionID);
				Assert.AreEqual((int)PromotionStatus.Disabled, found.PromotionStatusTypeID);
				
				Assert.AreEqual(2, repository.StoreCount);
				Assert.AreEqual(0, repository.RetrieveCount);
			}
		}

		private IPromotion Add(CachingPromotionDataProvider provider)
		{
            var promotion = new MockPromotion();
            promotion.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
            promotion.Description="Test";
			return provider.AddPromotion(promotion, new MockPromotionUnitOfWork().Object);
		}

		private IPromotion Retrieve(IPromotionDataProvider provider, int promotionID)
		{
			return provider.FindPromotion(promotionID, new MockPromotionUnitOfWork().Object);
		}

		private void UpdatePromotion(IPromotionDataProvider provider, int promotionID)
		{
			var promotion = new MockPromotion();
            promotion.PromotionID = promotionID;
            promotion.PromotionStatusTypeID = (int)PromotionStatus.Disabled;
            promotion.Description = "Test";
            provider.UpdatePromotion(promotion, new MockPromotionUnitOfWork().Object);
		}

		private IPromotion InsertDirectly(IPromotionRepository repository)
		{
			var promotion = new MockPromotion();
            promotion.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
            promotion.Description = "Test";
            return repository.InsertPromotion(promotion, new MockPromotionUnitOfWork().Object);
		}
	}
}
