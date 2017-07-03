using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionOrderStep : IOrderAdjustmentOrderStep
    {
        int PromotionID { get; set; }
        int PromotionRewardID { get; set; }
		new string OrderStepReferenceID { get; set; }
	}
}
