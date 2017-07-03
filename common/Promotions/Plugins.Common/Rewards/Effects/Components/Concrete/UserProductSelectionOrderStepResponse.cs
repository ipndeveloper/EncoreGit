using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components.Concrete
{
	[Serializable]
	public class UserProductSelectionOrderStepResponse : IUserProductSelectionOrderStepResponse
	{
		public UserProductSelectionOrderStepResponse()
		{
			SelectedOptions = new List<IProductOption>();
		}
		public IList<IProductOption> SelectedOptions { get; private set; }
	}
}
