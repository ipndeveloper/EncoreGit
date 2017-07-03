using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ReadyShipperIntegrationService
{
    class ReadyShipperIntegrationProxy : ClientBase<IReadyShipperIntegration>, IReadyShipperIntegration
    {
        public string ImportOrdersIntoReadyShipper(string userName, string password)
        {
            return Channel.ImportOrdersIntoReadyShipper(userName, password);
        }

        public void ExportOrdersFromReadyShipper(string trueOrderXml, string userName, string password)
        {
            Channel.ExportOrdersFromReadyShipper(trueOrderXml, userName, password);
        }
    }
}
