using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace Cart.Common
{
	[DTO]
	public interface ICart
	{
		string Guid { get; set; }
		int PaidPriceKind { get; set; }
		int VolumePriceKind { get; set; }
		IEnumerable<ICartItem> Items { get; set; }
		IEnumerable<ICartAdjustment> Adjustments { get; set; }
	}
}
