using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountPolicyRepository
    {
        List<AccountPolicy> LoadByAccountID(int accountID);
    }
}
