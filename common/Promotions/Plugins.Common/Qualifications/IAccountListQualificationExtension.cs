using System;
using System.Collections.Generic;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IAccountListQualificationExtension : IPromotionQualificationExtension
    {
        IList<int> AccountNumbers { get; }
    }
}
