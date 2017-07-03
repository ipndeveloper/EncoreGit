using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace NetSteps.AccountExport
{
    [ServiceContract(Namespace = "http://accountexport.michestaging.com", Name = "AccountExportService")]
    public interface IAccountExport
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
    }
}
