using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Base.Integrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BaseIntegrationsProxy prox = new BaseIntegrationsProxy();
            string user = ConfigurationManager.AppSettings["ClientUserName"];
            string pass = ConfigurationManager.AppSettings["ClientPassword"];
            //string s = prox.GetOrders(user, pass);
            string sss = prox.GetAccounts(user, pass);
            BaseIntegrationsProxy prox2 = new BaseIntegrationsProxy();
            string s2 = prox.GetOrders(user, pass);
            prox2.Close();
        }
    }
}
