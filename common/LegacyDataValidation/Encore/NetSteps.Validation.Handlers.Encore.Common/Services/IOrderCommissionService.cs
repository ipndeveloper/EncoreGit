using NetSteps.Validation.BatchProcess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Common.Services
{
    public interface IOrderCommissionService : IDependentDataService
    {
        bool GetStoredOrderCommission(int orderID, out decimal commissionTotal);
    }
}
