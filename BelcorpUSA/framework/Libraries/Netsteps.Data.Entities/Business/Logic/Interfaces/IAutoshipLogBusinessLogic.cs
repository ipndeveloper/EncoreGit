using System;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAutoshipLogBusinessLogic
    {
        int GetTotalAttemptsInInterval(IAutoshipLogRepository repository, DateTime date, AutoshipOrder autoshipOrder);
        int GetTotalAttemptsInInterval(IAutoshipLogRepository repository, DateTime date, int templateOrderID, IntervalType intervalType);
        int GetTotalAttemptsSinceLastSuccessful(IAutoshipLogRepository repository, DateTime lastSuccessfulDate, AutoshipOrder autoshipOrder);
        int GetTotalAttemptsSinceLastSuccessful(IAutoshipLogRepository repository, DateTime lastSuccessfulDate, int templateOrderID);
    }
}
