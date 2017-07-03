using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Common.Cache;


namespace NetSteps.Promotions.Service
{
	public class PromotionService : IPromotionService
	{
        private readonly IPromotionValidator _promotionValidator;
        private readonly IPromotionDataProvider _promotionDataProvider;
        private readonly IPromotionOrderAdjustmentRepository _promotionOrderAdjustmentRepository;
        private readonly IPromotionRewardHandlerManager _promotionRewardHandlerManager;
        private readonly IDataObjectExtensionProviderRegistry _extensionProviderRegistry;
        private readonly IPromotionKindManager _promotionKindManager;
        private readonly IPromotionOrderContextQualifier _orderContextQualifier;
        private readonly Func<IPromotionUnitOfWork> _unitOfWorkConstructor;

        public PromotionService(
            IPromotionValidator promotionValidator,
            IPromotionDataProvider promotionDataProvider,
            IPromotionOrderAdjustmentRepository promotionOrderAdjustmentRepository,
            IPromotionRewardHandlerManager promotionRewardHandlerManager,
            IDataObjectExtensionProviderRegistry extensionProviderRegistry,
            IPromotionKindManager promotionKindManager,
            IPromotionOrderContextQualifier orderContextQualifier,
            Func<IPromotionUnitOfWork> unitOfWorkConstructor)
        {
            _promotionValidator = promotionValidator;
            _promotionDataProvider = promotionDataProvider;
            _promotionOrderAdjustmentRepository = promotionOrderAdjustmentRepository;
            _promotionRewardHandlerManager = promotionRewardHandlerManager;
            _extensionProviderRegistry = extensionProviderRegistry;
            _promotionKindManager = promotionKindManager;
            _orderContextQualifier = orderContextQualifier;
            _unitOfWorkConstructor = unitOfWorkConstructor;
        }

		public IPromotion AddPromotion(IPromotion promotion, out IPromotionState promotionState)
		{
			promotionState = _promotionValidator.CheckValidity(promotion);

			if (promotionState.IsValid)
			{
				#region CodeContracts
				Contract.Assert(promotion != null);
				if (promotion.PromotionQualifications != null)
				{
					foreach (var qual in promotion.PromotionQualifications.Values)
					{
						Contract.Assert(qual != null);
					}
				}
				if (promotion.PromotionRewards != null)
				{
					foreach (var rew in promotion.PromotionRewards.Values)
					{
						foreach (var effect in rew.Effects.Values)
						{
							Contract.Assert(effect != null);
							Contract.Assert(effect.Extension != null);
							Contract.Assert(effect.ExtensionProviderKey != null);
						}
					}
				}
				#endregion


				using (var unitOfWork = _unitOfWorkConstructor())
				{
                    promotion = _promotionDataProvider.AddPromotion(promotion, unitOfWork);
					unitOfWork.SaveChanges();

				}
			}
			return promotion;
		}

		public IPromotion GetPromotion(int promotionID)
		{
			Contract.Assert(promotionID > 0);

			using (var unitOfWork = _unitOfWorkConstructor())
			{
                return _promotionDataProvider.FindPromotion(promotionID, unitOfWork);
			}
		}

		public IPromotion GetPromotion(Predicate<IPromotion> filter)
		{
			Contract.Assert(filter != null);
			return this.GetPromotions(filter).FirstOrDefault();
		}

		public bool IsInstanceOfPromotion(IOrderAdjustment adjustment, IPromotion promotion)
		{
			Contract.Assert(adjustment != null);
			Contract.Assert(promotion != null);
			Contract.Assert(adjustment.Extension != null);
			Contract.Assert(adjustment.Extension is IPromotionOrderAdjustment);

			return ((IPromotionOrderAdjustment)adjustment.Extension).PromotionID == promotion.PromotionID;

		}

		public bool IsInstanceOfPromotion(IOrderAdjustmentProfile profile, IPromotion promotion)
		{
			Contract.Assert(profile != null);
			Contract.Assert(profile is IPromotionOrderAdjustmentProfile);
			Contract.Assert(promotion != null);
			return ((IPromotionOrderAdjustmentProfile)profile).PromotionID == promotion.PromotionID;
		}

		public void DeleteDataObjectExtension(IExtensibleDataObject extensibleEntity)
		{
			Contract.Assert(extensibleEntity is IOrderAdjustment);

			IOrderAdjustment adjustment = extensibleEntity as IOrderAdjustment;

			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
				_promotionOrderAdjustmentRepository.DeletePromotionOrderAdjustment(adjustment.OrderAdjustmentID, unitOfWork);
				unitOfWork.SaveChanges();
			}
		}

