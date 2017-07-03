using System.Data.SqlClient;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICacheDependencyRepository
    {
        bool UpdateLastModifiedDateByName(string dependencyName);
        SqlDependency GetSqlDependencyByName(string name);
    }
}
