using NetSteps.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Query tooling for check holds.
	/// </summary>
	public class DisbursementHoldSearchParameters : FilterDateRangePaginatedListParameters<IDisbursementHold>
	{
		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		public int? AccountId { get; set; }
		/// <summary>
		/// Gets or sets the check hold identifier.
		/// </summary>
		/// <value>
		/// The check hold identifier.
		/// </value>
		public int? CheckHoldId { get; set; }
		/// <summary>
		/// Gets or sets the reason identifier.
		/// </summary>
		/// <value>
		/// The reason identifier.
		/// </value>
		public int? ReasonId { get; set; }
	}
}
