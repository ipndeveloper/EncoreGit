using System;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CacheDependencyRepository
    {
        public bool UpdateLastModifiedDateByName(string dependencyName)
        {
            //return ExceptionHandledDataAction.Run<bool>(new ExecutionContext(this), () =>
            //{
                using (NetStepsEntities context = CreateContext())
                {
                    var dependency = (from cd in context.CacheDependencies
                                      where cd.Name == dependencyName
                                      select cd).FirstOrDefault();

                    if(dependency != null)
                    {
                        dependency.StartTracking();
                        dependency.DateLastModifiedUTC = DateTime.UtcNow;
                        base.Save(dependency);

                        return true;
                    }

                    return false;
                };
            //});
        }

        public SqlDependency GetSqlDependencyByName(string dependencyName)
        {
            return ExceptionHandledDataAction.Run<SqlDependency>(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var dependencyQuery = from cd in context.CacheDependencies
                                          where cd.Name == dependencyName
                                          select cd;
              
                    var entityQuery = (ObjectQuery<CacheDependency>)dependencyQuery;
                    var result = LoadWithSqlDependency(entityQuery);
                    return result.SqlDependency;
                }
            });
        }
    }
}
