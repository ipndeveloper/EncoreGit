using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;

namespace NetSteps.Commissions.Service.Interfaces.CalculationOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICalculationOverrideRepository : IRepository<ICalculationOverride, int>
	{
		ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters);
	}
}
