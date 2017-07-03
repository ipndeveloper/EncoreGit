namespace nsCore.Areas.Products.Models.Promotions
{
	public interface IFlatDiscountPromotionProductModel : IPromotionProductModel
	{
		decimal RetailDiscountPrice { get; set; }

		decimal? QVDiscountPrice { get; set; }

		decimal? CVDiscountPrice { get; set; }
	}
}