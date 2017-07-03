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
	public interface IAccountLedgerEntryService
	{
		/// <summary>
		/// Gets the ledger entry.
		/// </summary>
		/// <param name="AccountLedgerId">The account ledger identifier.</param>
		/// <returns></returns>
		IAccountLedgerEntry GetLedgerEntry(int AccountLedgerId);

		/// <summary>
		/// Gets the account ledger.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IAccountLedgerEntry> GetAccountLedger(int accountId);

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
		/// Adds the ledger entry.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="entryAmount">The entry amount.</param>
		/// <param name="effectiveDateUTC">The effective date UTC.</param>
		/// <param name="entryDescription">The entry description.</param>
		/// <param name="entryReason">The entry reason.</param>
		/// <param name="entryType">Type of the entry.</param>
		/// <param name="bonusType">Type of the bonus.</param>
		/// <param name="notes">The notes.</param>
		/// <param name="currencyTypeId">The currency type identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		IAccountLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUTC, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId);
	}
}
