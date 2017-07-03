using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class ReduceOrderItemPropertyValueRewardEffectHandler : BasePromotionRewardEffectExtensionHandler<IReduceOrderItemPropertyValuePromotionRewardEffect, IReduceOrderItemPropertyValuePromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardTargetedEffectExtensionHandler
	{
		public virtual void AddEffectToOrderLineModificationTargets(
			IOrderContext orderContext,
			IEnumerable<IOrderAdjustmentProfileOrderItemTarget> orderItemTargets,
			IPromotionRewardEffect rewardEffect,
			PromotionQualificationResult matchResult,
			IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(rewardEffect != null);

			var effect = rewardEffect.Extension as IReduceOrderItemPropertyValuePromotionRewardEffect;
			int effectiveMarketID = orderContext.Order.GetShippingMarketID();
			if (!effect.MarketDecimalOperands.ContainsKey(effectiveMarketID))
			{
				if (effect.DefaultMarketID.HasValue)
				{
					effectiveMarketID = effect.DefaultMarketID.Value;
					if (!effect.MarketDecimalOperands.ContainsKey(effectiveMarketID))
					{
						throw new Exception(String.Format("Default market ID {0} for promotion reward effect id {1} is set but effect contains no value for that market ID.", effect.DefaultMarketID.Value, effect.PromotionRewardEffectID));
					}
				}
				else
				{
					throw new Exception(String.Format("Default market ID for promotion reward effect id {0} is not set but contains no value for market ID {1}", effect.PromotionRewardEffectID, orderContext.Order.GetShippingMarketID()));
				}
			}
			foreach (var target in orderItemTargets)
			{
				var adjustment = Create.New<IOrderAdjustmentProfileOrderLineModification>();
				switch (effect.OrderAdjustmentOrderLineOperationID)
				{
					case (int)OrderAdjustmentOrderLineOperationKind.FlatAmount:
						adjustment.Description = String.Format("{0} reduced by {1}", effect.OrderItemPropertyName, effect.MarketDecimalOperands[effectiveMarketID]);
						break;
					case (int)OrderAdjustmentOrderLineOperationKind.Multiplier:
						adjustment.Description = String.Format("{0} reduced by multiplier of {1}", effect.OrderItemPropertyName, effect.MarketDecimalOperands[effectiveMarketID]);
						break;
					default:
						throw new NotImplementedException(String.Format("Effect ID {0} implements unaccounted for OrderAdjustmentOrderLineOperationID {1}", effect.PromotionRewardEffectID, effect.OrderAdjustmentOrderLineOperationID));
				}
				adjustment.ModificationOperationID = effect.OrderAdjustmentOrderLineOperationID;
				adjustment.ModificationValue = effect.MarketDecimalOperands[effectiveMarketID];
				adjustment.Property = effect.OrderItemPropertyName;
				target.Modifications.Add(adjustment);
			}
		}
		
		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			if (!string.Equals(effect1.ExtensionProviderKey, effect2.ExtensionProviderKey))
				return false;
			if (effect1.PromotionRewardEffectID != effect2.PromotionRewardEffectID)
				return false;
			var effectExt1 = effect1 as IReduceOrderItemPropertyValuePromotionRewardEffect;
			var effectExt2 = effect2 as IReduceOrderItemPropertyValuePromotionRewardEffect;
			if (effectExt1.DefaultMarketID != effectExt2.DefaultMarketID)
				return false;
			if (effectExt1.MarketDecimalOperands.Keys.Except(effectExt2.MarketDecimalOperands.Keys).Count() > 0)
				return false;
			if (effectExt2.MarketDecimalOperands.Keys.Except(effectExt1.MarketDecimalOperands.Keys).Count() > 0)
				return false;
			foreach (var key in effectExt1.MarketDecimalOperands.Keys)
			{
				if (effectExt1.MarketDecimalOperands[key] != effectExt2.MarketDecimalOperands[key])
					return false;
			}
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.ReduceOrderItemPropertyValue;
		}

		public override void CheckValidity(string promotionRewardEffectKey, IReduceOrderItemPropertyValuePromotionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.MarketDecimalOperands == null || !rewardEffect.MarketDecimalOperands.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Market Operands collection is null or empty."
					);
			}
			if (String.IsNullOrEmpty(rewardEffect.OrderItemPropertyName))
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Order Item Property name cannot be null or empty."
					);
			}
			switch (rewardEffect.OrderAdjustmentOrderLineOperationID)
			{
				case (int)OrderAdjustmentOrderLineOperationKind.AddedItem:
				case (int)OrderAdjustmentOrderLineOperationKind.FlatAmount:
				case (int)OrderAdjustmentOrderLineOperationKind.Multiplier:
					break;
				default:
					state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						String.Format("Order Line Operation ID '{0}'is invalid.", rewardEffect.OrderAdjustmentOrderLineOperationID)
					);
					break;
			}
		}
	}
}
