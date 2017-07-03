using System;
using System.Collections.Generic;
namespace NetSteps.Data.Entities.Repositories
{
    public interface IAccountConsistencyStatusRepository
    {
        List<AccountConsistencyStatus> GetAll();
    }
}
