using System;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    [DTO]
    public interface IPromotionCodeQualificationExtension : IPromotionQualificationExtension
    {
        string PromotionCode { get; set; }
    }
}
