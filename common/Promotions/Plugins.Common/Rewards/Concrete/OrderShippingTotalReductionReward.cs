using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Concrete
{
	[Serializable]
	public class OrderShippingTotalReductionReward : BasePromotionReward, IOrderShippingTotalReductionReward
	{
		public OrderShippingTotalReductionReward()
		{
			var shippingEffect = GenerateEffect<IReduceOrderPropertyValuePromotionRewardEffect>();
			var shippingEffectExtension = (IReduceOrderPropertyValuePromotionRewardEffect)shippingEffect.Extension;
			shippingEffectExtension.OrderPropertyName = NetSteps.Data.Common.Entities.OrderAdjustablePropertyNames.ShippingTotal;
			Effects.Add(EffectKeys.ReducedShipping, shippingEffect);
		}

		private static class EffectKeys
		{
			public const string ReducedShipping = "Reduced Shipping";
		}

		IReduceOrderPropertyValuePromotionRewardEffect ReducedPropertyEffect
		{
			get
			{
				return (IReduceOrderPropertyValuePromotionRewardEffect)Effects[EffectKeys.ReducedShipping].Extension;
			}
		}

		public override string[] OrderOfApplication
		{
			get { return new string[] { EffectKeys.ReducedShipping }; }
		}

		public override string PromotionRewardKind
		{
			get { return RewardKinds.OrderShippingTotalReductionReward; }
		}

		public int OrderAdjustmentOrderOperationID
		{
			get
			{
				return ReducedPropertyEffect.OrderAdjustmentOrderOperationID;
			}
			set
			{
				ReducedPropertyEffect.OrderAdjustmentOrderOperationID = value;
			}
		}

		public IDictionary<int, decimal> MarketDecimalOperands
		{
			get { return ReducedPropertyEffect.MarketDecimalOperands; }
		}

		public int? DefaultMarketID
		{
			get
			{
				return ReducedPropertyEffect.DefaultMarketID;
			}
			set
			{
				ReducedPropertyEffect.DefaultMarketID = value;
			}
		}
	}
}
