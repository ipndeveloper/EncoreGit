using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Exceptions;

namespace NetSteps.ProxyIntegrations
{
    [ServiceContract(Namespace = "http://integrations.michestaging.com", Name = "ProxyIntegrationsService")]
    public interface IProxyIntegrations
    {
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

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// return orders to export for a given system.
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: return order number, return date and time, Order ID, Customer ID, SKU, Item Number, Item Return Qty,
        /// Item damage Qty, Item Total Qty, Return Reason, Order Status
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetReturnOrders(string userName, string password);
    }
}
