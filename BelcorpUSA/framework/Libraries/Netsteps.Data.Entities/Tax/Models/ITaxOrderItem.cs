using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
	public enum DiscountKind
	{
		None = 0,
		/// <summary>
		/// Indicates a percent based discount.
		/// </summary>
		Percent = 1,
		/// <summary>
		/// Indicates an amount based discount.
		/// </summary>
		Amount = 2
	}

	[DTO]
	public interface ITaxOrderItem
	{
		/// <summary>
		/// The item's ID
		/// </summary>
		string ItemID { get; set; }

		/// <summary>
		/// The item's product code, SKU, or identifier.
		/// </summary>
		string ProductCode { get; set; }

		/// <summary>
		/// The item's unit price.
		/// </summary>
		decimal UnitPrice { get; set; }

		/// <summary>
		/// The number of units.
		/// </summary>
		int Quantity { get; set; }

		/// <summary>
		/// Total shipping cost charged to the buyer for this item (not including price or handling).
		/// </summary>
		decimal ShippingFee { get; set; }

		/// <summary>
		/// Total service and handling cost charged to the buyer for this item (not including price or shipping).
		/// </summary>
		decimal HandlingFee { get; set; }

		/// <summary>
		/// Taxes that apply to the order item.
		/// </summary>
		IEnumerable<ITaxOrderItemTaxes> Taxes { get; set; }

		/// <summary>
		/// Kind of discount
		/// </summary>
		DiscountKind DiscountKind { get; set; }

		/// <summary>
		/// Discount to apply to the UnitPrice.
		/// </summary>
		decimal Discount { get; set; }

		/// <summary>
		/// True if the item is taxable.
		/// </summary>
		bool ChargeTax { get; set; }

		/// <summary>
		/// Kind of ITaxOrderItem.
		/// </summary>
		TaxApplicabilityKind Applicability { get; set; }

		/// <summary>
		/// The item's origin address (warehouse), if applicable.
		/// </summary>
		ITaxAddress OriginAddress { get; set; }

		/// <summary>
		/// The item's shipping address, if applicable.
		/// </summary>
		ITaxAddress ShippingAddress { get; set; }
	}
}
