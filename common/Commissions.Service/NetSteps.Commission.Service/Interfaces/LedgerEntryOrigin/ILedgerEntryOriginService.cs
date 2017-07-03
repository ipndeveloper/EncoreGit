using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin
{
	/// <summary>
	/// 
	/// </summary>
	public interface ILedgerEntryOriginService
	{
		/// <summary>
		/// Gets the ledger entry origin.
		/// </summary>
		/// <param name="ledgerEntryOriginId">The ledger entry origin identifier.</param>
		/// <returns></returns>
		ILedgerEntryOrigin GetLedgerEntryOrigin(int ledgerEntryOriginId);

		/// <summary>
		/// Gets the ledger entry origins.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ILedgerEntryOrigin> GetLedgerEntryOrigins();
	}
}
