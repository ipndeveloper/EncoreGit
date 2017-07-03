using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.CalculationOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICalculationOverrideService
	{
		/// <summary>
		/// Searches the calculation overrides.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters);

		/// <summary>
		/// Gets the calculation override.
		/// </summary>
		/// <param name="calculationOverrideId">The calculation override identifier.</param>
		/// <returns></returns>
		ICalculationOverride GetCalculationOverride(int calculationOverrideId);

		/// <summary>
		/// Deletes the calculation override.
		/// </summary>
		/// <param name="calculationOverrideId">The calculation override identifier.</param>
		/// <returns></returns>
		bool DeleteCalculationOverride(int calculationOverrideId);

		/// <summary>
		/// Adds the calculation override.
		/// </summary>
		/// <param name="calculationOverride">The calculation override.</param>
		/// <returns></returns>
		ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride);

	}
}
