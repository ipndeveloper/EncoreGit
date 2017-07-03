using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface IRepositoryStoreProcedure<TEntity>: IDisposable where TEntity : class
    {
        IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters);
        IEnumerable<T> ExecWithStoreProcedure<T>(string query, params object[] parameters);
        int ExecWithStoreProcedureScalar(string query, params object[] parameters);
        T ExecWithStoreProcedureScalarType<T>(string query, params object[] parameters);
        int ExecWithStoreProcedureSave(string query, params object[] parameters);
        int ExecWithStoreProcedureNonQuery(string procedureName, params object[] parameters); 
        List<Dictionary<string, object>> ExecQueryEntidadDictionary(string query);
        int ExecQueryRegistroDinamico(string query);
    }
}
