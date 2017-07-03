using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public interface ICartRewardsPromotionModel : ISingleMarketStandardQualificationPromotionModel
	{
		ICartConditionModel CartCondition { get; set; }

		ICartRewardModel CartReward { get; set; }
	}
}