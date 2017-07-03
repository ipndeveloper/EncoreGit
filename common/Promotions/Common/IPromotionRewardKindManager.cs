using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common
{
    public interface IPromotionRewardKindManager
    {

        void RegisterPromotionRewardKind<TPromotionReward>(string promotionKind) where TPromotionReward : IPromotionReward;

        bool UnregisterRewardKind(string rewardKindName);

        IPromotionReward CreatePromotionReward(string rewardKindName);
        TPromotionReward CreatePromotionReward<TPromotionReward>(string rewardKindName) where TPromotionReward : IPromotionReward;

        string GetPromotionRewardKindString<TPromotionReward>() where TPromotionReward : IPromotionReward;
    }
}
