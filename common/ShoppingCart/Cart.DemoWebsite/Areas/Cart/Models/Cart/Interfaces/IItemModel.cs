using System.Collections.Generic;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces
{
	public interface IItemModel
	{
		string Guid { get; set; }
		string Sku { get; set; }
		string Description { get; set; }
		string UnitPrice { get; set; }
		string Total { get; set; }
		string Volume { get; set; }
		uint Quantity { get; set; }
		bool IsRemovable { get; set; }
		bool IsQuantityEditable { get; set; }
		bool IsDynamicKitFull { get; set; }

		IList<IKitItemModel> KitItems { get; set; }
		IList<IAdjustmentModel> Adjustments { get; set; }
		IList<IAdjustmentModel> VolumeAdjustments { get; set; }
	}
}
