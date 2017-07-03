using System;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class CheckHoldBusinessLogic
    {
        public void InsertOrUpdate(ICheckHoldRepository repository, CheckHold checkHold)
        {
            try
            {
                repository.InsertOrUpdate(checkHold);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


    }
}