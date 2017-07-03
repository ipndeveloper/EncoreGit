
namespace nsCore.Areas.Products.Models.Promotions.Interfaces
{
	public interface ICartRewardModel
	{
		//int PromotionID { get; set; }
		//string Name { get; set; }
		//string CouponCode { get; set; }
		//bool OneTimeUse { get; set; }
		//DateTime? StartDate { get; set; }
		//DateTime? EndDate { get; set; }
		//IList<int> OrderTypeIDs { get; set; }
		//IList<int> PaidAsTitleIDs { get; set; }
		//IList<int> RecognizedTitleIDs { get; set; }
		//IList<int> AccountTypeIDs { get; set; }
	}

	public enum CartRewardType
	{
		AddProductsToCart,
		PickFromListOfProducts,
		DiscountOrFreeShipping
	}
}
