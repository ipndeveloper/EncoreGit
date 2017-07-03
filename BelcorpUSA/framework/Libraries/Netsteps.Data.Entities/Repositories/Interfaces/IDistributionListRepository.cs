using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IDistributionListRepository
    {
        List<DistributionList> LoadByAccountID(int accountID);
        List<DistributionList> LoadByAccountIDFull(int accountID);
    }
}
