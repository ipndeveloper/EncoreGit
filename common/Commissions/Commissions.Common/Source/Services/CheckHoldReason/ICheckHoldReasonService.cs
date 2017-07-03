using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.CheckHoldReason
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICheckHoldReasonService
	{
		/// <summary>
		/// Gets the check hold reason.
		/// </summary>
		/// <param name="checkHoldReasonId">The check hold reason identifier.</param>
		/// <returns></returns>
		ICheckHoldReason GetCheckHoldReason(int checkHoldReasonId);

		/// <summary>
		/// Gets the check hold reasons.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ICheckHoldReason> GetCheckHoldReasons();
	}
}
