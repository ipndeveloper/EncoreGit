using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSteps.ProxyIntegrations
{
    public class ProxyIntegrationsProxy : ClientBase<IProxyIntegrations>, IProxyIntegrations
    {
        public string GetAccounts(string userName, string password)
        {
            return Channel.GetAccounts(userName, password);
        }

        public string GetOrders(string userName, string password)
        {
            return Channel.GetOrders(userName, password);
        }

        public string GetReturnOrders(string userName, string password)
        {
            return Channel.GetReturnOrders(userName, password);
        }
    }
}
