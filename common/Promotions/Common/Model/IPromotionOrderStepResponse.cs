using NetSteps.Data.Common.Context;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionOrderStepResponse : IOrderStepResponse
    {
        int PromotionID { get; set; }
        int PromotionRewardID { get; set; }
    }
}
