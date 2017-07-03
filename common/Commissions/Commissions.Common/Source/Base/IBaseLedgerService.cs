using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;

namespace NetSteps.Commissions.Common.Base
{
	/// <summary>
	/// Base ledger service - not intended for 
	/// </summary>
	public interface IBaseLedgerService<TLedgerEntry> where TLedgerEntry : IBaseLedgerEntry
	{
		/// <summary>
		/// Gets the current balance.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		decimal GetCurrentBalance(int accountId);

        /// <summary>
        /// Gets the current balance for the requested kind
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="entryKind"></param>
        /// <returns></returns>
	    decimal GetCurrentBalance(int accountId, ILedgerEntryKind entryKind);

		/// <summary>
		/// Retrieves the ledger.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<TLedgerEntry> RetrieveLedger(int accountId);

		/// <summary>
		/// Updates the ledger entry.
		/// </summary>
		/// <param name="ledgerEntry">The ledger entry.</param>
		TLedgerEntry UpdateLedgerEntry(TLedgerEntry ledgerEntry);

		/// <summary>
		/// Adds the ledger entry.
		/// </summary>
		/// <param name="ledgerEntry">The ledger entry.</param>
		TLedgerEntry AddLedgerEntry(TLedgerEntry ledgerEntry);

        /// <summary>
        /// Gets the entry reason.
        /// </summary>
        /// <param name="entryReasonId">The entry reason identifier.</param>
        /// <returns></returns>
        ILedgerEntryReason GetEntryReason(int entryReasonId);

        /// <summary>
        /// Gets the entry reason.
        /// </summary>
        /// <param name="entryReasonCode">The entry reason code.</param>
        /// <returns></returns>
        ILedgerEntryReason GetEntryReason(string entryReasonCode);

		/// <summary>
		/// Gets the kind of the entry.
		/// </summary>
		/// <param name="entryKindId">The entry kind identifier.</param>
		/// <returns></returns>
		ILedgerEntryKind GetEntryKind(int entryKindId);

		/// <summary>
		/// Gets the kind of the entry.
		/// </summary>
		/// <param name="entryCode">The entry code.</param>
		/// <returns></returns>
		ILedgerEntryKind GetEntryKind(string entryCode);

		/// <summary>
		/// Gets the entry origin.
		/// </summary>
		/// <param name="entryOriginId">The entry origin identifier.</param>
		/// <returns></returns>
		ILedgerEntryOrigin GetEntryOrigin(int entryOriginId);

		/// <summary>
		/// Gets all entry origins.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryOrigin> GetEntryOrigins();

		/// <summary>
		/// Gets the entry kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryKind> GetEntryKinds();

		/// <summary>
		/// Gets the entry reasons.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryReason> GetEntryReasons();

		/// <summary>
		/// Adds a ledger entry.  This method formerly contained an address and a value for the "current balance", which
		/// I thought was stupid as we don't tell the ledger what its balance is, the ledger tells us.  So I've removed them.
		/// Hopefully that doesn't cause a problem.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="entryAmount">The entry amount.</param>
		/// <param name="effectiveDateUtc">The effective date UTC.</param>
		/// <param name="entryDescription">The entry description.</param>
		/// <param name="entryReason">The entry reason.</param>
		/// <param name="entryType">Type of the entry.</param>
		/// <param name="bonusType">Type of the bonus.</param>
		/// <param name="notes">The notes.</param>
		/// <param name="currencyTypeId">The currency type identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		TLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUtc, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId);
	}

	//[ContractClassFor(typeof(IBaseLedgerService))]
	//public abstract class IBaseLedgerServiceContracts : IBaseLedgerService

}
