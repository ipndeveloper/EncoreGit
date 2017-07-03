using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;
using NetSteps.Extensibility.Core;

namespace NetSteps.Promotions.Common.Model
{
    [DTO]
    public interface IPromotionOrderAdjustment : IDataObjectExtension
    {
        int OrderAdjustmentID { get; set; }
        int PromotionID { get; set; }
    }
}
