using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Exceptions;

namespace NetSteps.Integrations.Service
{
    [ServiceContract(Namespace = "http://baseintegrations.michelive.com", Name = "IntegrationsService")]
    public interface IOrderFulfillmentService
    {
        //[OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetOrdersToFulfill(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string SendOrderFulfillmentAcknowledgment(string userName, string password, string data);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string SendOrderShippingInformation(string userName, string password, string xml);

        
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetAccounts(string userName, string password);

        
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetOrdersForERP(string userName, string password, string xml);

        
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string UpdateInventory(string userName, string password, string xml);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetDisbursements(string userName, string password, int periodID);
    }
}
