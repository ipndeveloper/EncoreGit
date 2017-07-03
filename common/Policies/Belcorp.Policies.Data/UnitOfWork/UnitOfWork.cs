using System;
using System.Collections.Generic;
using Belcorp.Policies.Data.UnitOfWork.Interface;
using Belcorp.Policies.Data.GenericRepository.Interface;
using Belcorp.Policies.Data.GenericRepository;
using Belcorp.Policies.Entities;
using System.Linq;

namespace Belcorp.Policies.Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : ICoreEntities, new()
    {
        private readonly ICoreEntities _ctx;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork()
        {
            _ctx = new TContext();
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IGenericRepository<TEntity>;
            }

            // If the repository for that Model class doesn't exist, create it
            var repository = new GenericRepository<TEntity>(_ctx);

            // Add it to the dictionary
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;

            if (disposing)
            {
                _ctx.Dispose();
            }

            this._disposed = true;
        }
    } 
}