using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.CalculationOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICalculationOverrideProvider : ICache<int, ICalculationOverride>
	{
		ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride);

		bool DeleteCalculationOverride(int calculationOverrideId);
		
		ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters);
	}
}
