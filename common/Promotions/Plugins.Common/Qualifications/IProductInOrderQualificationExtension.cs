using System;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IProductInOrderQualificationExtension : IPromotionQualificationExtension
    {
        int ProductID { get; set; }
        int Quantity { get; set; }
    }
}
