using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace Cart.Common
{
	[DTO]
	public interface ICartPrice
	{
		int PriceKind { get; set; }
		decimal UnitPrice { get; set; }
		IEnumerable<ICartPriceAdjustment> Adjustments { get; set; }
	}
}
