using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Data.Common.Context;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components
{
	public interface IUserProductSelectionOrderStepResponse : IOrderStepResponse
	{
		IList<IProductOption> SelectedOptions { get; }
	}
}
