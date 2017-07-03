using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
    [ServiceContract(Name = "accountAPI")]
    interface IAccount
    {
        [OperationContract]
        [WebGet(UriTemplate = ("locateAccounts/{cep}"), ResponseFormat = WebMessageFormat.Json)]
        string locateAccounts(string cep);
    }
}
