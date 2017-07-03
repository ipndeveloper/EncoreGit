using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IProductPropertyTypeRepository : ISearchRepository<FilterPaginatedListParameters<ProductPropertyType>, PaginatedList<ProductPropertyType>>
	{
	}
}
