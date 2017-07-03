using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace NetSteps.Promotions.Plugins.Rewards.Effects.Concrete
{
	[Serializable]
	public class ReduceOrderPropertyValueEffect : IReduceOrderPropertyValuePromotionRewardEffect
    {
        public ReduceOrderPropertyValueEffect()
        {
            MarketDecimalOperands = new Dictionary<int, decimal>();
        }
        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.ReduceOrderPropertyValue; }
        }

        public string OrderPropertyName { get; set; }

        public int OrderAdjustmentOrderOperationID { get; set; }

        public IDictionary<int, decimal> MarketDecimalOperands { get; private set; }

        public int PromotionRewardEffectID { get; set; }

        public int? DefaultMarketID { get; set; }
    }
}
