using System;
using System.ServiceModel;

namespace NetSteps.Integrations.Service
{
    [ServiceContract(Namespace = "http://baseintegrations.michestaging.com", Name = "IntegrationsService")]
    public interface IIntegrationsService
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
        string SendOrderShippingInformation(string userName, string password, string data);


        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetAccountsForERP(string userName, string password, string startDate, string endDate);


        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetOrdersForERP(string userName, string password, string data);


        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string UpdateInventory(string userName, string password, string data);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetDisbursements(string userName, string password, int periodID);
    }
}
