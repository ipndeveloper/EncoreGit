using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IMarketRepository
    {
        List<Market> LoadAllBySiteIDAndUserID(int siteID, int userID);
    }
}
