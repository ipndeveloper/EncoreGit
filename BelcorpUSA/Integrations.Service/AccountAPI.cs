using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using NetSteps.Data.Entities;
using NetSteps.Integrations.Service.DataModels;
using NetSteps.Integrations.Service.Interfaces;
using Newtonsoft.Json;

namespace NetSteps.Integrations.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(
                        ConcurrencyMode = ConcurrencyMode.Multiple,
                        InstanceContextMode = InstanceContextMode.PerCall,
                        Name = "accountAPI",
                        IncludeExceptionDetailInFaults = true,
                        MaxItemsInObjectGraph = 2147483646
                    )]
    public class AccountAPI : IAccount
    {
        public string locateAccounts(string cep)
        {
            var listAccount = DataAccess.ExecWithStoreProcedureListParam<AccountModel>("Core", "usp_GetAccountsForCEP", new SqlParameter("CEP", SqlDbType.VarChar) { Value = cep });

            var json = JsonConvert.SerializeObject(listAccount);

            return json.ToString();
        }
    }
}
