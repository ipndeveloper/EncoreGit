using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects
{
    public interface ISelectItemWithProductIDPromotionRewardEffect : IPromotionRewardEffectExtension
    {
        int ProductID { get; set; }
        int MaximumQuantity { get; set; }
    }
}
