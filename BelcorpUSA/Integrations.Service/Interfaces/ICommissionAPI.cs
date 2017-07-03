using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
    [ServiceContract(Name = "commissionAPI", Namespace = "netSteps.commission")]
    public interface ICommissionAPI
    {
        [OperationContract]
        [FaultContract(typeof(APIFault))]
        DisbursementModel getDisbursement(string userName, string password, int periodID);

    }
}
