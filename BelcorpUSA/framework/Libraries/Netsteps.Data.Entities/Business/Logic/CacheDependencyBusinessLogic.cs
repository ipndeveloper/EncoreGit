using System.Data.SqlClient;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class CacheDependencyBusinessLogic
    {
        public bool InvalidateCache(ICacheDependencyRepository repository, string cacheDependencyName)
        {
            return repository.UpdateLastModifiedDateByName(cacheDependencyName);
        }

        public SqlDependency GetSqlDependencyByCacheDependencyName(ICacheDependencyRepository repository, string cacheDependencyName)
        {
            return repository.GetSqlDependencyByName(cacheDependencyName);
        }
    }
}
