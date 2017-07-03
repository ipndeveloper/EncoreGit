using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICorporateUserRepository : ISearchRepository<CorporateUserSearchParameters, PaginatedList<CorporateUserSearchData>>
	{
		CorporateUser LoadByUserIdFull(int userID);
		void GrantSiteAccess(CorporateUser corporateUser, int siteID);
		void RevokeSiteAccess(CorporateUser corporateUser, int siteID);
	}
}
