using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.Period
{
	/// <summary>
	/// Period repository
	/// </summary>
	public interface IPeriodRepository : IRepository<IPeriod, int>
	{
		/// <summary>
		/// Periods the ids for account.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<int> PeriodIdsForAccount(int accountId);
	}
}
