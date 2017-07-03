namespace nsCore.Areas.Products.Models.Promotions
{
	public interface IPercentOffPromotionProductModel : IPromotionProductModel
	{
		decimal RetailDiscountPercent { get; set; }

		decimal? CVDiscountPercent { get; set; }

		decimal? QVDiscountPercent { get; set; }
	}
}