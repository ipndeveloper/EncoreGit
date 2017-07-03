using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Extensions
{
	public static class ICartPriceExtensions
	{
		public static decimal GetUnitPrice(this ICartPrice price, bool adjusted = true)
		{
			Contract.Requires(price != null);
			Contract.Requires(!adjusted || price.Adjustments != null);

			var adjustmentAmount = adjusted ? price.Adjustments.GetTotalAdjustment() : 0m;
			return price.UnitPrice + adjustmentAmount;
		}

		public static decimal GetUnitPrice(this IEnumerable<ICartPrice> prices, int priceKind, bool adjusted = true)
		{
			Contract.Requires(prices != null);

			return prices.Where(p => p.PriceKind == priceKind).Sum(p => p.GetUnitPrice(adjusted));
		}
	}
}
