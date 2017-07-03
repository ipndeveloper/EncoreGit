using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;
using System.Configuration;

namespace NetSteps.Integrations.Internals.Security
{
    public static class IntegrationsSecurity
    {
        public static bool Authenticate(string userName, string password)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                bool isAuthenticated = false;
                try
                {
                    if (userName != ConfigurationManager.AppSettings["ClientUserName"].ToString()
                        || password != ConfigurationManager.AppSettings["ClientPassword"].ToString())
                    {
                        db.UspLogisticsCommunicationInsert("An unsuccessful login attempt was made by username: " + userName + " password: " + password);
                        return isAuthenticated;
                    }
                    isAuthenticated = true;
                    return isAuthenticated;
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "Exception thrown in Authenticating User", ex.Message);
                    return isAuthenticated;
                }
            }
        }

        public static bool IsInDebugMode()
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["IsInDebugMode"]);
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "Exception thrown in determining if application is in Debug Mode",
                        ex.Message);
                    return false;
                }
            }
        }

        public static int ModifiedByUserID()
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ModifiedByUserID"]);
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "Exception thrown in getting the ModifiedByUserID configuration section",
                        ex.Message);
                    return 0;
                }
            }
        }
    }
}
