using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.ProductCreditLedger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProductCreditLedgerEntryProvider : ICache<int, IProductCreditLedgerEntry>
	{
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
		/// Gets the product credit ledger.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<IProductCreditLedgerEntry> GetProductCreditLedger(int accountId);
	}
}
