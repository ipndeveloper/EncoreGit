using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountTypeRepository
    {
        List<Role> GetRolesByAccountType(short accountTypeID);
    }
}
