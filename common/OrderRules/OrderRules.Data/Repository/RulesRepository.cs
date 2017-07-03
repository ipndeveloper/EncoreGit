using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OrderRules.Core;
using OrderRules.Core.Model;
using OrderRules.Data.BaseRepository;
using OrderRules.Data.Repository.Interface;

namespace OrderRules.Data.Repository
{
    public class RulesRepository : GenericRepository<Rules>, IRulesRepository, IDisposable
    {
        public RulesRepository(CoreEntities context)
            : base(context)
        {
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