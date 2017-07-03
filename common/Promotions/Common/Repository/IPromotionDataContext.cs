using System;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common;

namespace NetSteps.Promotions.Common.Repository
{
    public interface IPromotionDataContext : IObjectContext
    {
        IDTOQueryable<IPromotion> Promotions { get; }
        IDTOQueryable<IPromotionOrderAdjustmentProfile> PromotionOrderAdjustments { get; }
    }
}
