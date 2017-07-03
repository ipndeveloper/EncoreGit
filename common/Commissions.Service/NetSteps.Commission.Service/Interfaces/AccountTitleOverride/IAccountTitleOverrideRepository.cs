using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;

namespace NetSteps.Commissions.Service.Interfaces.AccountTitleOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAccountTitleOverrideRepository : IRepository<IAccountTitleOverride, int>
	{
		IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters);
	}
}
