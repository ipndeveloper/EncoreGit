using System;
using System.Collections.Generic;
using System.Linq;
namespace Belcorp.Policies.Data.GenericRepository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> entity);
        void Update(T entity);
        bool Any();
    }
}
