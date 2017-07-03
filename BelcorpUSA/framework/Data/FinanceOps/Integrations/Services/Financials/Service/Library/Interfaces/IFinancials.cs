using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Exceptions;

namespace NetSteps.Financials
{
    [ServiceContract(Namespace = "http://baseintegrations.michestaging.com", Name = "FinancialsService")]
    public interface IFinancials
    {
        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// unearned income data given a date range and country
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <param name="fromDate">the beginning date range for the data</param>
        /// <param name="toDate">the ending date range for the data</param>
        /// <param name="CountryISOCode">the three-character country ISO code for the data</param>
        /// <returns>An XML with the following fields: Cash $ amount, ProductCredit $ amount, 
        ///          Gift Card $ amount,Service Income $ amount, 
        ///          Sales Tax $ amount</returns>
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// earned income data given a date range and country
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <param name="fromDate">the beginning date range for the data</param>
        /// <param name="toDate">the ending date range for the data</param>
        /// <param name="CountryISOCode">the three-character country ISO code for the data</param>
        /// <returns>An XML with the following fields: SKU text field, Quantity Shipped integer, 
        /// Retail Price $ amount, Wholesale Price $ amount, Adjusted Price (Actual Cost) $ amount,
        /// Shipping Cost $ amount, Currency Code enumeration</returns>
        [OperationContract]
        [FaultContract(typeof(FaultDetail))]
        string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode);
    }
}
