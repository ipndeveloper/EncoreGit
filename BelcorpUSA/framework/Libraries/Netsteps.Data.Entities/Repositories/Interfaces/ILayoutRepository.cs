using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ILayoutRepository
    {
        List<int> GetLayoutsForSite(int siteID);
    }
}
