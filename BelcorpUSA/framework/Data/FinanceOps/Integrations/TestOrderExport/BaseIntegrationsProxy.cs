using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using NetSteps.Base.Integrations;

namespace NetSteps.Base.Integrations
{
    public class BaseIntegrationsProxy : ClientBase<IBaseIntegrations>, IBaseIntegrations
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
