using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;

namespace NetSteps.AccountExport
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountExportProxy prox = new AccountExportProxy();
            string user = ConfigurationManager.AppSettings["ClientUserName"];
            string pass = ConfigurationManager.AppSettings["ClientPassword"];
            string s = prox.GetAccounts(user, pass);
        }
    }
}
