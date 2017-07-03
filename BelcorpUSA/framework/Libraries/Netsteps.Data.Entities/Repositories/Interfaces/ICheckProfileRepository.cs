using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICheckProfileRepository
    {
        CheckProfile LoadByDisbursementProfileID(int disbursementProfileID);
        List<CheckProfile> LoadByAccountID(int accountID);
    }
}
