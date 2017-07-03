using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common.Models
{
	/// <summary>
	/// Order Item DTO
	/// </summary>
	[DTO]
	public interface IOrderItem
	{
		/// <summary>
		/// Total
		/// </summary>
		decimal Total { get; set; }
		/// <summary>
		/// SKU
		/// </summary>
		string SKU { get; set; }
		/// <summary>
		/// ProductName
		/// </summary>
		string ProductName { get; set; }
		/// <summary>
		/// Quantity
		/// </summary>
		int Quantity { get; set; }
		/// <summary>
		/// BonusVolume
		/// </summary>
		decimal BonusVolume { get; set; }
	}
}
