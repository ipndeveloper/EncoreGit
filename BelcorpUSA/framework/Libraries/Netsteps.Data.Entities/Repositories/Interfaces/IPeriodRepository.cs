using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPeriodRepository
    {
        List<int> GetPeriodIds(int accountID);
        Period GetLastClosedPeriod(int planID);
    }
}
