using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class SelectAllItemsRewardEffectHandler : BasePromotionRewardEffectExtensionHandler<ISelectAllItemsPromotionRewardEffect, ISelectAllItemsPromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardTargetingEffectExtensionHandler
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

			foreach (var orderCustomer in orderContext.Order.OrderCustomers)
			{
				if (matchResult.MatchForCustomerAccountID(orderCustomer.AccountID))
				{
					var nonKitItems = orderCustomer.AdjustableOrderItems.GroupBy(oi => oi.ProductID.Value).Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) });
					foreach (var orderItem in nonKitItems)
					{
						var target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
						target.OrderCustomerAccountID = orderCustomer.AccountID;
						target.ProductID = orderItem.ProductId;
						target.Quantity = orderItem.Quantity;
						adjustmentProfile.OrderLineModificationTargets.Add(target);
						targets.Add(target);
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
			//var effectExt1 = effect1 as ISelectAllItemsPromotionRewardEffect;
			//var effectExt2 = effect2 as ISelectAllItemsPromotionRewardEffect;
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.SelectAllItems;
		}

		public override void CheckValidity(string promotionRewardEffectKey, ISelectAllItemsPromotionRewardEffect rewardEffect, IPromotionState state)
		{
			// nothing to check
		}
	}
}
