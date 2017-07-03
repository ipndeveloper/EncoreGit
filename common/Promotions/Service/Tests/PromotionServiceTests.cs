using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Extensibility.Core;
using System.Diagnostics;
using NetSteps.Data.Common.Services;
using Moq;
using NetSteps.Data.Common;
using NetSteps.Promotions.Common.Cache;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Context;

namespace NetSteps.Promotions.Service.Tests
{
	[TestClass]
	public class PromotionServiceTests
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
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

        private T GetMockPromotion<T>(PromotionStatus status, string kind = null) where T : class, IPromotion
        {
            var promotionMock = new Mock<T>();
            promotionMock.SetupAllProperties();
            promotionMock.Object.PromotionStatusTypeID = (int)status;
            promotionMock.SetupGet(x => x.PromotionKind).Returns(kind);
            var rewards = new Dictionary<string, IPromotionReward>();
            promotionMock.SetupGet(x => x.PromotionRewards).Returns(rewards);
            var qualifications = new Dictionary<string, IPromotionQualificationExtension>();
            promotionMock.SetupGet(x => x.PromotionQualifications).Returns(qualifications);
            return promotionMock.Object;
        }

        public interface IMockPromotion : IPromotion
        {
        }

        public interface ISpecificMockPromotion : IMockPromotion { }

        public class FunctionalMockPromotionUnitOfWork : Mock<IPromotionUnitOfWork>
        {
        }

        public class FunctionalMockPromotionValidator : Mock<IPromotionValidator> 
        {
            public FunctionalMockPromotionValidator(IPromotionState expectedState)
            {
                if (expectedState == null)
                {
                    var promotionStateMock = new Mock<IPromotionState>();
                    promotionStateMock.SetupAllProperties();
                    promotionStateMock.SetupGet(x => x.IsValid).Returns(true);
                    this.Setup(x => x.CheckValidity(It.IsAny<IPromotion>())).Returns(promotionStateMock.Object);
                }
                else
                {
                    this.Setup(x => x.CheckValidity(It.IsAny<IPromotion>())).Returns(expectedState);
                }
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
                        while (promo.PromotionID == 0 || promotionList.Any(x => x.PromotionID == promo.PromotionID))
                        {
                            promo.PromotionID = new Random().Next();
                        }
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

        public class FunctionalMockPromotionOrderAdjustmentRepository : Mock<IPromotionOrderAdjustmentRepository>
        {
        }

        public class FunctionalMockPromotionKindManager : Mock<IPromotionKindManager> 
        {
            public FunctionalMockPromotionKindManager()
            {
                this.Setup(x => x.GetPromotionKindStrings<IPromotion>())
                    .Returns(() => { return new string[] { "Type1", "Type2", "Type3" }; });
                this.Setup(x => x.GetPromotionKindStrings<IMockPromotion>())
                    .Returns(() => { return new string[] { "Type2", "Type3" }; });
                this.Setup(x => x.GetPromotionKindStrings<ISpecificMockPromotion>())
                    .Returns(() => { return new string[] { "Type3" }; });
            }
        }

        public class FunctionalMockRewardHandlerManager : Mock<IPromotionRewardHandlerManager>
        {
            public FunctionalMockRewardHandlerManager()
            {
                var mockHandler = new Mock<IPromotionRewardHandler>();
                this.Setup(x => x.GetRewardHandler(It.IsAny<string>())).Returns(mockHandler.Object);
            }
        }

        public class FunctionalMockExtensionProviderRegistry : Mock<IDataObjectExtensionProviderRegistry>
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

		[TestMethod]
		public void PromotionService_should_retrieve_promotions()
		{
			using (var container = Create.NewContainer())
			{
                var context = GetMockOrderContext();

                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var promotion = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled), null);

                var promotions = promotionService.GetPromotions().ToList();
				Assert.AreEqual(1, promotions.Count);
				Assert.AreEqual(promotion.PromotionID, promotions[0].PromotionID);
			}
		}

