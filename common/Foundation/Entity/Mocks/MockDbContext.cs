using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NetSteps.Foundation.Entity.Mocks
{
    public abstract class MockDbContext : IDbContext
    {
        public int SaveChanges()
        {
            return 0;
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return GetType()
                .GetProperties()
                .First(x => x.PropertyType == typeof(IDbSet<TEntity>))
                .GetValue(this, null) as IDbSet<TEntity>;
        }

        public void Dispose() { }
    }
}
