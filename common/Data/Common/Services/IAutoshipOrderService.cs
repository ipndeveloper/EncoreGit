using System;
using System.Linq;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Services
{
    public interface IAutoshipOrderService
    {
        IAutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID);
    }
}
