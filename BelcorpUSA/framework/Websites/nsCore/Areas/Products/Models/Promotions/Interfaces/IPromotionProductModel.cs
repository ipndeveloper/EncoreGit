using System.Collections.Generic;
namespace nsCore.Areas.Products.Models.Promotions
{
	public interface IPromotionProductModel
	{
		int ProductID { get; set; }

		decimal? RetailPrice { get; set; }

		decimal? CVPrice { get; set; }

		decimal? QVPrice { get; set; }

		string Name { get; set; }

		string SKU { get; set; }

		bool Active { get; set; }

		List<IPromotionPriceTypeInfoModel> AdditionalPriceTypeValues { get; set; }

	}

	public interface IPromotionPriceTypeInfoModel
	{
		string PriceTypeName { get; set; }
		decimal? Value { get; set; }
	}
}