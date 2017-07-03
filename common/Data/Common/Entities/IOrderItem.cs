using System.Collections.Generic;

namespace NetSteps.Data.Common.Entities
{
	public interface IOrderItem
	{
		int OrderItemID { get; set; }
		int? ProductID { get; set; }
		int Quantity { get; set; }
		int ProductPriceTypeID { get; set; }
		decimal ItemPrice { get; set; }
		IOrderItem ParentOrderItem { get; }
		IList<IOrderAdjustmentOrderLineModification> OrderLineModifications { get; }
		IList<IOrderItemProperty> OrderItemProperties { get; }
		
		void AddOrUpdateOrderItemProperty(string name, string value);
		void MarkAsDeleted();
		void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification);
		decimal GetAdjustedPrice(int priceTypeID);
	}
}
