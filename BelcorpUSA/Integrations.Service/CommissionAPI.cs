using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Integrations.Service.Interfaces;
using System.ServiceModel;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, Name = "commissionAPI", IncludeExceptionDetailInFaults = true)]
    public class CommissionAPI : ICommissionAPI
    {
        public DisbursementModel getDisbursement(string userName, string password, int periodID)
        {
            throw new NotImplementedException();
        }
    }
}
