using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Order.Common.Models
{
	/// <summary>
	/// Load Order View Model
	/// </summary>
	public class LoadOrderModel
	{
		/// <summary>
		/// Account to load orders from
		/// </summary>
		public int AccountID { get; set; }
		/// <summary>
		/// Number of Orders to load
		/// </summary>
		public int? NumberOfRecords { get; set; }
		/// <summary>
		/// Load Orders >= to this date
		/// </summary>
		public DateTime OrderDate { get; set; }
	}
}
