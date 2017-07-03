using System.Collections.Generic;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces
{
	public interface ICartModel
	{
		string Guid { get; set; }
		string SubTotal { get; set; }
		string Total { get; set; }
		string VolumeTotal { get; set; }

		ICartServerOptions ServerOptions { get; }

		IList<IItemModel> Items { get; set; }
		IList<IAdjustmentModel> Adjustments { get; set; }
		IList<IAdjustmentModel> VolumeAdjustments { get; set; }
	}
}
