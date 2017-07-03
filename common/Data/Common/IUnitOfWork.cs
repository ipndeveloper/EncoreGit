using System;

namespace NetSteps.Data.Common
{
    public interface IUnitOfWork : IObjectContext
    {
        void SaveChanges();
    }
}
