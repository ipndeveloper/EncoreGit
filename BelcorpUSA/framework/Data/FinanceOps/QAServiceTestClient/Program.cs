using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyShipperIntegrationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReadyShipperIntegrationProxy prox = new ReadyShipperIntegrationProxy();
            //prox.ClientCredentials.Windows.ClientCredential.UserName = @"nslindon\jgurney";
            //prox.ClientCredentials.Windows.ClientCredential.Password = "Tornado04188";
            string s = prox.ImportOrdersIntoReadyShipper("readyShipper4592", "r92rgeWEW457");
            prox.ExportOrdersFromReadyShipper("xml", "readyShipper4592", "r92rgeWEW457");
        }
    }
}
