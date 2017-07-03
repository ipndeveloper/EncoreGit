
namespace nsCore.Areas.Products.Models.Promotions
{
	public class FlatDiscountPromotionProductModel : PromotionProductModel, IFlatDiscountPromotionProductModel
	{
		public decimal RetailDiscountPrice { get; set; }
		public decimal? QVDiscountPrice { get; set; }
		public decimal? CVDiscountPrice { get; set; }
	}
}
