using NetSteps.Encore.Core.Dto;
using NetSteps.Extensibility.Core;
using System.Collections.Generic;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionQualification : IExtensibleDataObject
    {
        int PromotionQualificationID { get; set; }
    }
}
