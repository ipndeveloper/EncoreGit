using NetSteps.OrderAdjustments.Common.Model;
using System.Collections.Generic;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionOrderAdjustmentProfile : IOrderAdjustmentProfile
    {
        int PromotionID { get; set; }
    }
}
