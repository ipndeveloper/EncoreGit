using NetSteps.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Query tooling for account title overrides
	/// </summary>
	public class AccountTitleOverrideSearchParameters : FilterDateRangePaginatedListParameters<IAccountTitleOverride>
	{
		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		public int? AccountId { get; set; }
		/// <summary>
		/// Gets or sets the account title override identifier.
		/// </summary>
		/// <value>
		/// The account title override identifier.
		/// </value>
		public int? AccountTitleOverrideId { get; set; }
		/// <summary>
		/// Gets or sets the override reason identifier.
		/// </summary>
		/// <value>
		/// The override reason identifier.
		/// </value>
		public int? OverrideReasonId { get; set; }
	}
}
