using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects
{
	public interface IUserProductSelectionRewardEffect : IPromotionRewardEffectExtension
	{
		int SelectionsAllowed { get; set; }
		IList<IProductOption> Selections { get; }
        bool? IsEspecialPromotion { get; set; }
	}
}
