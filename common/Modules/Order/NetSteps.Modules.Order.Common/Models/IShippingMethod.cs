using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common.Models
{
	/// <summary>
	/// Shipping Method DTO
	/// </summary>
	[DTO]
	public interface IShippingMethod
	{
		/// <summary>
		/// ShippingMethodID
		/// </summary>
		int ShippingMethodID { get; set; }
		/// <summary>
		/// Name
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// ShippingRate
		/// </summary>
		decimal ShippingRate { get; set; }
	}
}