		[TestMethod]
		public void PromotionService_should_retrieve_archived_promotions_if_statuses_are_requested()
		{
			using (var container = Create.NewContainer())
			{
                var context = GetMockOrderContext();

                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var enabled = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled), null);

                var archived = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Archived), null);

				var promotions = promotionService.GetPromotions(PromotionStatus.Archived).ToList();
				Assert.AreEqual(1, promotions.Count);
                Assert.AreEqual(archived.PromotionID, promotions[0].PromotionID);
			}
		}

		[TestMethod]
		public void PromotionService_should_retrieve_disabled_promotions_if_statuses_are_requested()
		{
			using (var container = Create.NewContainer())
			{

                var context = GetMockOrderContext();
                
                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var disabled = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Disabled), null);

                var archived = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Archived), null);

                var promotions = promotionService.GetPromotions(PromotionStatus.Archived).ToList();
				Assert.AreEqual(1, promotions.Count);
                Assert.AreEqual(archived.PromotionID, promotions[0].PromotionID);
			}
		}

		[TestMethod]
		public void PromotionService_should_retrieve_obsolete_promotions_if_statuses_are_requested()
		{
			using (var container = Create.NewContainer())
			{
                var context = GetMockOrderContext();

                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var obsolete = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Obsolete), null);

                var archived = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Archived), null);

				var promotions = promotionService.GetPromotions(PromotionStatus.Archived).ToList();
				Assert.AreEqual(1, promotions.Count);
                Assert.AreEqual(archived.PromotionID, promotions[0].PromotionID);
			}
		}

		[TestMethod]
		public void PromotionService_should_retrieve_promotions_with_more_than_one_status_type_requested()
		{
			using (var container = Create.NewContainer())
			{
                var context = GetMockOrderContext();

                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var obsolete = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Obsolete), null);

                var archived = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Archived), null);
                Assert.AreNotEqual(obsolete.PromotionID, archived.PromotionID);

                var disabled = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Disabled), null);
                Assert.AreNotEqual(disabled.PromotionID, archived.PromotionID);
                Assert.AreNotEqual(disabled.PromotionID, obsolete.PromotionID);

                var promotions = promotionService.GetPromotions(PromotionStatus.Archived | PromotionStatus.Obsolete).ToList();
				Assert.AreEqual(2, promotions.Count);
                Assert.IsTrue(promotions[0].PromotionID == archived.PromotionID || promotions[0].PromotionID == obsolete.PromotionID);
                Assert.IsTrue(promotions[1].PromotionID == archived.PromotionID || promotions[1].PromotionID == obsolete.PromotionID);
                Assert.AreNotEqual(promotions[0], promotions[1]);
				Assert.AreNotEqual(promotions[0].PromotionID, promotions[1].PromotionID);
			}
		}

		[TestMethod]
		public void PromotionService_should_determine_if_an_orderadjustment_is_an_instance_of_a_specific_promotion()
		{
			using (var container = Create.NewContainer())
			{
                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var promotion = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled), null);

                var orderAdjMock = new Mock<IOrderAdjustment>().SetupAllProperties();
				var promotionAdjMock = new Mock<IPromotionOrderAdjustment>().SetupAllProperties();
                orderAdjMock.Object.Extension = promotionAdjMock.Object;
                promotionAdjMock.Object.PromotionID = promotion.PromotionID;

				Assert.IsTrue(promotionService.IsInstanceOfPromotion(orderAdjMock.Object, promotion));
			}
		}

        [TestMethod]
        public void PromotionService_should_query_by_promotion_type_with_inheritance()
        {
            using (var container = Create.NewContainer())
            {

                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var simplePromotion1 = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled, "Type1"), null);

                var simplePromotion2 = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled, "Type1"), null);

                var simplePromotion3 = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled, "Type1"), null);

                var mockPromotion1 = mockDataProvider.Object.AddPromotion(GetMockPromotion<IMockPromotion>(PromotionStatus.Enabled, "Type2"), null);

                var mockPromotion2 = mockDataProvider.Object.AddPromotion(GetMockPromotion<IMockPromotion>(PromotionStatus.Enabled, "Type2"), null);

                var mockSpecificPromotion1 = mockDataProvider.Object.AddPromotion(GetMockPromotion<ISpecificMockPromotion>(PromotionStatus.Enabled, "Type3"), null);

                var specificPromotions = promotionService.GetPromotions<ISpecificMockPromotion>();
                Assert.IsNotNull(specificPromotions);
                Assert.AreEqual(1, specificPromotions.Count());

                var regularPromotions = promotionService.GetPromotions<IMockPromotion>();
                Assert.IsNotNull(regularPromotions);
                Assert.AreEqual(3, regularPromotions.Count());

                var genericPromotions = promotionService.GetPromotions<IPromotion>();
                Assert.IsNotNull(genericPromotions);
                Assert.AreEqual(6, genericPromotions.Count());
            }

        }

        [TestMethod]
        public void PromotionService_should_return_promotions_for_qualified_order_context_without_application()
        {
            using (var container = Create.NewContainer())
            {
                var context = GetMockOrderContext();
                
                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(null);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var enabled = mockDataProvider.Object.AddPromotion(GetMockPromotion<IPromotion>(PromotionStatus.Enabled, "Type1"), null);

                var promos = promotionService.GetQualifiedPromotions<IPromotion>(context, x => { return true; });
                Assert.IsNotNull(promos);
                Assert.AreNotEqual(0, promos.Count());
            }
        }

        [TestMethod]
        public void PromotionService_should_not_save_invalid_promotions()
        {
            using (var container = Create.NewContainer())
            {
                #region Construct Service with mocks
                var mockDataProvider = new FunctionalMockDataProvider();
                var mockExtensionProviderRegistry = new FunctionalMockExtensionProviderRegistry();
                var mockRewardHandlerManager = new FunctionalMockRewardHandlerManager();
                var mockOrderAdjustmentRepository = new FunctionalMockPromotionOrderAdjustmentRepository();
                var mockOrderContextQualifier = new FunctionalMockOrderContextQualifier(PromotionQualificationResult.MatchForAllCustomers);
                var promotionStateMock = new Mock<IPromotionState>().SetupAllProperties();
                promotionStateMock.SetupGet(x => x.IsValid).Returns(false);
                var mockPromotionValidator = new FunctionalMockPromotionValidator(promotionStateMock.Object);
                var mockPromotionKindManager = new FunctionalMockPromotionKindManager();

                var promotionService = new PromotionService
                        (
                            mockPromotionValidator.Object,
                            mockDataProvider.Object,
                            mockOrderAdjustmentRepository.Object,
                            mockRewardHandlerManager.Object,
                            mockExtensionProviderRegistry.Object,
                            mockPromotionKindManager.Object,
                            mockOrderContextQualifier.Object,
                            () => { return new FunctionalMockPromotionUnitOfWork().Object; }
                        );
                #endregion

                var promo = GetMockPromotion<IPromotion>(PromotionStatus.Enabled, "Type1");
                var qualificationMock = new Mock<IPromotionQualificationExtension>();
                qualificationMock.SetupAllProperties();
                promo.PromotionQualifications.Add("nulltest", qualificationMock.Object);
                var rewardMock = new Mock<IPromotionReward>();
                rewardMock.SetupAllProperties();
                var effectDictionary = new Dictionary<string, IPromotionRewardEffect>();
                rewardMock.SetupGet(x => x.Effects).Returns(effectDictionary);
                var effectMock = new Mock<IPromotionRewardEffect>();
                rewardMock.Object.Effects.Add("effect",effectMock.Object);

                var state = Create.New<IPromotionState>();
                var saved = promotionService.AddPromotion(promo, out state);

                Assert.IsFalse(state.IsValid);
                Assert.IsTrue(saved.PromotionID == 0);
            }
        }
	}
}
