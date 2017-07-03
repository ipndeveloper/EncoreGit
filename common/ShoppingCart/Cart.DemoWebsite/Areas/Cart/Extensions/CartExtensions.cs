using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;
using Cart.Common;
using Cart.Common.Extensions;
using NetSteps.Encore.Core.IoC;
using System.Diagnostics.Contracts;

namespace Cart.DemoWebsite.Areas.Cart.Extensions
{
	public static class CartExtensions
	{
		public static ICartModel InjectFrom(this ICartModel cartModel, ICart cart)
		{
			Contract.Requires(cartModel != null);
			Contract.Requires(cart != null);

			Create.New<ICartMapper>().CopyToModelFrom(cartModel, cart);
			return cartModel;
		}
	}

	[ContractClass(typeof(CartMapperContracts))]
	public interface ICartMapper
	{
		void CopyToModelFrom(ICartModel cartModel, ICart cart);
		IEnumerable<IItemModel> BuildModelFrom(IEnumerable<ICartItem> items, int paidPriceKind, int volumePriceKind);
		IEnumerable<IKitItemModel> BuildModelFrom(IEnumerable<ICartKitItem> kitItems);
		IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartAdjustment> adjustments);
		IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartPriceAdjustment> priceAdjustments);
	}

	[ContractClassFor(typeof(ICartMapper))]
	abstract class CartMapperContracts : ICartMapper
	{
		public void CopyToModelFrom(ICartModel cartModel, ICart cart)
		{
			Contract.Requires(cartModel != null);
			Contract.Requires(cart != null);
			Contract.Requires(cartModel != null);
			Contract.Requires(cart.Items != null);
			Contract.Requires(cart.Adjustments != null);

			throw new NotImplementedException();
		}

		public IEnumerable<IItemModel> BuildModelFrom(IEnumerable<ICartItem> items, int paidPriceKind, int volumePriceKind)
		{
			Contract.Requires(items != null);

			throw new NotImplementedException();
		}

		public IEnumerable<IKitItemModel> BuildModelFrom(IEnumerable<ICartKitItem> kitItems)
		{
			Contract.Requires(kitItems != null);

			throw new NotImplementedException();
		}

		public IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartAdjustment> adjustments)
		{
			Contract.Requires(adjustments != null);

			throw new NotImplementedException();
		}

		public IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartPriceAdjustment> priceAdjustments)
		{
			Contract.Requires(priceAdjustments != null);

			throw new NotImplementedException();
		}
	}

	[ContainerRegister(typeof(ICartMapper), RegistrationBehaviors.Default)]
	public class CartMapper : ICartMapper
	{
		public void CopyToModelFrom(ICartModel cartModel, ICart cart)
		{
			cartModel.Guid = cart.Guid;
			cartModel.VolumeTotal = cart.GetGrandTotal(cart.VolumePriceKind).ToString("C", CultureInfo.CurrentCulture);
			cartModel.SubTotal = cart.GetSubtotal(cart.PaidPriceKind).ToString();
			cartModel.Total = cart.GetGrandTotal(cart.PaidPriceKind).ToString("C", CultureInfo.CurrentCulture);
			cartModel.Items = BuildModelFrom(cart.Items, cart.PaidPriceKind, cart.VolumePriceKind).ToList();
			cartModel.Adjustments = BuildModelFrom(cart.Adjustments.Where(a => a.PriceKind == cart.PaidPriceKind)).ToList();
			cartModel.VolumeAdjustments = BuildModelFrom(cart.Adjustments.Where(a => a.PriceKind == cart.VolumePriceKind)).ToList();
		}

		public IEnumerable<IItemModel> BuildModelFrom(IEnumerable<ICartItem> items, int paidPriceKind, int volumePriceKind)
		{
			return items.Select(item =>
				{
					var model = Create.New<IItemModel>();
					model.Guid = item.Guid;
					model.Description = item.Description;
					model.Sku = item.Sku;
					model.Quantity = item.Quantity;
					model.UnitPrice = item.GetUnitPrice(paidPriceKind).ToString("C", CultureInfo.CurrentCulture);
					model.Volume = item.GetUnitPrice(volumePriceKind).ToString("C", CultureInfo.CurrentCulture);
					model.Total = item.GetSubtotal(paidPriceKind).ToString("C", CultureInfo.CurrentCulture);
					model.KitItems = BuildModelFrom(item.KitItems).ToList();
					model.Adjustments = BuildModelFrom(item.Prices.Where(p => p.PriceKind == paidPriceKind).SelectMany(p => p.Adjustments)).ToList();
					model.VolumeAdjustments = BuildModelFrom(item.Prices.Where(p => p.PriceKind == volumePriceKind).SelectMany(p => p.Adjustments)).ToList();
					model.IsRemovable = true;
					model.IsQuantityEditable = true;
					return model;
				});
		}

		public IEnumerable<IKitItemModel> BuildModelFrom(IEnumerable<ICartKitItem> kitItems)
		{
			return kitItems.Select(kitItem =>
				{
					var model = Create.New<IKitItemModel>();
					model.Sku = kitItem.Sku;
					model.Description = kitItem.Description;
					model.Quantity = kitItem.Quantity;
					return model;
				});
		}

		public IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartAdjustment> adjustments)
		{
			return adjustments.Select(adjustment =>
				{
					var model = Create.New<IAdjustmentModel>();
					model.Amount = adjustment.Amount;
					model.Description = adjustment.Description;
					return model;
				});
		}

		public IEnumerable<IAdjustmentModel> BuildModelFrom(IEnumerable<ICartPriceAdjustment> priceAdjustments)
		{
			return priceAdjustments.Select(priceAdjustment =>
				{
					var model = Create.New<IAdjustmentModel>();
					model.Amount = priceAdjustment.Amount;
					model.Description = priceAdjustment.Description;
					return model;
				});
		}
	}
}