using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IHostessRewardTypeRepository
	{
		List<int> GetAvailableCatalogs(IEnumerable<int> rewardTypes);
	}
}
