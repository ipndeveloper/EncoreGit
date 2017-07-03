using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Extensions
{
	public static class ICartAdjustmentExtensions
	{
		public static decimal GetTotalAdjustment(this IEnumerable<ICartAdjustment> adjustments, int priceKind)
		{
			Contract.Requires(adjustments != null);

			return adjustments.Where(a => a.PriceKind == priceKind).Sum(a => a.Amount);
		}
	}
}
