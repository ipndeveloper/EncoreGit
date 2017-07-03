using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Concrete
{
	[Serializable]
	public class OrderSubtotalReductionReward : BasePromotionReward, IOrderSubtotalReductionReward
	{
        public OrderSubtotalReductionReward(IPriceTypeService priceTypeService)
		{
			var allItemsSelector = GenerateEffect<ISelectAllItemsPromotionRewardEffect>();
			Effects.Add(EffectKeys.AllProducts, allItemsSelector);

            var currencyTypes = priceTypeService.GetCurrencyPriceTypes();
			foreach (var currencyType in currencyTypes)
			{
				var effect = GenerateEffect<IReduceOrderItemPropertyValuePromotionRewardEffect>();
				var extension = (IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension;
				extension.DefaultMarketID = 1;
				extension.MarketDecimalOperands.Add(1, 0);
				extension.OrderAdjustmentOrderLineOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
				extension.OrderItemPropertyName = currencyType.Name;
				Effects.Add(currencyType.Name, effect);
			}

			var commissionTypes = priceTypeService.GetVolumePriceTypes();
			foreach (var commissionType in commissionTypes)
			{
				var effect = GenerateEffect<IReduceOrderItemPropertyValuePromotionRewardEffect>();
				var extension = (IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension;
				extension.DefaultMarketID = 1;
				extension.MarketDecimalOperands.Add(1, 0);
				extension.OrderAdjustmentOrderLineOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
				extension.OrderItemPropertyName = commissionType.Name;
				Effects.Add(commissionType.Name, effect);
			}
		}

		private IReduceOrderItemPropertyValuePromotionRewardEffect firstEffect
		{
			get
			{
				return (IReduceOrderItemPropertyValuePromotionRewardEffect)Effects.Where(effect => !effect.Key.Equals(EffectKeys.AllProducts)).First().Value.Extension;
			}
		}

		private static class EffectKeys
		{
			public const string AllProducts = "Effects All Products In Cart";
		}

		public override string[] OrderOfApplication
		{
			get
			{
				var order = new List<string>();
				order.Add(EffectKeys.AllProducts);
				order.AddRange(Effects.Keys.Where(key => !key.Equals(EffectKeys.AllProducts)));
				return order.ToArray();
			}
		}

		public override string PromotionRewardKind
		{
			get { return RewardKinds.OrderSubtotalReductionReward; }
		}

		private IDictionary<int, decimal> MarketDecimalOperands
		{
			get { return firstEffect.MarketDecimalOperands; }
		}

		public int? DefaultMarketID
		{
			get
			{

				return firstEffect.DefaultMarketID;
			}
			set
			{
				var volumeEffects = GetItemAdjustmentEffects();
				foreach (var effect in volumeEffects)
				{
					effect.DefaultMarketID = value;
				}
			}
		}

		private IEnumerable<IReduceOrderItemPropertyValuePromotionRewardEffect> GetItemAdjustmentEffects()
		{
			return Effects.Values.Select(effect => effect.Extension).OfType<IReduceOrderItemPropertyValuePromotionRewardEffect>();
		}

		public decimal GetMarketDecimalOperand(int marketID)
		{
			if (MarketDecimalOperands.ContainsKey(marketID))
				return MarketDecimalOperands[marketID];
			else
				return 0;
		}

		public void SetMarketDecimalOperand(int marketID, decimal operand)
		{
			var valueEffects = GetItemAdjustmentEffects();
			foreach (var effect in valueEffects)
			{
				if (effect.MarketDecimalOperands.ContainsKey(marketID))
					effect.MarketDecimalOperands[marketID] = operand;
				else
					effect.MarketDecimalOperands.Add(marketID, operand);
			}
		}

		public void RemoveMarketDecimalOperand(int marketID)
		{
			var valueEffects = GetItemAdjustmentEffects();
			foreach (var effect in valueEffects)
			{
				effect.MarketDecimalOperands.Remove(marketID);
			}
		}
	}
}
