using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class OptOutBusinessLogic
    {
        public virtual OptOut Search(Repositories.IOptOutRepository repository, string emailAddress)
        {
            try
            {
                return repository.Search(emailAddress);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
