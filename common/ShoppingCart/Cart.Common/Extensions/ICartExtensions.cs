using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Extensions
{
	public static class ICartExtensions
	{
		public static decimal GetSubtotal(this ICart cart, int priceKind = -1, bool adjusted = true)
		{
			Contract.Requires<ArgumentNullException>(cart != null);
			Contract.Requires<ArgumentNullException>(cart.Items != null);
			Contract.Requires<ArgumentNullException>(!adjusted || cart.Adjustments != null);

			if (priceKind == -1)
			{
				priceKind = cart.PaidPriceKind;
			}

			var adjustmentAmount = adjusted ? cart.Adjustments.GetTotalAdjustment(priceKind) : 0m;
			return cart.Items.GetSubtotal(priceKind, adjusted) + adjustmentAmount;
		}

		public static decimal GetGrandTotal(this ICart cart, int priceKind = -1, bool adjusted = true)
		{
			Contract.Requires<ArgumentNullException>(cart != null);
			Contract.Requires<ArgumentNullException>(cart.Items != null);
			Contract.Requires<ArgumentNullException>(!adjusted || cart.Adjustments != null);

			var grandTotal = cart.GetSubtotal(priceKind, adjusted);
			// grandTotal += (tax amount + shipping amount)

			return grandTotal;
		}
	}
}
