using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Common
{
    public interface IPromotionRewardHandler
    {
        string PromotionRewardKind { get; }

        bool RequiresRemoveNotification { get; }

        void Remove(IPromotionReward reward, IOrderContext orderContext);

        bool RequiresCommitNotification { get; }

        void Commit(IPromotionReward reward, IOrderContext orderContext);

        bool AreEqual(IPromotionReward reward1, IPromotionReward reward2);

        void AddRewardToAdjustmentProfile(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionReward reward, PromotionQualificationResult matchResult);

		void CheckValidity(string promotionKey, IPromotionReward promotionReward, IPromotionState state);
	}
}
