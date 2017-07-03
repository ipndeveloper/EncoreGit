using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects
{
    public interface IAddItemByFactorInCartPromotionRewardEffect : IPromotionRewardEffectExtension
    {
        int ProductIDInCart { get; set; }
        int ProductIDToAdd { get; set; }
        decimal Factor { get; set; }
        int MaximumRewardCount { get; set; }
    }
}
