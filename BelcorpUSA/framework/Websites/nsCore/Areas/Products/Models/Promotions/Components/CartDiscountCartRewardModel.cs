using NetSteps.Encore.Core.Dto;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	[DTO]
	public interface ICartDiscountCartReward : ICartRewardModel
	{
		decimal? DiscountPercent { get; set; }
		bool FreeShipping { get; set; }
        int ProductPriceTypeID { get; set; }
	}
}