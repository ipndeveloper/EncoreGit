using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using System.ServiceModel;
using System.Runtime.Serialization;
using NetstepsDataAccess.Security;
using log4net;
using NetSteps.Financials.Data;
using NetSteps.Financials.ShippedRevenue;

namespace NetSteps.Financials
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class FinancialsService : IFinancials
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(FinancialsService));

        public FinancialsService() { }

        public string GetGrossRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info("***************************");
            logger.Info("GetGrossRevenue method call started.");
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                        return "An unsuccessful login attempt was made";
                    string xml = "sucessful test xml response";
                    // business logic
                    FinancialsGrossRevenue rev = Repository.GetGrossRevenue(fromDate, toDate, CountryISOCode);
                    xml = rev.Serialize();
                    logger.Info("Exiting the GetGrossRevenue method call");
                    logger.Info("***************************");
                    return xml;
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetGrossRevenue", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }

        public string GetShippedRevenue(string userName, string password, DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info("***************************");
            logger.Info("GetShippedRevenue method call started.");
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                        return "An unsuccessful login attempt was made";
                    string xml = "test xml response";
                    // business logic 

                    FinancialsShippedRevenue rev = Repository.GetShippedRevenue(fromDate, toDate, CountryISOCode);
                    xml = rev.Serialize();
                    logger.Info("Exiting the GetShippedRevenue method call");
                    logger.Info("***************************");
                    return xml;
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetShippedRevenue", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }
    }
}
