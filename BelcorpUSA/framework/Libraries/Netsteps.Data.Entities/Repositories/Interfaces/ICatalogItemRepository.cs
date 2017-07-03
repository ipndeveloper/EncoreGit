using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICatalogItemRepository : ISearchRepository<FilterPaginatedListParameters<CatalogItem>, PaginatedList<CatalogItem>>
	{
	}
}
