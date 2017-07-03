
namespace nsCore.Areas.Products.Models.Promotions
{
	public class PercentOffPromotionProductModel : PromotionProductModel, IPercentOffPromotionProductModel
	{
		public decimal RetailDiscountPercent { get; set; }
		public decimal? CVDiscountPercent { get; set; }
		public decimal? QVDiscountPercent { get; set; }
	}
}
