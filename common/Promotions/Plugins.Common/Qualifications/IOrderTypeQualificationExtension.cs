using System.Collections.Generic;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IOrderTypeQualificationExtension : IPromotionQualificationExtension
    {
        IList<int> OrderTypes { get; }
    }
}
