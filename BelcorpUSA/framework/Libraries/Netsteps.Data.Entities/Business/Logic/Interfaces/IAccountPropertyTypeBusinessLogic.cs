using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAccountPropertyTypeBusinessLogic
    {
        List<AccountPropertyType> LoadAllFullAccountPropertyTypes(IAccountPropertyTypeRepository repository);
    }
}
