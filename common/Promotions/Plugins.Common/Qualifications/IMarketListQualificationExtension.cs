using System;
using NetSteps.Promotions.Common.Model;
using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IMarketListQualificationExtension : IPromotionQualificationExtension
    {
        IList<int> Markets { get; }
    }
}
