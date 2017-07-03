using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.Period
{
	/// <summary>
	/// Period service
	/// </summary>
	public interface IPeriodService
	{
		/// <summary>
		/// Gets the periods.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		IEnumerable<IPeriod> GetPeriods(Predicate<IPeriod> filter);
		/// <summary>
		/// Gets the period.
		/// </summary>
		/// <param name="periodID">The period identifier.</param>
		/// <returns></returns>
		IPeriod GetPeriod(int periodID);
		/// <summary>
		/// Gets the current periods.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IPeriod> GetCurrentPeriods();
		/// <summary>
		/// Gets the current period.
		/// </summary>
		/// <param name="disbursementFrequency">The disbursement frequency.</param>
		/// <returns></returns>
		IPeriod GetCurrentPeriod(DisbursementFrequencyKind disbursementFrequency);
		/// <summary>
		/// Gets the open periods.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IPeriod> GetOpenPeriods();
		/// <summary>
		/// Gets the open periods.
		/// </summary>
		/// <param name="disbursementFrequency">The disbursement frequency.</param>
		/// <returns></returns>
		IEnumerable<IPeriod> GetOpenPeriods(DisbursementFrequencyKind disbursementFrequency);

		/// <summary>
		/// Gets the periods for account.
		/// </summary>
		/// <param name="accountID">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IPeriod> GetPeriodsForAccount(int accountID);
	}
}
