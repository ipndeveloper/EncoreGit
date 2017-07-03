using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class SelectItemWithProductIDRewardEffectHandler
		: BasePromotionRewardEffectExtensionHandler<ISelectItemWithProductIDPromotionRewardEffect, ISelectItemWithProductIDPromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>,
			IPromotionRewardTargetingEffectExtensionHandler
	{
		public virtual IEnumerable<IOrderAdjustmentProfileOrderItemTarget> CreateOrderLineModificationTargets(
			IOrderContext orderContext,
			IOrderAdjustmentProfile adjustmentProfile,
			IPromotionRewardEffect rewardEffect,
			PromotionQualificationResult matchResult,
			IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(adjustmentProfile != null);
			Contract.Assert(rewardEffect != null);

			var targets = new List<IOrderAdjustmentProfileOrderItemTarget>();

			var effect = rewardEffect.Extension as ISelectItemWithProductIDPromotionRewardEffect;
			foreach (var orderCustomer in orderContext.Order.OrderCustomers)
			{
				if (matchResult.MatchForCustomerAccountID(orderCustomer.AccountID))
				{
					var nonKitItems = orderCustomer.AdjustableOrderItems;
					foreach (var orderItem in nonKitItems)
					{
						if (orderItem.ProductID == effect.ProductID)
						{
							var target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
							target.OrderCustomerAccountID = orderCustomer.AccountID;
							target.ProductID = orderItem.ProductID.Value;
							target.Quantity = effect.MaximumQuantity;
							adjustmentProfile.OrderLineModificationTargets.Add(target);
							targets.Add(target);
						}
					}
				}
			}

			return targets;
		}
		
		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			if (!string.Equals(effect1.ExtensionProviderKey, effect2.ExtensionProviderKey))
				return false;
			if (effect1.PromotionRewardEffectID != effect2.PromotionRewardEffectID)
				return false;
			var effectExt1 = effect1 as ISelectItemWithProductIDPromotionRewardEffect;
			var effectExt2 = effect2 as ISelectItemWithProductIDPromotionRewardEffect;
			if (effectExt1.MaximumQuantity != effectExt2.MaximumQuantity)
				return false;
			if (effectExt1.ProductID != effectExt2.ProductID)
				return false;
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.SelectItemsWithProductID;
		}

		public override void CheckValidity(string promotionRewardEffectKey, ISelectItemWithProductIDPromotionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.MaximumQuantity < 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", promotionRewardEffectKey),
						"Maximum Quantity cannot be less than 0."
					);
			}

			if (rewardEffect.ProductID <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", promotionRewardEffectKey),
						"ProductID cannot be less than or equal to 0."
					);
			}
		}
	}
}
