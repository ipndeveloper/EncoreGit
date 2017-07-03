using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Extensions
{
	public static class ICartPriceAdjustmentExtensions
	{
		public static decimal GetTotalAdjustment(this IEnumerable<ICartPriceAdjustment> adjustments)
		{
			Contract.Requires(adjustments != null);

			return adjustments.Sum(a => a.Amount);
		}
	}
}
