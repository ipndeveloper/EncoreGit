using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IRepositoryStoredProcedure<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters);
        IEnumerable<T> ExecWithStoreProcedure<T>(string query, params object[] parameters);
        
    }
}
