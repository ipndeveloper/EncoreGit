using System;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Components
{
    public interface IProductAdjustment
    {
        int MarketID { get; set; }
        int PriceTypeID { get; set; }
        decimal AdjustmentOperand { get; set; }
    }
}
