using System.Collections.Generic;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IOrderHasMinimumProductSelectionsQualificationExtension : IPromotionQualificationExtension
    {
        IList<IProductOption> ProductOptions { get; }
        int NumberOfOptionsRequired { get; set; }
    }
}
