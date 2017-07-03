using System;
using NetSteps.Promotions.Common.Model;
using NetSteps.Extensibility.Core;

namespace NetSteps.Promotions.Service.EntityModels
{
    public partial class PromotionQualification : IPromotionQualification
    {
        public IDataObjectExtension Extension { get; set; }
    }
}
