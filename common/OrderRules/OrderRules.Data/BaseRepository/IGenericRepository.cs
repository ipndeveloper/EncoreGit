using System;
using System.Collections.Generic;
namespace OrderRules.Data.BaseRepository.Interface
{
    public interface IGenericRepository<TEntity>
     where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        IEnumerable<TEntity> Get();
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
