using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Concrete
{
	[Serializable]
	public class SelectItemsInProductIDSetEffect : ISelectItemsInProductIDSetPromotionRewardEffect
    {
        public SelectItemsInProductIDSetEffect()
        {
            ProductIDs = new List<int>();
        }

        public IList<int> ProductIDs { get; private set; }

        public string ExtensionProviderKey
        {
            get { return NetStepsPromotionRewardEffectExtensionProviderKeys.SelectItemsInProductIDSet; }
        }

        public int PromotionRewardEffectID { get; set; }
    }
}
