using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace Cart.Common
{
	[DTO]
	public interface ICartPriceAdjustment
	{
		decimal Amount { get; set; }
		string Description { get; set; }
	}
}
