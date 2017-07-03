using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards
{
	public interface IOrderSubtotalReductionReward : IPromotionReward
	{
		decimal GetMarketDecimalOperand(int marketID);
		void SetMarketDecimalOperand(int marketID, decimal operand);
		void RemoveMarketDecimalOperand(int marketID);
		int? DefaultMarketID { get; set; }
	}
}
