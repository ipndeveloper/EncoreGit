using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using System.ServiceModel;
using System.Runtime.Serialization;
using NetstepsDataAccess.Security;
using log4net;
using NetSteps.ReturnOrderExport.Data;

namespace NetSteps.ReturnOrderExport
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ReturnOrderExportService : IReturnOrderExport
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(ReturnOrderExportService));

        public ReturnOrderExportService() { }

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
                    return Repository.GetReturnOrders();
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "GetReturnOrders", ex.Message);
                    return "There was an error encountered: " + ex.Message;
                }
            }
        }
    }
}
