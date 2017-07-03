using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Orders.Models.Details
{
	public class PartialOrderItemDetailsModel
	{
		public Order Order { get; set; }
		public OrderCustomer OrderCustomer { get; set; }
		public List<OrderItemDetailModel> OrderItemDetails { get; set; }
		public bool IsReturnOrder { get; set; }
		public bool IsReplacementOrder { get; set; }
		public Currency Currency { get; set; }
		public int OrderAdjustmentAddOperationKindID { get; set; }
		public InventoryBaseRepository Inventory { get; set; }
		public IEnumerable<OrderItem> AddedOrderItems { get; set; }
	}
}