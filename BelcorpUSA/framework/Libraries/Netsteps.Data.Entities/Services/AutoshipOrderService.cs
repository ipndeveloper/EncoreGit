using NetSteps.Data.Common.Services;

namespace NetSteps.Data.Entities.Services
{
    public class AutoshipOrderService : IAutoshipOrderService
    {
        public Common.Entities.IAutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID)
        {
            return AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(accountID, autoshipScheduleID);
        }
    }
}
