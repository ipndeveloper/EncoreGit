using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace NetSteps.Promotions.Plugins.Rewards.Effects.Concrete
{
	[Serializable]
	public class AddItemByFactorInOrderEffect : IAddItemByFactorInCartPromotionRewardEffect
    {
        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.AddItemByFactorInCart; }
        }

        public int ProductIDInCart { get; set; }

        public int ProductIDToAdd { get; set; }

        public decimal Factor { get; set; }

        public int MaximumRewardCount { get; set; }

        public int PromotionRewardEffectID { get; set; }
    }
}
