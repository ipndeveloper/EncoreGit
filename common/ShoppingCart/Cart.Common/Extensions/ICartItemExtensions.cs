using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Extensions
{
	public static class ICartItemExtensions
	{
		public static decimal GetSubtotal(this ICartItem item, int priceKind, bool adjusted = true)
		{
			Contract.Requires(item != null);
			Contract.Requires(item.Prices != null);
			Contract.Requires(item.Quantity > 0);

			return item.GetUnitPrice(priceKind, adjusted) * item.Quantity;
		}

		public static decimal GetUnitPrice(this ICartItem item, int priceKind, bool adjusted = true)
		{
			Contract.Requires(item != null);
			Contract.Requires(item.Prices != null);

			return item.Prices.GetUnitPrice(priceKind, adjusted);
		}

		public static decimal GetSubtotal(this IEnumerable<ICartItem> items, int priceKind, bool adjusted = true)
		{
			Contract.Requires(items != null);

			return items.Sum(x => x.GetSubtotal(priceKind, adjusted));
		}
	}
}
