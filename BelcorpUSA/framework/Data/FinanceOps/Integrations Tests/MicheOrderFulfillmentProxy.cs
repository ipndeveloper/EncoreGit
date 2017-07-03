using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace NetSteps.Integrations.Service
{
    class MicheOrderFulfillmentProxy : ClientBase<IIntegrationsService>, IIntegrationsService
    {
        public string GetOrdersToFulfill(string userName, string password)
        {
            return Channel.GetOrdersToFulfill(userName, password);
        }

        public string SendOrderFulfillmentAcknowledgment(string userName, string password, string data)
        {
            return Channel.SendOrderFulfillmentAcknowledgment(userName, password, data);
        }

        public string SendOrderShippingInformation(string userName, string password, string data)
        {
            return Channel.SendOrderShippingInformation(userName, password, data);
        }

        public string GetAccountsForERP(string userName, string password, string startDate, string endDate)
        {
            return Channel.GetAccountsForERP(userName, password, startDate, endDate);
        }

        public string GetOrdersForERP(string userName, string password, string data)
        {
            return Channel.GetOrdersForERP(userName, password, data);
        }

        public string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            return Channel.GetGrossRevenue(userName, password, fromDate, toDate, CountryISOCode);
        }

        public string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            return Channel.GetShippedRevenue(userName, password, fromDate, toDate, CountryISOCode);
        }

        public string UpdateInventory(string userName, string password, string data)
        {
            return Channel.UpdateInventory(userName, password, data);
        }

        public string GetDisbursements(string userName, string password, int periodID)
        {
            return Channel.GetDisbursements(userName, password, periodID);
        }
    }
}
