using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace NetSteps.Integrations.Service
{
    [ServiceContract(Namespace = "http://baseintegrations.michestaging.com", Name = "IntegrationsService")]
    public interface IOrderFulfillmentService
    {
        //[OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetOrdersToFulfill(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string SendOrderFulfillmentAcknowledgment(string userName, string password, string xml);

        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string SendOrderShippingInformation(string userName, string password, string xml);

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// accounts to export for a given system.
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: FirstName, LastName, Account Number, Address on Record, Email Address, Home Phone, Total Earning for 1099
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetAccounts(string userName, string password);

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// order to export for a given system.
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: Customer ID, Date, Dept, Sales, Warehouse, Items, Qty
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetOrders(string userName, string password);
    }
}
