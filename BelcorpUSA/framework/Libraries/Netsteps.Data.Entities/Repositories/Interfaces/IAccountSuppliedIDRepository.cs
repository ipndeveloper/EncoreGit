using System.Collections.Generic;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountSuppliedIDRepository
    {
        AccountSuppliedID GetByAccountID(int AccountID, string CFP);
    }
}
