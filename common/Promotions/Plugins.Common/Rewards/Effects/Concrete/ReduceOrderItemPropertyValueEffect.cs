using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace NetSteps.Promotions.Plugins.Rewards.Effects.Concrete
{
	[Serializable]
	public class ReduceOrderItemPropertyValueEffect : IReduceOrderItemPropertyValuePromotionRewardEffect
    {
        public ReduceOrderItemPropertyValueEffect()
        {
            MarketDecimalOperands = new Dictionary<int, decimal>();
        }

        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.ReduceOrderItemPropertyValue; }
        }

        public string OrderItemPropertyName { get; set; }

        public int OrderAdjustmentOrderLineOperationID { get; set; }

        public IDictionary<int, decimal> MarketDecimalOperands { get; private set; }

        public int PromotionRewardEffectID { get; set; }

        public int? DefaultMarketID { get; set; }
    }
}
