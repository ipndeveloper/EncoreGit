using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.CoreImplementations;
using NetSteps.Promotions.Plugins.Common.Rewards;

namespace NetSteps.Promotions.Plugins.Rewards
{
	public class SelectFreeItemsFromListRewardHandler : BasePromotionRewardHandler
	{
		public override string PromotionRewardKind
		{
			get { return RewardKinds.SelectFreeItemsFromListReward; }
		}
	}
}
