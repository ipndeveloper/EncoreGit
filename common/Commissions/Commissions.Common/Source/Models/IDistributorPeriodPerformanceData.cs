using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Data for the performance widget
	/// </summary>
	public interface IDistributorPeriodPerformanceData
	{
		/// <summary>
		/// Gets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		int AccountId { get; }

		/// <summary>
		/// Gets the period identifier.
		/// </summary>
		/// <value>
		/// The period identifier.
		/// </value>
		int PeriodId { get; }

		/// <summary>
		/// Gets the volume earned for the period and account specified.
		/// </summary>
		/// <value>
		/// The volume.
		/// </value>
		Decimal Volume { get; }

		/// <summary>
		/// Gets the required volume for the period and account specified.
		/// </summary>
		/// <value>
		/// The required volume.
		/// </value>
		Decimal RequiredVolume { get; }

		/// <summary>
		/// Gets the current level title earned by the account for the period specified.
		/// </summary>
		/// <value>
		/// The current level.
		/// </value>
		ITitle CurrentTitle { get; }

		/// <summary>
		/// Gets the title that the distributor will be "paid as" for the period specified.
		/// </summary>
		/// <value>
		/// The paid as level.
		/// </value>
		ITitle PaidAsTitle { get; }

		/// <summary>
		/// Gets the sales indicator level.
		/// </summary>
		/// <value>
		/// The sales indicator level.
		/// </value>
		string SalesIndicatorLevel { get; }
	}
}
