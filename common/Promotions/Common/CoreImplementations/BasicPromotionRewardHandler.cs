
namespace NetSteps.Promotions.Common.CoreImplementations
{
    public sealed class BasicPromotionRewardHandler : BasePromotionRewardHandler
    {
        public const string PromotionRewardKindName = "Basic";
        public override string PromotionRewardKind
        {
            get { return PromotionRewardKindName; }
        }
    }
}