		public IEnumerable<IPromotion> GetPromotions(PromotionStatus promotionStatuses, Predicate<IPromotion> filter)
		{
			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
                return _promotionDataProvider.FindPromotions(unitOfWork, promotionStatuses, filter);
			}
		}

		public int GetPromotionIDForPromotionQualificationID(int promotionQualificationID)
		{
			using (IPromotionUnitOfWork unitOfWork = _unitOfWorkConstructor())
			{
                return _promotionDataProvider.FindPromotionIDByPromotionQualificationID(unitOfWork, promotionQualificationID);
			}
		}

		public IPromotion UpdatePromotion(IPromotion promotion, out IPromotionState promotionState)
		{
			Contract.Assert(promotion != null);
			Contract.Assert(promotion.PromotionID != 0);

			// if the promotion's data (i.e. start/end date, status changes, etc) we don't have to create a new one -
			// but if the qualifications/rewards are modified, we must archive and create a new one.

			promotionState = _promotionValidator.CheckValidity(promotion);

			if (promotionState.IsValid)
			{
				using (var unitOfWork = _unitOfWorkConstructor())
				{
                    var existingPromotion = _promotionDataProvider.FindPromotion(promotion.PromotionID, unitOfWork);
					
					bool mustCreateSuccessor = false;
					// quick count check, simplest way to verify succession/modification
					if (promotion.PromotionQualifications.Count != existingPromotion.PromotionQualifications.Count || promotion.PromotionRewards.Count != existingPromotion.PromotionRewards.Count)
					{
						mustCreateSuccessor = true;
					}
					else
					{
						#region Detect qualification changes

						foreach (var qualification in promotion.PromotionQualifications)
						{

                            if (qualification.Value.ExtensionProviderKey == "48c4d3a9-ee29-435a-aa92-9759d5acc218") continue;
							var targetKey = qualification.Key;
							var matchExists = existingPromotion.PromotionQualifications.ContainsKey(targetKey);
							if (!matchExists)
							{
								mustCreateSuccessor = true;
								break;
							}
							var foundMatch = existingPromotion.PromotionQualifications.SingleOrDefault(x => x.Key == targetKey).Value;

							if (foundMatch == null || !qualification.Value.ExtensionProviderKey.Equals(foundMatch.ExtensionProviderKey))
								mustCreateSuccessor = true;
							else
							{
                                var handler = _extensionProviderRegistry.RetrieveExtensionProviderForRegisteredProvidedType<IPromotionQualificationHandler>(qualification.Value.GetType().ToString());
								if (!handler.AreEqual(qualification.Value, foundMatch))
								{
									mustCreateSuccessor = true;
									break;
								}
							}
						}

						#endregion

						#region Detect reward changes
						if (!mustCreateSuccessor)
						{
							foreach (var reward in promotion.PromotionRewards)
							{
								var targetKey = reward.Key;
								var matchExists = existingPromotion.PromotionRewards.ContainsKey(targetKey);
								if (!matchExists)
								{
									mustCreateSuccessor = true;
									break;
								}
								var foundMatch = existingPromotion.PromotionRewards.SingleOrDefault(x => x.Key == targetKey).Value;
								if (foundMatch == null)
								{
									mustCreateSuccessor = true;
									break;
								}
								else
								{
									if (!reward.Value.PromotionRewardKind.Equals(foundMatch.PromotionRewardKind))
									{
										mustCreateSuccessor = true;
										break;
									}
									else
									{
                                        var handler = _promotionRewardHandlerManager.GetRewardHandler(reward.Value.PromotionRewardKind);
										if (!handler.AreEqual(reward.Value, foundMatch))
										{
											mustCreateSuccessor = true;
											break;
										}
									}
								}
							}
						}
						#endregion
					}
					if (mustCreateSuccessor)
					{
						var oldStatus = existingPromotion.PromotionStatusTypeID;
						existingPromotion.PromotionStatusTypeID = (int)PromotionStatus.Obsolete;
                        _promotionDataProvider.UpdatePromotion(existingPromotion, unitOfWork);
						promotion.PromotionID = 0;
						promotion.PromotionStatusTypeID = oldStatus;
                        var saved = _promotionDataProvider.AddPromotion(promotion, unitOfWork);
						unitOfWork.SaveChanges();
						return saved;
					}
					else
					{
                        var saved = _promotionDataProvider.UpdatePromotion(promotion, unitOfWork);
						unitOfWork.SaveChanges();
						return saved;
					}
				}
			}
			return promotion;
		}

		public bool IsInstanceOfProfile(IOrderAdjustment adjustment, IOrderAdjustmentProfile adjustmentProfile)
		{
			Contract.Assert(adjustment != null);
			Contract.Assert(adjustment.Extension != null);
			Contract.Assert(adjustment.Extension is IPromotionOrderAdjustment);
			Contract.Assert(adjustmentProfile != null);

			return ((IPromotionOrderAdjustment)adjustment.Extension).PromotionID == ((IPromotionOrderAdjustmentProfile)adjustmentProfile).PromotionID;
		}

		public TPromotion GetPromotion<TPromotion>(int promotionID) where TPromotion : IPromotion
		{
			Contract.Assert(promotionID > 0);

			using (var unitOfWork = _unitOfWorkConstructor())
			{
                var promotion = _promotionDataProvider.FindPromotion(promotionID, unitOfWork);
				Contract.Assert(typeof(TPromotion).IsAssignableFrom(promotion.GetType()));
				return (TPromotion)(object)promotion;
			}
		}

		public TPromotion GetPromotion<TPromotion>(Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			Contract.Assert(filter != null);
			return this.GetPromotions<TPromotion>(filter).FirstOrDefault();
		}

		public IEnumerable<TPromotion> GetPromotions<TPromotion>(PromotionStatus promotionStatuses, Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			var registeredPromotionKinds = _promotionKindManager.GetPromotionKindStrings<TPromotion>();
			using (var unitOfWork = _unitOfWorkConstructor())
			{
				return _promotionDataProvider.FindPromotions(unitOfWork, promotionStatuses, registeredPromotionKinds).Cast<TPromotion>().Where(x => filter(x));
			}
		}

		public IEnumerable<TPromotion> GetQualifiedPromotions<TPromotion>(Data.Common.Context.IOrderContext orderContext, Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			var registeredPromotionKinds = _promotionKindManager.GetPromotionKindStrings<TPromotion>();
			using (var unitofWork = _unitOfWorkConstructor())
			{
                var promotions = _promotionDataProvider.FindPromotions(unitofWork, PromotionStatus.Enabled, registeredPromotionKinds).Cast<TPromotion>().Where(x => filter(x)).ToList();
				
				return promotions.Where(promotion => _orderContextQualifier.GetQualificationResult(promotion, orderContext));
			}
		}
	}
}
