using System;
using System.Reflection;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Utility
{
    public class AuditHelper
    {
        /// <summary>
        /// Supply the entityName as a singular name of the Entity: Ex: Account, Order, ect..
        /// Helper method to call the entityName's Static GetAuditLog() method via reflection to help make a reusable Audit grid control(UI). - JHE
        /// Example: var results = AuditHelper.GetAuditLog("CorporateUser", 1, new NetSteps.Common.Base.PaginatedListParameters());
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="primaryKey"></param>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public static PaginatedList<AuditLogRow> GetAuditLog(string entityName, int primaryKey, AuditLogSearchParameters searchParameters)
        {
            string typeName = string.Format("NetSteps.Data.Entities.{0}", entityName);
            Assembly a = Assembly.GetExecutingAssembly();
            Type type = a.GetType(typeName);

            PaginatedList<AuditLogRow> result = (PaginatedList<AuditLogRow>)type.InvokeMember("GetAuditLog", BindingFlags.InvokeMethod, null, null, new object[] { primaryKey, searchParameters });
            return result;
        }

        public static PaginatedList<AuditLogRow> GetAuditLog(object fullyLoadedEntity, string entityName, int primaryKey, AuditLogSearchParameters searchParameters)
        {
            string typeName = string.Format("NetSteps.Data.Entities.{0}", entityName);
            Assembly a = Assembly.GetExecutingAssembly();
            Type type = a.GetType(typeName);

            PaginatedList<AuditLogRow> result = (PaginatedList<AuditLogRow>)type.InvokeMember("GetAuditLog", BindingFlags.InvokeMethod, null, null, new object[] { fullyLoadedEntity, primaryKey, searchParameters });
            return result;
        }
    }
}
