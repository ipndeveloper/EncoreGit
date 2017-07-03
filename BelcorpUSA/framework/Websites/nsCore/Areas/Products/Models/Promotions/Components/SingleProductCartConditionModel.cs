using NetSteps.Encore.Core.Dto;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	[DTO]
    public interface ISingleProductCartCondition : ICartConditionModel 
    {
        int ProductID { get; set; }
        int Quantity { get; set; }
        bool Cumulative { get; set; }
    }
}