using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICampaignRepository : ISearchRepository<CampaignSearchParameters, PaginatedList<CampaignSearchData>>
    {
        List<Campaign> LoadFullAllByDomainEventTypeID(int domainEventTypeID);
        List<Campaign> LoadFullAllByCampaignTypeID(short campaignTypeID);
    }
}
