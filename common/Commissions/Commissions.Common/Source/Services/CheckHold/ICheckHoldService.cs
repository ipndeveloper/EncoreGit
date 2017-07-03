using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.CheckHold
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICheckHoldService
	{
		/// <summary>
		/// Searches the check holds.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		IEnumerable<ICheckHold> SearchCheckHolds(CheckHoldSearchParameters parameters);

		/// <summary>
		/// Gets the check hold.
		/// </summary>
		/// <param name="checkHoldId">The check hold identifier.</param>
		/// <returns></returns>
		ICheckHold GetCheckHold(int checkHoldId);

		/// <summary>
		/// Deletes the check hold.
		/// </summary>
		/// <param name="checkHoldId">The check hold identifier.</param>
		/// <returns></returns>
		bool DeleteCheckHold(int checkHoldId);

		/// <summary>
		/// Adds the check hold.
		/// </summary>
		/// <param name="checkHold">The check hold.</param>
		/// <returns></returns>
		ICheckHold AddCheckHold(ICheckHold checkHold);
	}
}
