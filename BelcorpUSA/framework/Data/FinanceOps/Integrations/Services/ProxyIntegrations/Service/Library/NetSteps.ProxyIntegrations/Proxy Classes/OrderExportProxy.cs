using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using NetSteps.Base.Integrations;

namespace NetSteps.ProxyIntegrations.Proxy_Classes
{
    public class OrderExportProxy : ClientBase<IBaseIntegrations>, IBaseIntegrations
    {
        public string GetOrders(string userName, string password)
        {
            return Channel.GetOrders(userName, password);
        }

        public string GetAccounts(string userName, string password)
        {
            return Channel.GetAccounts(userName, password);
        }
    }
}
