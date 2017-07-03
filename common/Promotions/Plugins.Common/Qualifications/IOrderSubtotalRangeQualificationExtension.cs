using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IOrderSubtotalRangeQualificationExtension : IPromotionQualificationExtension
    {
        IDictionary<int, IOrderSubtotalRange> OrderSubtotalRangesByCurrencyID { get; }
    }
}
