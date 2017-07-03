using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.Period
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
