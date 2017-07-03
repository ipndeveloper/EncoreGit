using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using NetSteps.ProxyIntegrations.Proxy_Classes;

namespace NetSteps.ProxyIntegrationsService.Data
{
    public static class ProxyRepository
    {
        public static string GetAccounts(string userName, string password)
        {
            AccountExportProxy prox = new AccountExportProxy();
            return prox.GetAccounts(userName, password);
        }

        public static string GetOrders(string userName, string password)
        {
            OrderExportProxy prox = new OrderExportProxy();
            return prox.GetOrders(userName, password);
        }

        public static string GetReturnOrders(string userName, string password)
        {
            ReturnOrderExportProxy prox = new ReturnOrderExportProxy();
            return prox.GetReturnOrders(userName, password);
        }
    }
}
