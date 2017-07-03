using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ReadyShipperIntegrationService
{
    [ServiceContract(Namespace = "http://readyshipper.itworksstage.net", Name = "ReadyShipperIntegrationService")]
    public interface IReadyShipperIntegration
    {
        //[OperationBehavior(Impersonation = ImpersonationOption.Allowed)]

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string ImportOrdersIntoReadyShipper(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        void ExportOrdersFromReadyShipper(string trueOrderXml, string userName, string password);       
    }
}
