using System.Data.SqlClient;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ICacheDependencyBusinessLogic
    {
        bool InvalidateCache(ICacheDependencyRepository repository, string cacheDependencyName);
        SqlDependency GetSqlDependencyByCacheDependencyName(ICacheDependencyRepository repository, string cacheDependencyName);
    }
}
