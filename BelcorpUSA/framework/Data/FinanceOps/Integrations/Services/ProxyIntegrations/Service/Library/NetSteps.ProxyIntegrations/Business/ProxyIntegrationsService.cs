using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using System.ServiceModel;
using System.Runtime.Serialization;
using NetstepsDataAccess.Security;
using log4net;
using NetSteps.ProxyIntegrationsService.Data;

namespace NetSteps.ProxyIntegrations
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProxyIntegrationsService : IProxyIntegrations
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(ProxyIntegrationsService));

        public ProxyIntegrationsService() { }

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
                    return ProxyRepository.GetAccounts(userName, password);
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetAccounts Proxy", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }

        public string GetReturnOrders(string userName, string password)
        {
            log4net.Config.XmlConfigurator.Configure();

            logger.Info("***************************");
            logger.Info("GetReturnOrders method call started.");
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    // authenticate
                    if (!IntegrationsSecurity.Authenticate(userName, password))
                        return "An unsuccessful login attempt was made";
                    // business logic
                    logger.Info("Exiting the GetReturnOrders method call");
                    logger.Info("***************************");
                    return ProxyRepository.GetReturnOrders(userName, password);
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetReturnOrders", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }

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
                    logger.Info("Exiting the GetOrders method call");
                    logger.Info("***************************");
                    return ProxyRepository.GetOrders(userName, password);
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetOrders", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }
    }
}
