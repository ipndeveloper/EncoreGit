using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.AccountLedger
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
