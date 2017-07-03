using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components
{
	public interface IUserProductSelectionOrderStep : IPromotionOrderStep
	{
		IList<IProductOption> AvailableOptions { get; }
		int MaximumOptionSelectionCount { get; set; }
	}
}
