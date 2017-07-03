using System;
using OrderRules.Core.Model;
using OrderRules.Data.UnitOfWork.Interface;

namespace OrderRules.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private CoreEntities context;

        public UnitOfWork(CoreEntities context)
        {
            this.context = context;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}