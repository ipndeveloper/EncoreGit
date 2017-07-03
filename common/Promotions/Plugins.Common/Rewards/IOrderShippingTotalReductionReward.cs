using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards
{
	public interface IOrderShippingTotalReductionReward : IPromotionReward
	{
		int OrderAdjustmentOrderOperationID { get; set; }
		IDictionary<int, decimal> MarketDecimalOperands { get; }
		int? DefaultMarketID { get; set; }
	}
}
