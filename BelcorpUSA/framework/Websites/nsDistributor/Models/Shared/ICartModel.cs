using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	[DTO]
	public interface ICartModel
	{
		IList<IOrderItemModel> OrderItems { get; set; }
		IList<IPromotionallyAddedItemModel> PromotionallyAddedItems { get; set; }
		IList<IPromotionInfoModel> ApplicablePromotions { get; set; }
		decimal? Subtotal { get; set; }
		decimal? AdjustedSubtotal { get; set; }
		decimal? Tax { get; set; }
		decimal? ShippingHandling { get; set; }
		decimal? AdjustedShippingHandling { get; set; }
		decimal? GrandTotal { get; set; }
		string CurrencySymbol { get; set; }
		object OutOfStockProducts { get; set; }
	}
}