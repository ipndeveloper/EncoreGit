using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.LedgerEntryKind
{
	/// <summary>
	/// 
	/// </summary>
	public interface ILedgerEntryKindService
	{
		/// <summary>
		/// Gets the kind of the ledger entry.
		/// </summary>
		/// <param name="ledgerEntryKindId">The ledger entry kind identifier.</param>
		/// <returns></returns>
		ILedgerEntryKind GetLedgerEntryKind(int ledgerEntryKindId);

		/// <summary>
		/// Gets the kind of the ledger entry.
		/// </summary>
		/// <param name="ledgerEntryKindCode">The ledger entry kind code.</param>
		/// <returns></returns>
		ILedgerEntryKind GetLedgerEntryKind(string ledgerEntryKindCode);

		/// <summary>
		/// Gets the ledger entry kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryKind> GetLedgerEntryKinds();
	}
}
