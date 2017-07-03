using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSteps.Financials
{
    class FinancialsProxy : ClientBase<IFinancials>, IFinancials
    {
        public string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            return Channel.GetGrossRevenue(userName, password, fromDate, toDate, CountryISOCode);
        }

        public string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            return Channel.GetShippedRevenue(userName, password, fromDate, toDate, CountryISOCode);
        }
    }
}
