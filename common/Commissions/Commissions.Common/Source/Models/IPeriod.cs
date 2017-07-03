using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commissions period.
	/// </summary>
	public interface IPeriod
	{
		/// <summary>
		/// Gets the period identifier.
		/// </summary>
		/// <value>
		/// The period identifier.
		/// </value>
		int PeriodId { get; }

		/// <summary>
		/// Gets the start date in UTC.
		/// </summary>
		/// <value>
		/// The start date.
		/// </value>
		DateTime StartDateUTC { get; }

		/// <summary>
		/// Gets the end date in UTC.
		/// </summary>
		/// <value>
		/// The end date.
		/// </value>
		DateTime EndDateUTC { get; }

		/// <summary>
		/// Gets the closed date UTC.
		/// </summary>
		/// <value>
		/// The closed date UTC.
		/// </value>
		DateTime? ClosedDateUTC { get; }

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		string Description { get; }

		/// <summary>
		/// Gets a value indicating whether [is open].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is open]; otherwise, <c>false</c>.
		/// </value>
		bool IsOpen { get; }

		/// <summary>
		/// Gets the disbursement plan.
		/// </summary>
		/// <value>
		/// The disbursement plan.
		/// </value>
		DisbursementFrequencyKind DisbursementFrequency{ get; }
	}
}
