using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IProductBaseRepository : ISearchRepository<ProductBaseSearchParameters, PaginatedList<ProductBaseSearchData>>
	{
		void ChangeActiveStatus(int productBaseID, bool active);
	}
}
