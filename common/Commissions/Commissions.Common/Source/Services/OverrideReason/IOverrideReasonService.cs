using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.OverrideReason
{
	/// <summary>
	/// 
	/// </summary>
	public interface IOverrideReasonService
	{
		/// <summary>
		/// Gets the override reasons.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IOverrideReason> GetOverrideReasons();

		/// <summary>
		/// Gets the override reasons for source.
		/// </summary>
		/// <param name="overrideReasonSourceId">The override reason source identifier.</param>
		/// <returns></returns>
		IEnumerable<IOverrideReason> GetOverrideReasonsForSource(int overrideReasonSourceId);
	}
}
