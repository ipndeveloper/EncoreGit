using System;
using System.ServiceModel;
using System.Configuration;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Integrations.Service.DataModels;
using NetstepsDataAccess.DataEntities;
using NetSteps.Integrations.Internals.Security;
using NetSteps.Integrations.Service.Interfaces;

namespace NetSteps.Integrations.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class IntegrationsService : IIntegrationsService
    {
        #region OrderFulfillment API
        /// <summary>
        /// Returns a string object containing the text of a netsteps order xml to import into a third party logistics provider
        /// </summary>
        /// <returns>string of netsteps order fulfillment xml</returns>
        public string GetOrdersToFulfill(string userName, string password)
        {
            using (var db = new NetStepsEntities())
            {
                using (this.TraceActivity("GetOrdersToFulfill"))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return GetAndLogInvalidLoginMessage();
                        }

                        this.TraceEvent("Entering repository call.");

                        var collect = new Fulfillment.OrderCollection();
                        collect.Order = Repository.GetOrdersToFulfill();

                        var xml = collect.Serialize();

                        #region Post Logging
                        var toLog = string.Format("GetOrdersToFulfill result: {0}", xml);
                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert(toLog);
                        }
                        this.TraceVerbose(toLog);
                        #endregion

                        return xml;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in a string containing the data elements of an Order ID and status to update the database with order status information
        /// and to confirm the successful receipt of orders to fulfill. Returns string of success or failure message.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendOrderFulfillmentAcknowledgment(string userName, string password, string data)
        {
            using (var db = new NetStepsEntities())
            {
                using (this.TraceActivity("SendOrderFulfillmentAcknowledgment"))
                {
                    // data format for input
                    // 105,Printed|106,Printed|
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return GetAndLogInvalidLoginMessage();
                        }

                        #region Pre Logging
                        var preLog = string.Format("SendOrderFulfillmentAcknowledgment called with: {0}", data);
                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert(preLog);
                        }
                        this.TraceVerbose(preLog);
                        #endregion

                        var response = OrderUpdateRepository.UpdateOrderAcknowledgement(data, db);

                        #region Post Logging
                        var postLog = string.Format("SendOrderFulfillmentAcknowledgment result: {0}", response.Message);
                        this.TraceVerbose(postLog);
                        #endregion

                        return response.Message;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in a string containing the text of a netsteps EncoreOrderShippingInfomration xml to update the database with order shipment information. Returns string of success or failure message.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendOrderShippingInformation(string userName, string password, string data)
        {
            using (var db = new NetStepsEntities())
            {
                using (this.TraceActivity("SendOrderShippingInformation"))
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return GetAndLogInvalidLoginMessage();
                        }

                        #region Logging
                        this.TraceVerbose(string.Format("SendOrderShippingInformation called with: {0}", data));
                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert("SendOrderShippingInformation called with the following data passed in: " + data);
                        }
                        #endregion

                        var response = OrderUpdateRepository.OrderShipmentsUpdate(data);

                        #region Logging
                        var toLog = string.Format("SendOrderShippingInformation result: {0}", response.Message);
                        if (!response.Success)
                        {
                            this.TraceError(toLog);
                        }
                        else
                        {
                            this.TraceVerbose(toLog);
                        }
                        #endregion

                        return response.Message;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
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
        /// <param name="data"></param>
        /// <returns>An XML with the following fields: WarehouseProduct, SKU and warehouse ID, and a success or failure message indicating whether the update was successful.</returns>
        public string UpdateInventory(string userName, string password, string data)
        {
            using (var db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                    {
                        return "An unsuccessful login attempt was made";
                    }

                    #region Pre Logging
                    this.TraceVerbose(string.Format("UpdateInventory called with: {0}", data));
                    #endregion

                    var response = Repository.UpdateInventory(data);

                    #region Post Logging
                    var toLog = string.Format("UpdateInventory result: {0}", response.Message);
                    if (!response.Success)
                    {
                        this.TraceError(toLog);
                    }
                    else
                    {
                        this.TraceVerbose(toLog);
                    }
                    #endregion

                    return response.Message;
                }
                catch (Exception ex)
                {
                    ex.TraceException(ex);
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "UpdateInventory", ex.Message);
                    return "There was an error encountered: " + ex.Message;
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
            using (this.TraceActivity("GetGrossRevenue"))
            {
                using (var db = new NetStepsEntities())
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return "An unsuccessful login attempt was made";
                        }

                        // business logic
                        var xml = Repository.GetGrossRevenue(fromDate, toDate, CountryISOCode);
                        return xml;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetGrossRevenue", ex.Message);
                        return "There was an error encountered: " + ex.Message;
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
            using (this.TraceActivity("GetShippedRevenue"))
            {
                using (var db = new NetStepsEntities())
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return "An unsuccessful login attempt was made";
                        }

                        // business logic 
                        var xml = Repository.GetShippedRevenue(fromDate, toDate, CountryISOCode);
                        return xml;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetShippedRevenue", ex.Message);
                        return "There was an error encountered: " + ex.Message;
                    }
                }
            }
        }

        #endregion

        #region Disbursements API

        /// <summary>
        /// Returns a string containing a netsteps xml containing 
        /// disbursement data given a certain period
        /// </summary>
        /// <param name="userName">user name for the client</param>
        /// <param name="password">password for the client</param>
        /// <returns></returns>
        public string GetDisbursements(string userName, string password, int periodID)
        {
            using (this.TraceActivity("GetDisbursements"))
            {
                using (var db = new NetStepsEntities())
                {
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                        {
                            return "An unsuccessful login attempt was made";
                        }

                        // business logic
                        return Repository.GetDisbursements(periodID);

                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetDisbursements",
                            ex.Message);
                        return "There was an error encountered: " + ex.Message;
                    }
                }
            }
        }

        public string SendDisbursementsAcknowledgment(string userName, string password, string data)
        {
            using (this.TraceActivity("SendOrderFulfillmentAcknowledgment"))
            {
                using (var db = new NetStepsEntities())
                {
                    // data format for input
                    // 105,Printed|106,Printed|
                    try
                    {
                        // authenticate
                        if (!IntegrationsSecurity.Authenticate(userName, password))
                            return GetAndLogInvalidLoginMessage();

                        if (IntegrationsSecurity.IsInDebugMode())
                        {
                            db.UspLogisticsCommunicationInsert(
                                "SendDisbursementsAcknowledgment called with the following data passed in: " + data);
                        }

                        this.TraceError("Entering repository call.");

                        var xmlToReturn = string.Empty; //Repository.SendDisbursementsAcknowledgment(data, db);
                        this.TraceEvent(string.Format("SendDisbursementsAcknowledgment exiting successfully with response: {0}", xmlToReturn));
                        return xmlToReturn;
                    }
                    catch (Exception ex)
                    {
                        ex.TraceException(ex);
                        return GetExternalErrorMessage(ex);
                    }
                }
            }
        }


        #endregion

        string GetAndLogInvalidLoginMessage()
        {
            const string message = "An unsuccessful login attempt was made";
            this.TraceError(message);
            return message;
        }

        string GetExternalErrorMessage(Exception ex)
        {
            return "There was an error encountered: " + ex.Message;
        }

        public ProductModel AddProduct(string userName, string password, addProductItemCollection products)
        {
            throw new NotImplementedException();
        }

        public UpdateInventoryItemModelResponseCollection UpdateInventory(string userName, string password, UpdateInventoryItemModelCollection inventoryItems)
        {
            throw new NotImplementedException();
        }
    }
}
