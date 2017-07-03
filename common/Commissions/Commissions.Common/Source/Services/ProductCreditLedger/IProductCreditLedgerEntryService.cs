using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.ProductCreditLedger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProductCreditLedgerEntryService
	{
		/// <summary>
		/// Gets the product credit ledger entry.
		/// </summary>
		/// <param name="ProductCreditLedgerId">The product credit ledger identifier.</param>
		/// <returns></returns>
		IProductCreditLedgerEntry GetProductCreditLedgerEntry(int ProductCreditLedgerId);

		/// <summary>
		/// Gets the product credit ledger.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IProductCreditLedgerEntry> GetProductCreditLedger(int accountId);

		/// <summary>
		/// Adds the ledger entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		IProductCreditLedgerEntry AddLedgerEntry(IProductCreditLedgerEntry entry);

		/// <summary>
		/// Updates the ledger entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		IProductCreditLedgerEntry UpdateLedgerEntry(IProductCreditLedgerEntry entry);

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
		IProductCreditLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUTC, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId);
	}
}
