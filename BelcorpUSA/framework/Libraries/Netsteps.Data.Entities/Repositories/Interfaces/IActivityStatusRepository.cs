using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.EntityModels;
namespace NetSteps.Data.Entities.Repositories
{
    public interface IActivityStatusRepository
    {
        List<ActivityStatus> GetAll();
    }
}
