using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cart.Common;
using Cart.Common.Extensions;
using NetSteps.Encore.Core.IoC;

namespace Cart.Common.Tests
{
	[TestClass]
	public class ICartExtensionTests
	{
		#region Cart
		[TestMethod]
		public void ICart_GetSubtotal_Returns_SubtotalForAllItemsThenAddsAdjustments()
		{
			var adjustmentAmount1 = -2m;
			var adjustmentAmount2 = -4.35m;

			var itemAmount1 = GetRandomAmount();
			var item1Q = 2u;
			var itemAmount2 = GetRandomAmount();
			var item2Q = 3u;
			var item1Adjustment1 = -1m;
			var expected = ((itemAmount1 + item1Adjustment1) * item1Q) + (itemAmount2 * item2Q) + adjustmentAmount1 + adjustmentAmount2;
			var item1 = CreateItem(item1Q, new List<ICartPrice> { CreatePriceWithAdjustments(itemAmount1, item1Adjustment1) });
			var item2 = CreateItem(item2Q, new List<ICartPrice> { CreatePriceWithAdjustments(itemAmount2) });
			var cart = CreateCart(new List<ICartItem> { item1, item2 }, adjustmentAmount1, adjustmentAmount2);
			Assert.AreEqual(expected, cart.GetSubtotal());
		}
		#endregion

		#region Item
		[TestMethod]
		public void ICartItem_GetUnitPrice_Returns_UnitPriceForPriceKindWithoutQuantity()
		{
			var quantity = 5u;
			var prices = Enumerable.Range(0, 10).Select(x => CreatePrice(GetRandomAmount(), x)).ToList();
			var item = CreateItem(quantity, prices);
			foreach (var price in prices)
			{
				Assert.AreEqual(price.GetUnitPrice(), item.GetUnitPrice(priceKind: price.PriceKind));
			}
		}

		[TestMethod]
		public void ICartItem_GetSubtotal_Returns_UnitPriceForPriceKindWithQuantity()
		{
			var quantity = 5u;
			var prices = Enumerable.Range(0, 10).Select(x => CreatePrice(GetRandomAmount(), x)).ToList();
			var item = CreateItem(quantity, prices);
			foreach (var price in prices)
			{
				Assert.AreEqual(price.GetUnitPrice() * quantity, item.GetSubtotal(priceKind: price.PriceKind));
			}
		}
		#endregion

		#region Price
		[TestMethod]
		public void ICartPrice_GetUnitPriceForIEnumerable_Returns_GetUnitPriceForPriceKind()
		{
			var prices = Enumerable.Range(0, 10).Select(x => CreatePrice(GetRandomAmount(), x)).ToList();
			foreach (var price in prices)
			{
				Assert.AreEqual(price.GetUnitPrice(), prices.GetUnitPrice(priceKind: price.PriceKind));
			}
		}

		[TestMethod]
		public void ICartPrice_GetUnitPriceNoAdjustments_Returns_UnitPrice()
		{
			var amount = GetRandomAmount();
			var price = CreatePrice(amount);
			Assert.AreEqual(amount, price.UnitPrice);
			Assert.AreEqual(amount, price.GetUnitPrice(adjusted: false));
		}

		[TestMethod]
		public void ICartPrice_GetUnitPriceWithAdjustments_Returns_UnitPricePlusAdjustments()
		{
			var amount = GetRandomAmount();
			var adjustmentAmount1 = -2m;
			var adjustmentAmount2 = -4.35m;
			var expected = amount + adjustmentAmount1 + adjustmentAmount2;
			var price = CreatePriceWithAdjustments(amount, adjustmentAmount1, adjustmentAmount2);
			Assert.AreEqual(expected, price.GetUnitPrice());
		}
		#endregion

		#region Adjustment
		[TestMethod]
		public void ICartAdjustment_GetTotalAdjustment_ReturnsAmountSum()
		{
			var priceKind = 1;
			var expected = 0m;
			var adjustments = Enumerable.Range(0, 10).Select(x =>
				{
					var amount = GetRandomAmount();
					expected += amount;
					return CreateAdjustment(amount, priceKind);
				}).ToList();

			Assert.AreEqual(adjustments.GetTotalAdjustment(priceKind), expected);
		}
		#endregion

		#region Price Adjustment
		[TestMethod]
		public void ICartPriceAdjustment_GetTotalAdjustment_ReturnsAmountSum()
		{
			var expected = 0m;
			var adjustments = Enumerable.Range(0, 10).Select(x =>
			{
				var amount = GetRandomAmount();
				expected += amount;
				return CreatePriceAdjustment(amount);
			}).ToList();

			Assert.AreEqual(adjustments.GetTotalAdjustment(), expected);
		}
		#endregion

		#region Helpers
		private Random Random = new Random();

		private ICartAdjustment CreateAdjustment(decimal amount = 0m, int priceKind = -1, string description = null)
		{
			var adjustment = Create.New<ICartAdjustment>();
			adjustment.Amount = amount;
			adjustment.Description = description;
			adjustment.PriceKind = priceKind;
			return adjustment;
		}

		private ICartPriceAdjustment CreatePriceAdjustment(decimal amount = 0m, string description = null)
		{
			var priceAdjustment = Create.New<ICartPriceAdjustment>();
			priceAdjustment.Amount = amount;
			priceAdjustment.Description = description;
			return priceAdjustment;
		}

		private ICartPrice CreatePrice(decimal amount, int priceKind = -1)
		{
			var price = Create.New<ICartPrice>();
			price.Adjustments = new List<ICartPriceAdjustment>();
			price.PriceKind = priceKind;
			price.UnitPrice = amount;
			return price;
		}

		private ICartPrice CreatePriceWithAdjustments(decimal amount, params decimal[] adjustmentAmounts)
		{
			var price = CreatePrice(amount);
			price.Adjustments = adjustmentAmounts.Select(x => CreatePriceAdjustment(x));
			return price;
		}

		private ICartItem CreateItem(uint quantity, IEnumerable<ICartPrice> prices)
		{
			var item = Create.New<ICartItem>();
			item.Prices = prices;
			item.Quantity = quantity;
			return item;
		}

		private ICart CreateCart(IEnumerable<ICartItem> items, params decimal[] adjustmentAmounts)
		{
			var cart = Create.New<ICart>();
			cart.PaidPriceKind = 1;
			cart.VolumePriceKind = 2;
			cart.Adjustments = adjustmentAmounts.Select(x => CreateAdjustment(x, cart.PaidPriceKind));
			cart.Items = items;
			return cart;
		}

		private decimal GetRandomAmount(int maxValue = 50)
		{
			return (decimal)(Random.Next(maxValue - 1) + Random.NextDouble()) * (Random.Next(0, 1) == 0 ? 1 : -1);
		}
		#endregion
	}
}
