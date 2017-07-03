using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace ParcelPortIntegrationService
{
    [ServiceContract(Name="ParcelPortIntegrationService")]
    public interface IParcelPortIntegrationService
    {
        [OperationContract]
        void SendOrders(String PassKey);

        //[OperationContract]
        string ShipmentPush(XDocument ShipmentPush);
    }
}
