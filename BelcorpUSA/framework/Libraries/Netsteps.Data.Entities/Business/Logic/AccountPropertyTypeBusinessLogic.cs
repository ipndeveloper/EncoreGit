using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class AccountPropertyTypeBusinessLogic
    {
        public List<AccountPropertyType> LoadAllFullAccountPropertyTypes(IAccountPropertyTypeRepository repository)
        {
            try
            {
                return repository.LoadAllFullAccountPropertyType();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
