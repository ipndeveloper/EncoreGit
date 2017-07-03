using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Promotions.Plugins.Common.Rewards
{
    public interface IProductReward : IPromotionReward
    {
        int ProductID { get; set; }
        bool ContainsPriceTypeEffect(IPriceType priceType);
		IReduceOrderItemPropertyValuePromotionRewardEffect GetPriceTypeEffect(IPriceType priceType);
        void AddPriceTypeEffect(IPriceType priceType, int orderAdjustmentOrderLineOperationKindID);
        IEnumerable<IPriceType> PriceTypes { get; }
        IEnumerable<IReduceOrderItemPropertyValuePromotionRewardEffect> PriceTypeEffects { get; }
    }
}
