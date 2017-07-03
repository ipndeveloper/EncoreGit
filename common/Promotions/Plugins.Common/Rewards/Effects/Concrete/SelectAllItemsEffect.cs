using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace NetSteps.Promotions.Plugins.Rewards.Effects.Concrete
{
	[Serializable]
	public class SelectAllItemsEffect : ISelectAllItemsPromotionRewardEffect
    {
        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.SelectAllItems; }
        }

        public int PromotionRewardEffectID { get; set; }
    }
}
