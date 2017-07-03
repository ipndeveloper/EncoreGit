using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models
{
	[ContainerRegister(typeof(ICartModel), RegistrationBehaviors.Default)]
	public class CartModel : ICartModel
	{
		public CartModel()
		{
			this.ServerOptions = Create.New<ICartServerOptions>();

			this.Items = new List<IItemModel>();
			this.Adjustments = new List<IAdjustmentModel>();
			this.VolumeAdjustments = new List<IAdjustmentModel>();
		}

		public string Guid { get; set; }
		public string SubTotal { get; set; }
		public string Total { get; set; }
		public string VolumeTotal { get; set; }

		public ICartServerOptions ServerOptions { get; private set; }

		public IList<IItemModel> Items { get; set; }
		public IList<IAdjustmentModel> Adjustments { get; set; }
		public IList<IAdjustmentModel> VolumeAdjustments { get; set; }
	}
}