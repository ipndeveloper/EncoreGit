using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace Cart.Common
{
	[DTO]
	public interface ICartItem
	{
		string Guid { get; set; }
		string Sku { get; set; }
		uint Quantity { get; set; }
		string Description { get; set; }
		IEnumerable<ICartKitItem> KitItems { get; set; }
		IEnumerable<ICartPrice> Prices { get; set; }
	}
}
