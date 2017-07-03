using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common
{
	[ContractClass(typeof(PromotionRewardHandlerManagerContract))]
    public interface IPromotionRewardHandlerManager
    {
        IPromotionRewardHandler GetRewardHandler(string rewardKindName);
        TPromotionRewardHandler GetRewardHandler<TPromotionRewardHandler>(string rewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler;
        void RegisterHandler<TPromotionRewardHandler>(string rewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler;
    }

	

}
