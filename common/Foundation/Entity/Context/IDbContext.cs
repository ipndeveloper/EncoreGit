using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NetSteps.Foundation.Entity
{
    public interface IDbContext : IDisposable
    {
        int SaveChanges();
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
