using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.LedgerEntryReason
{
	/// <summary>
	/// 
	/// </summary>
	public interface ILedgerEntryReasonService
	{
		/// <summary>
		/// Gets the ledger entry reason.
		/// </summary>
		/// <param name="ledgerEntryReasonId">The ledger entry reason identifier.</param>
		/// <returns></returns>
		ILedgerEntryReason GetLedgerEntryReason(int ledgerEntryReasonId);

		/// <summary>
		/// Gets the ledger entry reasons.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryReason> GetLedgerEntryReasons();
	}
}
