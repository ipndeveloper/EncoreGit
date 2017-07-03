using System.Data.SqlClient;

namespace NetSteps.Data.Entities
{
    public partial class CacheDependency
    {
        public static bool InvalidateCache(string cacheDependencyName)
        {
            return BusinessLogic.InvalidateCache(Repository, cacheDependencyName);
        }

        public static SqlDependency GetSqlDependencyByName(string name)
        {
            return BusinessLogic.GetSqlDependencyByCacheDependencyName(Repository, name);
        }
    }
}
