using System;
using System.ServiceModel;
using NetSteps.Exceptions;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
    [ServiceContract(Name = "IntegrationsService")]
    public interface IIntegrationsService
    {
        [OperationContract]
        [FaultContract(typeof(APIFault))]
        string GetOrdersToFulfill(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(APIFault))]
        string SendOrderFulfillmentAcknowledgment(string userName, string password, string data);

        [OperationContract]
        [FaultContract(typeof(APIFault))]
        string SendOrderShippingInformation(string userName, string password, string data);

        // 01/22/2013 - removed methods that are not needed for the Beauty Counter implementation.
        /*[OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);*/

        

        /*[OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetDisbursements(string userName, string password, int periodID);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string SendDisbursementsAcknowledgment(string userName, string password, string data);*/

        /// <summary>
        /// Adds a product to the product catalog.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="products">The products.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(APIFault))]
        ProductModel AddProduct(string userName, string password, addProductItemCollection products);

        [OperationContract]
        [FaultContract(typeof(APIFault))]
        UpdateInventoryItemModelResponseCollection UpdateInventory(string userName, string password, UpdateInventoryItemModelCollection inventoryItems);
    }
}
