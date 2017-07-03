using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Common.Cache;

namespace NetSteps.Promotions.Service
{
	/// <summary>
	/// Service that locates and provides promotions services for orders. 
	/// </summary>
	public class PromotionProvider : IPromotionProvider
	{
		private readonly IPromotionService _promotionService;
		private readonly IDataObjectExtensionProviderRegistry _extensionProviderRegistry;
		private readonly IPromotionRewardHandlerManager _rewardHandlerManager;
		private readonly IPromotionDataProvider _dataProvider;
		private readonly IPromotionOrderAdjustmentRepository _orderAdjustmentRepository;
		private readonly IPromotionOrderContextQualifier _orderContextQualifier;
		private readonly Func<IPromotionUnitOfWork> _unitOfWorkConstructor;
		private readonly Func<IPromotionOrderAdjustment> _orderAdjustmentConstructor;
		private readonly Func<IPromotionOrderAdjustmentProfile> _orderAdjustmentProfileConstructor;



		public PromotionProvider(IPromotionService promotionService, IDataObjectExtensionProviderRegistry extensionProviderRegistry, IPromotionRewardHandlerManager rewardHandlerManager, IPromotionDataProvider dataProvider, IPromotionOrderAdjustmentRepository orderAdjustmentRepository, IPromotionOrderContextQualifier orderContextQualifier, Func<IPromotionUnitOfWork> unitOfWorkConstructor, Func<IPromotionOrderAdjustment> orderAdjustmentConstructor, Func<IPromotionOrderAdjustmentProfile> orderAdjustmentProfileConstructor)
		{
			_promotionService = promotionService;
			_extensionProviderRegistry = extensionProviderRegistry;
			_rewardHandlerManager = rewardHandlerManager;
			_dataProvider = dataProvider;
			_orderAdjustmentRepository = orderAdjustmentRepository;
			_orderContextQualifier = orderContextQualifier;
			_unitOfWorkConstructor = unitOfWorkConstructor;
			_orderAdjustmentConstructor = orderAdjustmentConstructor;
			_orderAdjustmentProfileConstructor = orderAdjustmentProfileConstructor;
		}

		public const string ProviderKey = "78121e1d-94d4-426f-b0be-0e6253cbe3d9";
		/// <summary>
		/// Gets the provider key.
		/// </summary>
		public string GetProviderKey() { return ProviderKey; }

		public void CommitAdjustment(IOrderAdjustment adjustment, IOrderContext orderContext)
		{
			Contract.Assert(adjustment != null);
			Contract.Assert(adjustment.Extension != null);
			Contract.Assert(adjustment.Extension is IPromotionOrderAdjustment);
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments.Any(x => x.OrderAdjustmentID == adjustment.OrderAdjustmentID));

			IPromotion promotion = _promotionService.GetPromotion((adjustment.Extension as IPromotionOrderAdjustment).PromotionID);

			foreach (var qualification in promotion.PromotionQualifications.Values)
			{
				var handler = _extensionProviderRegistry.RetrieveExtensionProvider<IPromotionQualificationHandler>(qualification.ExtensionProviderKey);
				if (handler.RequiresCommitNotification)
				{
					handler.Commit(promotion, qualification, orderContext);
				}
			}
			foreach (var reward in promotion.PromotionRewards.Values)
			{
				var handler = _rewardHandlerManager.GetRewardHandler(reward.PromotionRewardKind);
				if (handler.RequiresCommitNotification)
				{
					handler.Commit(reward, orderContext);
				}
			}

		}

		public IDataObjectExtension CreateOrderAdjustmentDataObjectExtension(IOrderAdjustmentProfile profile)
		{
			IPromotionOrderAdjustmentProfile Promotion = (IPromotionOrderAdjustmentProfile)profile;
			IPromotionOrderAdjustment adjustment = _orderAdjustmentConstructor();
			adjustment.PromotionID = Promotion.PromotionID;

			return adjustment;
		}

		public IEnumerable<IOrderAdjustmentProfile> GetApplicableAdjustments(IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);

