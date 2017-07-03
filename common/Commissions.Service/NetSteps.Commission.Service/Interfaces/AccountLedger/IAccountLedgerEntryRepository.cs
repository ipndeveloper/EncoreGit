using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.AccountLedger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAccountLedgerEntryRepository : IRepository<IAccountLedgerEntry, int>
	{
		/// <summary>
		/// Gets the account ledger entry ids.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<int> GetAccountLedgerEntryIds(int accountId);
	}
}
