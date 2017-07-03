using System;

namespace NetSteps.Data.Entities
{
    public partial class AutoshipLog
    {
        public static int GetTotalAttemptsInInterval(DateTime date, AutoshipOrder autoshipOrder)
        {
            return BusinessLogic.GetTotalAttemptsInInterval(Repository, date, autoshipOrder);
        }

        public static int GetTotalAttemptsInInterval(DateTime date, int templateOrderID, IntervalType intervalType)
        {
            return BusinessLogic.GetTotalAttemptsInInterval(Repository, date, templateOrderID, intervalType);
        }

        public static int GetTotalAttemptsSinceLastSuccessful(DateTime lastSuccessfulDate, AutoshipOrder autoshipOrder)
        {
            return BusinessLogic.GetTotalAttemptsSinceLastSuccessful(Repository, lastSuccessfulDate, autoshipOrder);
        }

        public static int GetTotalAttemptsSinceLastSuccessful(DateTime lastSuccessfulDate, int templateOrderID)
        {
            return BusinessLogic.GetTotalAttemptsSinceLastSuccessful(Repository, lastSuccessfulDate, templateOrderID);
        }
    }
}
