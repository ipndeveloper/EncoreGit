using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface INavigationRepository
	{
		List<Navigation> LoadSingleLevelNav(int siteId, int navigationTypeId, int? parentId, bool showAll);
	}
}
