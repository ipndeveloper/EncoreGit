using NetSteps.Encore.Core.Dto;

namespace nsCore.Areas.Orders.Models
{
	[DTO]
	public interface IOrderItemModel
	{
		string Guid { get; set; }
		int ProductID { get; set; }
		string SKU { get; set; }
		string ProductName { get; set; }
		string UnitPrice { get; set; }
		int Quantity { get; set; }
		string CommissionableTotal { get; set; }
		string Total { get; set; }
		bool IsStaticKit { get; set; }
		bool IsDynamicKit { get; set; }
		bool IsDynamicKitFull { get; set; }
		bool IsHostReward { get; set; }
		string BundlePackItemsUrl { get; set; }
		IKitItemsModel KitItemsModel { get; set; }

		// Calculated Properties
		bool IsRemovable { get; }
		bool IsKit { get; }
		bool IsQuantityEditable { get; }
	}
}
