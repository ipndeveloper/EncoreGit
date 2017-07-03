using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IOrderPriceTypeTotalRangeQualificationExtension : IPromotionQualificationExtension
    {
        int ProductPriceTypeID { get; set; }
        IDictionary<int, IOrderPriceTypeTotalRange> OrderPriceTypeTotalRangesByCurrencyID { get; }
    }
}
