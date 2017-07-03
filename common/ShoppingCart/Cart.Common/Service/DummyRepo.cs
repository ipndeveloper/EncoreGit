using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using System.Diagnostics.Contracts;

namespace Cart.Common.Service
{
	public interface IDummyCartRepo
	{
		IEnumerable<ICart> GetCarts();
	}

	[ContainerRegister(typeof(IDummyCartRepo), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DummyRepo : IDummyCartRepo
	{
		public IEnumerable<ICart> GetCarts()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>() != null);
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(r => r.Items != null));
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(r => r.Adjustments != null));

			var cart = Create.New<ICart>();
			cart.Guid = Guid.NewGuid().ToString();
			cart.PaidPriceKind = 1;
			cart.VolumePriceKind = 2;

			var items = new List<ICartItem>();
			var item1 = Create.New<ICartItem>();
			item1.Guid = Guid.NewGuid().ToString();
			item1.KitItems = new List<ICartKitItem>();
			item1.Quantity = 3u;
			item1.Sku = "FWJWEF";
			item1.Description = "Test Product";

			var item1price1 = Create.New<ICartPrice>();
			var item1price2 = Create.New<ICartPrice>();
			
			item1price1.Adjustments = new List<ICartPriceAdjustment>();
			item1price1.UnitPrice = 21.24m;
			item1price1.PriceKind = 1;
			item1price2.Adjustments = new List<ICartPriceAdjustment>();
			item1price2.UnitPrice = 15m;
			item1price2.PriceKind = 2;
			item1.Prices = new List<ICartPrice> { item1price1, item1price2 };
			items.Add(item1);

			item1 = Create.New<ICartItem>();
			item1.Guid = Guid.NewGuid().ToString();
			item1.KitItems = new List<ICartKitItem>();
			item1.Quantity = 2u;
			item1.Sku = "IFIEIOW";
			item1.Description = "Another Product!";

			item1price1 = Create.New<ICartPrice>();
			item1price2 = Create.New<ICartPrice>();

			item1price1.Adjustments = new List<ICartPriceAdjustment>();
			item1price1.UnitPrice = 45.98m;
			item1price1.PriceKind = 1;
			item1price2.Adjustments = new List<ICartPriceAdjustment>();
			item1price2.UnitPrice = 39.98m;
			item1price2.PriceKind = 2;
			item1.Prices = new List<ICartPrice> { item1price1, item1price2 };
			items.Add(item1);

			cart.Items = items;
			cart.Adjustments = new List<ICartAdjustment>();

			return new List<ICart> { cart };
		}
	}
}
