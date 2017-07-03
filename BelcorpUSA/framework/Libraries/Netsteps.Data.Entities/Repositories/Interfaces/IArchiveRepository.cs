using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IArchiveRepository : ISearchRepository<ArchiveSearchParameters, PaginatedList<ArchiveSearchData>>
	{
		List<Archive> GetRecent100();
		List<Archive> LoadAllFullBySiteID(int siteID);
	}
}
