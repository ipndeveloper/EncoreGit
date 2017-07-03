using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using NetSteps.ReturnOrderExport;

namespace NetSteps.ProxyIntegrations.Proxy_Classes
{
    public class ReturnOrderExportProxy : ClientBase<IReturnOrderExport>, IReturnOrderExport
    {
        public string GetReturnOrders(string userName, string password)
        {
            return Channel.GetReturnOrders(userName, password);
        }
    }
}
