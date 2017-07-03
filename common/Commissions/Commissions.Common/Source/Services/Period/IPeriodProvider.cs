using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.Period
{
	/// <summary>
	/// 
	/// </summary>
	public interface IPeriodProvider : IEnumerable<IPeriod>
	{
		/// <summary>
		/// Gets the periods for account.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IPeriod> GetPeriodsForAccount(int accountId);
	}
}
