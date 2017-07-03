using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPolicyRepository
    {
        List<Policy> GetPolicies(int accountTypeID, int marketID);
    }
}
