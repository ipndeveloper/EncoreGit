using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Exceptions;

namespace NetSteps.ReturnOrderExport
{
    [ServiceContract(Namespace = "http://returnorderexport.michestaging.com", Name = "ReturnOrderExportService")]
    public interface IReturnOrderExport
    {
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
