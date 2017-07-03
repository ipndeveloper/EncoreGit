using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class AddItemsRewardEffectHandler : BasePromotionRewardEffectExtensionHandler<IAddItemPromotionRewardEffect, IAddItemPromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardTargetingEffectExtensionHandler
	{
		public IEnumerable<IOrderAdjustmentProfileOrderItemTarget> CreateOrderLineModificationTargets(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionRewardEffect rewardEffect, PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(adjustmentProfile != null);
			Contract.Assert(rewardEffect != null);

			List<IOrderAdjustmentProfileOrderItemTarget> targets = null;
			var effect = rewardEffect.Extension as IAddItemPromotionRewardEffect;
			foreach (var orderCustomer in orderContext.Order.OrderCustomers)
			{
				if (matchResult.MatchForCustomerAccountID(orderCustomer.AccountID))
				{
					targets = targets ?? new List<IOrderAdjustmentProfileOrderItemTarget>();
					var target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
					target.OrderCustomerAccountID = orderCustomer.AccountID;
					target.ProductID = effect.ProductID;
					target.Quantity = effect.Quantity;
					adjustmentProfile.OrderLineModificationTargets.Add(target);

					var addAdjust = Create.New<IOrderAdjustmentProfileOrderLineModification>();
					addAdjust.Description = String.Format("Added {0}.", target.Quantity);
					addAdjust.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
					addAdjust.ModificationValue = target.Quantity;
					addAdjust.Property = "Added";
					target.Modifications.Add(addAdjust);
					targets.Add(target);
				}
			}
			return targets ?? Enumerable.Empty<IOrderAdjustmentProfileOrderItemTarget>();
		}

		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			if (!string.Equals(effect1.ExtensionProviderKey, effect2.ExtensionProviderKey))
				return false;
			if (effect1.PromotionRewardEffectID != effect2.PromotionRewardEffectID)
				return false;
			var effectExt1 = effect1 as IAddItemPromotionRewardEffect;
			var effectExt2 = effect2 as IAddItemPromotionRewardEffect;
			if (effectExt1.ProductID != effectExt2.ProductID)
				return false;
			if (effectExt1.Quantity != effectExt2.Quantity)
				return false;
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.AddItem;
		}

		public override void CheckValidity(string promotionRewardEffectKey, IAddItemPromotionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.ProductID <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"ProductID cannot be less than or equal to 0."
					);
			}
			if (rewardEffect.Quantity <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Quantity cannot be less than or equal to 0."
					);
			}
		}
	}
}
