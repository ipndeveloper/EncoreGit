using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.AccountLedger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAccountLedgerEntryProvider : ICache<int, IAccountLedgerEntry>
	{
		/// <summary>
		/// Adds the ledger entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		IAccountLedgerEntry AddLedgerEntry(IAccountLedgerEntry entry);

		/// <summary>
		/// Updates the ledger entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		IAccountLedgerEntry UpdateLedgerEntry(IAccountLedgerEntry entry);

		/// <summary>
		/// Deletes the ledger entry.
		/// </summary>
		/// <param name="accountLedgerEntryId">The account ledger entry identifier.</param>
		/// <returns></returns>
		bool DeleteLedgerEntry(int accountLedgerEntryId);

		/// <summary>
		/// Gets the account ledger.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IAccountLedgerEntry> GetAccountLedger(int accountId);
	}
}
