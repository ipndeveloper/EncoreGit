using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models
{
	[ContainerRegister(typeof(IItemModel), RegistrationBehaviors.Default)]
	public class ItemModel : IItemModel
	{
		public ItemModel()
		{
			this.KitItems = new List<IKitItemModel>();
			this.Adjustments = new List<IAdjustmentModel>();
			this.VolumeAdjustments = new List<IAdjustmentModel>();
		}

		#region Implementation of IItemModel

		public string Guid { get; set; }
		public string Sku { get; set; }
		public string Description { get; set; }
		public string UnitPrice { get; set; }
		public string Total { get; set; }
		public string Volume { get; set; }
		public uint Quantity { get; set; }
		public bool IsRemovable { get; set; }
		public bool IsQuantityEditable { get; set; }
		public bool IsDynamicKitFull { get; set; }

		public IList<IKitItemModel> KitItems { get; set; }
		public IList<IAdjustmentModel> Adjustments { get; set; }
		public IList<IAdjustmentModel> VolumeAdjustments { get; set; }

		#endregion
	}
}