using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace NetSteps.Promotions.Plugins.Rewards.Effects.Concrete
{
	[Serializable]
	public class AddItemEffect : IAddItemPromotionRewardEffect
    {
        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.AddItem; }
        }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public int PromotionRewardEffectID { get; set; }
    }
}
