using System;

namespace OrderRules.Data.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        void Dispose();
        void Commit();
    }
}
