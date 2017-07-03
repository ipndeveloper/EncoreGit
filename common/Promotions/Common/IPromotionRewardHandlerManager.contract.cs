using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common
{
	[ContractClassFor(typeof(IPromotionRewardHandlerManager))]
	public abstract class PromotionRewardHandlerManagerContract : IPromotionRewardHandlerManager
	{
		public IPromotionRewardHandler GetRewardHandler(string rewardKindName)
		{
			Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(rewardKindName), "PromotionRewardHandlerManager.GetRewardHandler cannot have a null or empty rewardKindName argument.");
			throw new NotImplementedException();
		}

		public TPromotionRewardHandler GetRewardHandler<TPromotionRewardHandler>(string rewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler
		{
			Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(rewardKindName), "PromotionRewardHandlerManager.GetRewardHandler cannot have a null or empty rewardKindName argument.");
			throw new NotImplementedException();
		}

		public void RegisterHandler<TPromotionRewardHandler>(string rewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler
		{
			Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(rewardKindName), "PromotionRewardHandlerManager.GetRewardHandler cannot have a null or empty rewardKindName argument.");
			throw new NotImplementedException();
		}
	}
}
