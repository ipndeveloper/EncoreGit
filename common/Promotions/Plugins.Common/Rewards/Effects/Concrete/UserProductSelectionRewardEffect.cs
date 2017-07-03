using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Concrete
{
	[Serializable]
	public class UserProductSelectionRewardEffect : IUserProductSelectionRewardEffect
	{
		public UserProductSelectionRewardEffect()
		{
			Selections = new List<IProductOption>();
		}

		public int SelectionsAllowed { get; set; }

		public IList<IProductOption> Selections	{ get; private set;}

		public string ExtensionProviderKey
		{
			get { return NetStepsPromotionRewardEffectExtensionProviderKeys.UserProductSelection; }
		}

		public int PromotionRewardEffectID { get; set; }
        public bool? IsEspecialPromotion { get; set; }
	}
}
