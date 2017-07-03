using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.ProxyIntegrations
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyIntegrationsProxy prox = new ProxyIntegrationsProxy();
            string user = ConfigurationManager.AppSettings["ClientUserName"];
            string pass = ConfigurationManager.AppSettings["ClientPassword"];
            
            string s1 = prox.GetOrders(user, pass);
            string s2 = prox.GetOrders(user, pass);
            string s3 = prox.GetOrders(user, pass);
            string s4 = prox.GetOrders(user, pass);
            //string s2 = prox.GetReturnOrders(user, pass);
            //string s = prox.GetAccounts(user, pass);
        }
    }
}
