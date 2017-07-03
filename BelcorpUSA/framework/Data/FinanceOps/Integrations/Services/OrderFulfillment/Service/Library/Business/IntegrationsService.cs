using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using log4net;
using NetstepsDataAccess.DataEntities;
using NetstepsDataAccess.Security;
using System.Configuration;
using NetSteps.Integrations.Service.Business;

namespace NetSteps.Integrations.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class IntegrationsService : IIntegrationsService
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(IntegrationsService));

        public IntegrationsService()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region Disbursements API

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// disbursement data given a certain period
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: WarehouseProduct, SKU and warehouse ID, and a success or failure message indicating whether the update was successful.
        public string GetDisbursements(string userName, string password, int periodID)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetDisbursements", logger, db, GetTimeoutDurationFor("GetDisbursements")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic
                        timer.Message("Entering Repository Method Call");
                        return Repository.GetDisbursements(periodID);
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }
        #endregion

        #region Inventory API

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// the success or failure of an inventory update
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: WarehouseProduct, SKU and warehouse ID, and a success or failure message indicating whether the update was successful.
        public string UpdateInventory(string userName, string password, string data)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("UpdateInventory", logger, db, GetTimeoutDurationFor("UpdateInventory")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic
                        timer.Message("Entering Repository Method Call");
                        return Repository.UpdateInventory(data);
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }


        #endregion

        #region GetOrdersForERP API
        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// order to export for a given system.
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: Customer ID, Date, Dept, Sales, Warehouse, Items, Qty
        public string GetOrdersForERP(string userName, string password, string data)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetOrdersForERP", logger, db, GetTimeoutDurationFor("GetOrdersForERP")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic
                        timer.Message("Entering Repository Method Call");

                        List<string> orderNumbers = new List<string>();
                        orderNumbers = data.Split('|').ToList();

                        return Repository.GetOrdersForERP(orderNumbers);
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        #endregion

        #region GetAccountsForERP API
        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// accounts to export for a given system.
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns>An XML with the following fields: FirstName, LastName, Account Number, Address on Record, Email Address, Home Phone, Total Earning for 1099
        public string GetAccountsForERP(string userName, string password, string startDate, string endDate)
        {
            log4net.Config.XmlConfigurator.Configure();

            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetAccountsForERP", logger, db, GetTimeoutDurationFor("GetAccountsForERP")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic
                        timer.Message("Entering repository call");
                        return Repository.GetAccountsForERP(DateTime.Parse(startDate), DateTime.Parse(endDate));
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        #endregion

        #region OrderFulfillment API
        /// <summary>
        /// Returns a string object containing the text of a netsteps order xml to import into a third party logistics provider
        /// </summary>
        /// <returns>string of netsteps order fulfillment xml</returns>
        public string GetOrdersToFulfill(string userName, string password)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetOrdersToFulfill", logger, db, GetTimeoutDurationFor("GetOrdersToFulfill")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        timer.Message("Entering repository call.");

                        Fulfillment.OrderCollection collect = new Fulfillment.OrderCollection();
                        collect.Order = Repository.GetOrdersToFulfill();

                        string xml = collect.Serialize();

                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert("GetOrdersToFulfill called with the following xml returned: " + xml);
                        }

                        return xml;
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in a string containing the data elements of an Order ID and status to update the database with order status information
        /// and to confirm the successful receipt of orders to fulfill. Returns string of success or failure message.
        /// </summary>
        /// <param name="xml">string of xml data of netsteps EncoreOrderFulfillment xml</param>
        /// <returns>string</returns>
        public string SendOrderFulfillmentAcknowledgment(string userName, string password, string data)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("SendOrderFulfillmentAcknowledgment", logger, db, GetTimeoutDurationFor("SendOrderFulfillmentAcknowledgment")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert("SendOrderFulfillmentAcknowledgment called with the following data passed in: " + data);
                        }

                        timer.Message("Entering repository call.");

                        string xmlToReturn = OrderUpdateRepository.UpdateOrderAcknowledgement(data, db);
                        timer.AddExitMessage(string.Format("SendOrderFulfillmentAcknowledgment exiting successfully with response: {0}", xmlToReturn));
                        return xmlToReturn;
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in a string containing the text of a netsteps EncoreOrderShippingInfomration xml to update the database with order shipment information. Returns string of success or failure message.
        /// </summary>
        /// <param name="xml">string of xml data of netsteps EncoreOrderShippingInformation xml</param>
        /// <returns>string</returns>
        public string SendOrderShippingInformation(string userName, string password, string data)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("SendOrderShippingInformation", logger, db, GetTimeoutDurationFor("SendOrderShippingInformation")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();
                        
                        if (IntegrationsSecurity.IsInDebugMode())
                            db.UspLogisticsCommunicationInsert("SendOrderShippingInformation called with the following data passed in: " + data);

                        string stringToReturn = OrderUpdateRepository.OrderShipmentsUpdate(data);
                        return stringToReturn;
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        #endregion

        #region Financials API

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
        public string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetGrossRevenue", logger, db, GetTimeoutDurationFor("GetGrossRevenue")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic
                        string xml = Repository.GetGrossRevenue(fromDate, toDate, CountryISOCode);
                        return xml;
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

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
        public string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                using (LoggingTimer timer = new LoggingTimer("GetShippedRevenue", logger, db, GetTimeoutDurationFor("GetShippedRevenue")))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetInvalidLoginMessage();

                        // business logic 
                        string xml = Repository.GetShippedRevenue(fromDate, toDate, CountryISOCode);
                        return xml;
                    }
                    catch (Exception ex)
                    {
                        timer.Error(GetInternalErrorMessage(ex));
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        #endregion

        int GetTimeoutDurationFor(string name)
        {
            string setting = string.Format("{0}TimeoutWarning", name);
            if (ConfigurationManager.AppSettings[setting] != null)
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[setting]);
            }
            else
            {
                logger.Warn(string.Format("missing setting for {0}, using generic setting", setting));
                return Convert.ToInt32(ConfigurationManager.AppSettings["GeneralTimeoutWarning"]);
            }
        }

        string GetInvalidLoginMessage()
        {
            string message = "An unsuccessful login attempt was made";
            logger.Warn(message);
            return message;
        }

        string GetInternalErrorMessage(Exception ex)
        {
            string message = string.Format("Exception Encountered: {0} {1}", ex.Message, (ex.InnerException != null) ? string.Format("Inner Exception: {0}", ex.InnerException.Message) : string.Empty);
            return message;
        }

        string GetExternalErrorMessage(Exception ex)
        {
            return "There was an error encountered: " + ex.Message;
        }
    }
}
