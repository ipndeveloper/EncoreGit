using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Order Search Result
	/// </summary>
	[DTO]
	public interface IOrderSearchResult
	{
		/// <summary>
		/// OrderID
		/// </summary>
		int OrderID { get; set; }
		/// <summary>
		/// Order Date
		/// </summary>
		DateTime Date { get; set; }
		/// <summary>
		/// Status
		/// </summary>
		string Status { get; set; }
		/// <summary>
		/// Volume
		/// </summary>
		decimal Volume { get; set; }
		/// <summary>
		/// Total
		/// </summary>
		decimal Total { get; set; }
		/// <summary>
		/// OwnerID
		/// </summary>
		int OwnerID { get; set; }
		/// <summary>
		/// Order Type
		/// </summary>
		int OrderTypeID { get; set; }		
	}
}
