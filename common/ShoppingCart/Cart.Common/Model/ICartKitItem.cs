using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace Cart.Common
{
	[DTO]
	public interface ICartKitItem
	{
		uint Quantity { get; set; }
		string Sku { get; set; }
		string Description { get; set; }
	}
}
