using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class ReduceOrderPropertyValueRewardEffectHandler : BasePromotionRewardEffectExtensionHandler<IReduceOrderPropertyValuePromotionRewardEffect, IReduceOrderPropertyValuePromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardGeneralEffectExtensionHandler
	{
		public virtual void AddEffectToOrderAdjustmentProfile(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionRewardEffect rewardEffect, PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(adjustmentProfile != null);
			Contract.Assert(rewardEffect != null);

			var effect = rewardEffect.Extension as IReduceOrderPropertyValuePromotionRewardEffect;
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
			var adjustment = Create.New<IOrderAdjustmentProfileOrderModification>();
			switch (effect.OrderAdjustmentOrderOperationID)
			{
				case (int)OrderAdjustmentOrderOperationKind.FlatAmount:
					adjustment.Description = String.Format("{0} reduced by {1}", effect.OrderPropertyName, effect.MarketDecimalOperands[effectiveMarketID]);
					break;
				case (int)OrderAdjustmentOrderOperationKind.Multiplier:
					adjustment.Description = String.Format("{0} reduced by multiplier of {1}", effect.OrderPropertyName, effect.MarketDecimalOperands[effectiveMarketID]);
					break;
				default:
					throw new NotImplementedException(String.Format("Effect ID {0} implements unaccounted for OrderAdjustmentOrderOperationID {1}", effect.PromotionRewardEffectID, effect.OrderAdjustmentOrderOperationID));
			}
			adjustment.ModificationOperationID = effect.OrderAdjustmentOrderOperationID;
			adjustment.ModificationValue = effect.MarketDecimalOperands[effectiveMarketID];
			adjustment.Property = effect.OrderPropertyName;
			if (adjustmentProfile.AffectedAccountIDs.Count == 0)
			{
				var accounts = orderContext.Order.OrderCustomers.Where(customer => matchResult.MatchForCustomerAccountID(customer.AccountID));
				foreach (var account in accounts)
				{
					adjustmentProfile.AffectedAccountIDs.Add(account.AccountID);
				}
			}
			adjustmentProfile.OrderModifications.Add(adjustment);
		}

		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			if (!string.Equals(effect1.ExtensionProviderKey, effect2.ExtensionProviderKey))
				return false;
			if (effect1.PromotionRewardEffectID != effect2.PromotionRewardEffectID)
				return false;
			var effectExt1 = effect1 as IReduceOrderPropertyValuePromotionRewardEffect;
			var effectExt2 = effect2 as IReduceOrderPropertyValuePromotionRewardEffect;
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
			return NetStepsPromotionRewardEffectExtensionProviderKeys.ReduceOrderPropertyValue;
		}

		public override void CheckValidity(string promotionRewardEffectKey, IReduceOrderPropertyValuePromotionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.MarketDecimalOperands == null || !rewardEffect.MarketDecimalOperands.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Market Operands collection is null or empty."
					);
			}
			if (String.IsNullOrEmpty(rewardEffect.OrderPropertyName))
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Order Property name cannot be null or empty."
					);
			}
			switch (rewardEffect.OrderAdjustmentOrderOperationID)
			{
				case (int)OrderAdjustmentOrderOperationKind.FlatAmount:
				case (int)OrderAdjustmentOrderOperationKind.Multiplier:
					break;
				default:
					state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						String.Format("Order Operation ID '{0}'is invalid.", rewardEffect.OrderAdjustmentOrderOperationID)
					);
					break;
			}
		}
	}
}
