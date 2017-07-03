using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components.Concrete
{
	[Serializable]
	public class UserProductSelectionOrderStep : IUserProductSelectionOrderStep
	{
		public UserProductSelectionOrderStep()
		{
			AvailableOptions = new List<IProductOption>();
		}

		public IList<Helpers.IProductOption> AvailableOptions { get; private set; }

		public IOrderStepResponse Response
		{
			get
			{
				return UserProductSelectionResponse;
			}
			set
			{
				Contract.Assert(typeof(IUserProductSelectionOrderStepResponse).IsAssignableFrom(value.GetType()));
				UserProductSelectionResponse = (IUserProductSelectionOrderStepResponse)value;
			}
		}

		public IUserProductSelectionOrderStepResponse UserProductSelectionResponse { get; set; }

		public int MaximumOptionSelectionCount { get; set; }

		public int CustomerAccountID { get; set; }

		public string OrderStepReferenceID { get; set; }

		public int PromotionRewardID { get; set; }

		public int PromotionID { get; set; }
	}
}
