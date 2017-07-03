using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAutoresponderRepository : ISearchRepository<AutoresponderSearchParameters, PaginatedList<Autoresponder>>
	{
	}
}
