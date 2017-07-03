using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Load Order View Model
	/// </summary>
	[DTO]
	public interface ILoadOrderModel
	{
		/// <summary>
		/// Account to load orders from
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// Number of Orders to load
		/// </summary>
		int? NumberOfRecords { get; set; }
		/// <summary>
		/// Load Orders >= to this date
		/// </summary>
		DateTime OrderDate { get; set; }
	}
}
