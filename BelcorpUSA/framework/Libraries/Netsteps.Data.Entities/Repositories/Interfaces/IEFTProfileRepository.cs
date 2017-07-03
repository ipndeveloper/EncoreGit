using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IEFTProfileRepository
    {
        EFTProfile LoadByDisbursementProfileID(int disbursementProfileID);
        List<EFTProfile> LoadByAccountID(int accountID);
    }
}