			var orderAdjustments = new List<IOrderAdjustmentProfile>();
			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{

				Predicate<IPromotion> activeNow = p => (!p.StartDate.HasValue || p.StartDate < DateTime.Now) && (!p.EndDate.HasValue || p.EndDate > DateTime.Now);
				var Promotions = _dataProvider.FindPromotions(unitOfWork, activeNow).ToList();
				foreach (var profile in Promotions)
				{
					var qualified = _orderContextQualifier.GetQualificationResult(profile, orderContext);
					if (qualified)
						orderAdjustments.Add(CreateOrderAdjustmentProfiles(orderContext, profile, qualified));
				}
			}
			return orderAdjustments;
		}

		private IOrderAdjustmentProfile CreateOrderAdjustmentProfiles(IOrderContext orderContext, IPromotion profile, PromotionQualificationResult matchResult)
		{
			var adjustmentProfile = _orderAdjustmentProfileConstructor();
			adjustmentProfile.ExtensionProviderKey = ProviderKey;
			adjustmentProfile.Description = profile.Description;
			adjustmentProfile.PromotionID = profile.PromotionID;

			foreach (var reward in profile.PromotionRewards.Values)
			{
				IPromotionRewardHandler handler = _rewardHandlerManager.GetRewardHandler(reward.PromotionRewardKind);
				handler.AddRewardToAdjustmentProfile(orderContext, adjustmentProfile, reward, matchResult);
			}

			return adjustmentProfile;
		}

		public IOrderAdjustmentProfile GetOrderAdjustmentProfile(IOrderContext orderContext, int adjustmentID)
		{
			using (var unitOfWork = _unitOfWorkConstructor())
			{
				var promoOrderAdjustmentRepository = _orderAdjustmentRepository;
				var promotionOrderAdjustment = promoOrderAdjustmentRepository.FindPromotionOrderAdjustment(adjustmentID, unitOfWork);
				var promotion = _dataProvider.FindPromotion(promotionOrderAdjustment.PromotionID, unitOfWork);

				var adjustmentProfile = _orderAdjustmentProfileConstructor();
				adjustmentProfile.ExtensionProviderKey = ProviderKey;
				adjustmentProfile.Description = promotion.Description;
				adjustmentProfile.PromotionID = promotion.PromotionID;

				var result = PromotionQualificationResult.MatchForAllCustomers;
				foreach (var qualification in promotion.PromotionQualifications.Values)
				{
					Contract.Assert(typeof(IPromotionQualificationExtension).IsAssignableFrom(qualification.GetType()));
					var handler = _extensionProviderRegistry.RetrieveExtensionProviderForRegisteredProvidedType<IPromotionQualificationHandler>(qualification.GetType().ToString());
					result.BitwiseAnd(handler.Matches(promotion, qualification, orderContext));
				}

				if (result.Status != PromotionQualificationResult.MatchType.NoMatch)
				{
					foreach (var reward in promotion.PromotionRewards.Values)
					{
						Contract.Assert(reward.Effects.Count == reward.OrderOfApplication.Count(), "Reward order of application count and effect count must match.");
						foreach (var key in reward.OrderOfApplication)
						{
							Contract.Assert(reward.Effects.ContainsKey(key), String.Format("Reward order of application contains key {0} but that key is not present in the effects dictionary.", key));
							Contract.Assert(typeof(IPromotionRewardEffectExtension).IsAssignableFrom(reward.Effects[key].Extension.GetType()));

							var handler = _rewardHandlerManager.GetRewardHandler(reward.Effects[key].ExtensionProviderKey);
							handler.AddRewardToAdjustmentProfile(orderContext, adjustmentProfile, reward, result);
						}
					}
				}
				return adjustmentProfile;
			}
		}

		public bool IsInstanceOfProfile(IOrderAdjustment adjustment, IOrderAdjustmentProfile adjustmentProfile)
		{
			Contract.Assert(typeof(IOrderAdjustmentProfile).IsAssignableFrom(adjustmentProfile.GetType()));
			Contract.Assert(typeof(IPromotionOrderAdjustmentProfile).IsAssignableFrom(adjustmentProfile.GetType()));
			var extension = GetDataObjectExtension(adjustment);
			Contract.Assert(typeof(IPromotionOrderAdjustment).IsAssignableFrom(extension.GetType()));
			var promotionExtension = extension as IPromotionOrderAdjustment;
			var promotionAdjustmentProfile = adjustmentProfile as IPromotionOrderAdjustmentProfile;
			return promotionExtension.PromotionID == promotionExtension.PromotionID;
		}

		public void NotifyOfRemoval(IOrderContext orderContext, IOrderAdjustment adjustment)
		{
			Contract.Assert(adjustment != null);
			Contract.Assert(adjustment.Extension != null);
			Contract.Assert(adjustment.Extension is IPromotionOrderAdjustment);
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments.Any(x => x.OrderAdjustmentID == adjustment.OrderAdjustmentID));

			IPromotion promotion = _promotionService.GetPromotion((adjustment.Extension as IPromotionOrderAdjustment).PromotionID);

			foreach (var qualification in promotion.PromotionQualifications.Values)
			{
				var handler = _extensionProviderRegistry.RetrieveExtensionProvider(qualification.ExtensionProviderKey) as IPromotionQualificationHandler;
				if (handler.RequiresRemoveNotification)
				{
					handler.Remove(promotion, qualification, orderContext);
				}
			}
			foreach (var reward in promotion.PromotionRewards.Values)
			{
				var handler = _rewardHandlerManager.GetRewardHandler(reward.PromotionRewardKind);
				if (handler.RequiresRemoveNotification)
				{
					handler.Remove(reward, orderContext);
				}
			}
		}

		public void DeleteDataObjectExtension(IExtensibleDataObject extensibleEntity)
		{
			Contract.Assert(extensibleEntity is IOrderAdjustment);

			IOrderAdjustment adjustment = extensibleEntity as IOrderAdjustment;

			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
				IPromotionOrderAdjustmentRepository repository = _orderAdjustmentRepository;

				repository.DeletePromotionOrderAdjustment(adjustment.OrderAdjustmentID, unitOfWork);

				unitOfWork.SaveChanges();
			}
		}

		public IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject extensibleEntity)
		{
			Contract.Assert(extensibleEntity is IOrderAdjustment);

			IOrderAdjustment adjustment = extensibleEntity as IOrderAdjustment;

			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
				IPromotionOrderAdjustmentRepository repository = _orderAdjustmentRepository;

				return repository.FindPromotionOrderAdjustment(adjustment.OrderAdjustmentID, unitOfWork);
			}
		}

		public IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject extensibleEntity)
		{
			Contract.Assert(extensibleEntity is IOrderAdjustment);
			Contract.Assert(extensibleEntity.Extension is IPromotionOrderAdjustment);

			IOrderAdjustment adjustment = extensibleEntity as IOrderAdjustment;
			IPromotionOrderAdjustment extension = adjustment.Extension as IPromotionOrderAdjustment;

			extension.OrderAdjustmentID = adjustment.OrderAdjustmentID;
			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
				IPromotionOrderAdjustmentRepository repository = _orderAdjustmentRepository;
				repository.SaveAdjustmentExtension(adjustment, unitOfWork);
				unitOfWork.SaveChanges();

				return extensibleEntity.Extension;
			}
		}

		public void UpdateDataObjectExtension(IExtensibleDataObject extensibleEntity)
		{
			Contract.Assert(extensibleEntity is IOrderAdjustment);
			Contract.Assert(extensibleEntity.Extension is IPromotionOrderAdjustment);

			IOrderAdjustment adjustment = extensibleEntity as IOrderAdjustment;
			IPromotionOrderAdjustment extension = adjustment.Extension as IPromotionOrderAdjustment;

			extension.OrderAdjustmentID = adjustment.OrderAdjustmentID;
			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
				var repository = _orderAdjustmentRepository;
				repository.SaveAdjustmentExtension(adjustment, unitOfWork);
				unitOfWork.SaveChanges();
			}
		}
	}
}
