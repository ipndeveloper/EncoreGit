using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using System.ServiceModel;
using System.Runtime.Serialization;
using NetstepsDataAccess.Security;
using log4net;
using NetSteps.Base.Integrations.Data;

namespace NetSteps.Base.Integrations
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class BaseIntegrationsService : IBaseIntegrations
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(BaseIntegrationsService));

        public BaseIntegrationsService() { }

        public string GetOrders(string userName, string password)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info("***************************");
            logger.Info("GetOrders method call started.");
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                        return "An unsuccessful login attempt was made";
                    // business logic
                    logger.Info("Entering GetOrders Repository Method Call");
                    logger.Info("***************************");
                    return Repository.GetOrders();
                }
                catch (Exception ex)
                {
                    logger.Info("Exception Encountered, Message: " + ex.Message + " Inner Exception: " + ex.InnerException.Message);
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetOrders", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }

        public string GetAccounts(string userName, string password)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info("***************************");
            logger.Info("GetAccounts method call started.");
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                        return "An unsuccessful login attempt was made";
                    // business logic
                    logger.Info("Exiting the GetAccounts method call");
                    logger.Info("***************************");
                    return Repository.GetAccounts();
                }
                catch (Exception ex)
                {
                    logger.Info("Exception Encountered, Message: " + ex.Message + " Inner Exception: " + ex.InnerException.Message);
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetAccounts", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }
    }
}
