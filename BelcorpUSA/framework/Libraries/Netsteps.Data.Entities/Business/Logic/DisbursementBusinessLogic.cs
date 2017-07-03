using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class DisbursementLogic
    {
        public virtual List<Disbursement> LoadByDisbursementsByTypeAndPeriod(Repositories.IDisbursementRepository repository, int disbursementTypeID, int periodID)
        {
            try
            {
                return repository.LoadDisbursementsByTypeAndPeriod(disbursementTypeID, periodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual List<Disbursement> LoadByDisbursementsByPeriod(Repositories.IDisbursementRepository repository, int periodID)
        {
            try
            {
                return repository.LoadDisbursementsByPeriod(periodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
