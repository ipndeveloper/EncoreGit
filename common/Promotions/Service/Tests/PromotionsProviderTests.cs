using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Service.Repository;
using NetSteps.Promotions.Common.Cache;
using Moq;
using NetSteps.Data.Common;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Service.Tests
{
    [TestClass]
    public class PromotionsProviderTests
    {
        int _productID;

        [TestInitialize]
        public void Init()
        {
            _productID = new Random().Next();
            WireupCoordinator.SelfConfigure();
        }
        private void mockWireup(IContainer container)
        {
            container.ForType<IPromotionUnitOfWork>().Register<IPromotionUnitOfWork>((c, p) => { return new Mock<IPromotionUnitOfWork>().Object; }).ResolveAsSingleton().End();

            Container.Current.ForType<IPromotionDataProvider>().Register<IPromotionDataProvider>((c, p) => { return new FunctionalMockDataProvider().Object; }).ResolveAnInstancePerScope().End();
        }

        public class FunctionalMockPromotionUnitOfWork : Mock<IPromotionUnitOfWork>
        {
        }

        public class FunctionalMockPromotionOrderAdjustment : Mock<IPromotionOrderAdjustment>
        {
        }

        public class FunctionalMockPromotionOrderAdjustmentProfile : Mock<IPromotionOrderAdjustmentProfile>
        {
            
            public FunctionalMockPromotionOrderAdjustmentProfile()
            {
                this.SetupAllProperties();
            }
        }

        public class FunctionalMockDataProvider : Mock<IPromotionDataProvider>
        {
            public IList<IPromotion> promotionList = new List<IPromotion>();

            public FunctionalMockDataProvider()
            {
                this
                    .Setup(x => x.AddPromotion(It.IsAny<IPromotion>(), It.IsAny<IUnitOfWork>()))
                    .Returns((IPromotion promo, IUnitOfWork unitOfWork) =>
                    {
                       promo.PromotionID = promotionList.Count() + 1;
                       promotionList.Add(promo);
                       return promo;
                    });
                this
                    .Setup(x => x.FindPromotions(It.IsAny<IPromotionUnitOfWork>(), It.IsAny<PromotionStatus>(), It.IsAny<IPromotionInterval>(), It.IsAny<Predicate<IPromotion>>(), It.IsAny<IEnumerable<string>>()))
                    .Returns((IPromotionUnitOfWork unitOfWork, PromotionStatus promotionStatus, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds) =>
                    {
                        return promotionList.Where(promo => (
                                                                                    (((int)promo.PromotionStatusTypeID & (int)promotionStatus) > 0) &&
                                                                                    (!promo.StartDate.HasValue || !searchInterval.StartDate.HasValue || promo.StartDate.Value <= searchInterval.StartDate.Value) &&
                                                                                    (!promo.EndDate.HasValue || !searchInterval.EndDate.HasValue || promo.EndDate.Value <= searchInterval.EndDate.Value) &&
                                                                                    (!ofKinds.Any() || ofKinds.Contains(promo.PromotionKind)) &&
                                                                                    filter(promo)
                                                                                )
                                                                        );
                    });
                this
                    .Setup(x => x.FindPromotion(It.IsAny<int>(), It.IsAny<IUnitOfWork>()))
                    .Returns((int promotionID, IUnitOfWork unitOfWork) =>
                    {
                        return promotionList.SingleOrDefault(x => x.PromotionID == promotionID);
                    });

            }
        }

        public class FunctionalMockPromotionService : Mock<IPromotionService>
        {
        }

        public class FunctionalMockExtensionProviderRegistry : Mock<IDataObjectExtensionProviderRegistry>
        {
        }

        public class FunctionalMockRewardHandlerManager : Mock<IPromotionRewardHandlerManager>
        {
            public FunctionalMockRewardHandlerManager()
            {
                var mockHandler = new Mock<IPromotionRewardHandler>();
                this.Setup(x => x.GetRewardHandler(It.IsAny<string>())).Returns(mockHandler.Object);
            }
        }
        public class FunctionalMockPromotionOrderAdjustmentRepository : Mock<IPromotionOrderAdjustmentRepository>
        {
        }
        public class FunctionalMockOrderContextQualifier : Mock<IPromotionOrderContextQualifier>
        {
            public FunctionalMockOrderContextQualifier(PromotionQualificationResult expectedResult)
            {
                this.Setup(x => x.GetQualificationResult(It.IsAny<IPromotion>(), It.IsAny<IOrderContext>()))
                    .Returns(expectedResult);
                    
                   
            }
        }

        private IOrderContext GetMockOrderContext()
        {
            var customerList = new List<IOrderCustomer>();
            var orderContextMock = new Mock<IOrderContext>().SetupAllProperties();
            var orderMock = new Mock<IOrder>().SetupAllProperties();
            orderMock.SetupGet(x => x.OrderCustomers).Returns(customerList);
            var orderCustomerMock = new Mock<IOrderCustomer>().SetupAllProperties(); 
            orderContextMock.Object.Order = orderMock.Object;
            orderMock.Object.OrderCustomers.Add(orderCustomerMock.Object);
            return orderContextMock.Object;
        }

        [TestMethod]
        public void PromotionProvider_should_be_registered_in_EntityExtensionProviderRegistry()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);
                
                IDataObjectExtensionProvider provider = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(PromotionProvider.ProviderKey);
                Assert.IsNotNull(provider);
                Assert.IsInstanceOfType(provider, typeof(IPromotionProvider));
                Assert.IsInstanceOfType(provider, typeof(PromotionProvider));
            }
        }

        [TestMethod]
        public void PromotionProvider_should_retrieve_a_list_of_promotions_for_an_order()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);

                var context = GetMockOrderContext();

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => {return new FunctionalMockPromotionOrderAdjustment().Object; }, () => {return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var promotion = new Mock<IPromotion>();
                promotion.SetupAllProperties();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.Description = Guid.NewGuid().ToString();
                var rewardDictionary = new Dictionary<string, IPromotionReward>();
                promotion.SetupGet(x => x.PromotionRewards).Returns(rewardDictionary);
                
                mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
                Assert.IsNotNull(adjustments);
                Assert.AreNotEqual(0, adjustments.Count());
                var profile = adjustments.First() as IPromotionOrderAdjustmentProfile;

                IPromotion foundProfile = mockDataProvider.Object.FindPromotion(promotion.Object.PromotionID, null);
                Assert.IsNotNull(foundProfile);
                Assert.AreEqual(promotion.Object.PromotionID, foundProfile.PromotionID);
            }
        }

        [TestMethod]
        public void PromotionProvider_should_not_retrieve_disabled_promotions_for_an_order()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var context = GetMockOrderContext();

				var promotion = new Mock<IPromotion>().SetupAllProperties();
				promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Disabled;
                mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
                Assert.IsNotNull(adjustments);
                Assert.IsNull(adjustments.SingleOrDefault(x => x.Description.Equals(promotion.Object.Description)));
            }
        }

        [TestMethod]
        public void PromotionProvider_should_not_retrieve_obsolete_promotions_for_an_order()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);

                var context = GetMockOrderContext();

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var promotion = new Mock<IPromotion>().SetupAllProperties();
				promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Obsolete;
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.Description = Guid.NewGuid().ToString();
                mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
                Assert.IsNotNull(adjustments);
                Assert.IsNull(adjustments.SingleOrDefault(x => x.Description.Equals(promotion.Object.Description)));
            }
        }

        [TestMethod]
        public void PromotionProvider_should_not_retrieve_archived_promotions_for_an_order()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);

                var context = GetMockOrderContext();

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var promotion = new Mock<IPromotion>().SetupAllProperties();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Archived;
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.Description = Guid.NewGuid().ToString();
                mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
                Assert.IsNotNull(adjustments);
                Assert.IsNull(adjustments.SingleOrDefault(x => x.Description.Equals(promotion.Object.Description)));
            }
        }

        [TestMethod]
        public void PromotionProvider_should_return_a_providerkey()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(container);

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                string key = PromotionProvider.ProviderKey;
                Assert.IsFalse(String.IsNullOrEmpty(key));
                Assert.AreEqual(key, promotionProvider.GetProviderKey());
            }
        }

		[TestMethod]
		public void PromotionProvider_should_not_retrieve_expired_promotions_for_an_order()
		{
			using (var container = Create.NewContainer())
            {
                mockWireup(container);

                var context = GetMockOrderContext();

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var promotion = new Mock<IPromotion>().SetupAllProperties();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.Description = "mock promotion";
                promotion.Object.EndDate = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
                Assert.IsNotNull(adjustments);
                Assert.AreEqual(0, adjustments.Count());
            }
		}

		[TestMethod]
		public void PromotionProvider_should_not_retrieve_not_yet_active_promotions_for_an_order()
		{
			using (var container = Create.NewContainer())
			{
                mockWireup(container);

                var context = GetMockOrderContext();

                #region Construct Provider with mocks
                var mockPromotionService = new FunctionalMockPromotionService();
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);

                var promotionProvider = new PromotionProvider(mockPromotionService.Object, mockExtensionProviderRegistry.Object, mockRewardHandlerManager.Object, mockDataProvider.Object, mockOrderAdjustmentRepository.Object, mockOrderContextQualifier.Object, () => { return new FunctionalMockPromotionUnitOfWork().Object; }, () => { return new FunctionalMockPromotionOrderAdjustment().Object; }, () => { return new FunctionalMockPromotionOrderAdjustmentProfile().Object; });
                #endregion

                var promotion = new Mock<IPromotion>().SetupAllProperties();
                promotion.Object.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                promotion.Object.PromotionID = new Random().Next();
                promotion.Object.Description = "mock promotion";
                promotion.Object.StartDate = DateTime.Now.AddDays(1);
				mockDataProvider.Object.AddPromotion(promotion.Object, null);

                var adjustments = promotionProvider.GetApplicableAdjustments(context);
				Assert.IsNotNull(adjustments);
				Assert.AreEqual(0, adjustments.Count());
			}
		}
    }
}
