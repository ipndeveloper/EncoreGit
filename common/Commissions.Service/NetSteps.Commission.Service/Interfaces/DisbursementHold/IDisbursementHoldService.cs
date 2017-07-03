using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementHold
{
	/// <summary>
	/// 
	/// </summary>
    public interface IDisbursementHoldService
	{
		/// <summary>
		/// Searches the Disbursement holds.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
        IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters);

		/// <summary>
		/// Gets the Disbursement hold.
		/// </summary>
        /// <param name="disbursementHoldId">The Disbursement hold identifier.</param>
		/// <returns></returns>
        IDisbursementHold GetDisbursementHold(int disbursementHoldId);

		/// <summary>
		/// Deletes the Disbursement hold.
		/// </summary>
        /// <param name="disbursementHoldId">The Disbursement hold identifier.</param>
		/// <returns></returns>
		bool DeleteDisbursementHold(int disbursementHoldId);

		/// <summary>
		/// Adds the Disbursement hold.
		/// </summary>
        /// <param name="disbursementHold">The Disbursement hold.</param>
		/// <returns></returns>
        IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold);
	}
}
