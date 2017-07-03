using NetSteps.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Query tooling for calculation overrides
	/// </summary>
	public class CalculationOverrideSearchParameters : FilterDateRangePaginatedListParameters<ICalculationOverride>
	{
		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		public int? AccountId { get; set; }
		/// <summary>
		/// Gets or sets the calculation override identifier.
		/// </summary>
		/// <value>
		/// The calculation override identifier.
		/// </value>
		public int? CalculationOverrideId { get; set; }
		/// <summary>
		/// Gets or sets the override reason identifier.
		/// </summary>
		/// <value>
		/// The override reason identifier.
		/// </value>
		public int? OverrideReasonId { get; set; }
	}
}
