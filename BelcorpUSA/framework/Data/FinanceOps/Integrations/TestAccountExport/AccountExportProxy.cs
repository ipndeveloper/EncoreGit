using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using NetSteps.AccountExport;

namespace NetSteps.AccountExport
{
    public class AccountExportProxy : ClientBase<IAccountExport>, IAccountExport
    {
        public string GetAccounts(string userName, string password)
        {
            return Channel.GetAccounts(userName, password);
        }
    }
}
