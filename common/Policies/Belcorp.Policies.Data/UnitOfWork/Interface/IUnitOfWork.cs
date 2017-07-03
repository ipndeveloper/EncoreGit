using System;
using Belcorp.Policies.Data.GenericRepository.Interface;

namespace Belcorp.Policies.Data.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void Save();
    }
}
