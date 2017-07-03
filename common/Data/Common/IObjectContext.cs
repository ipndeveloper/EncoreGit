using System;
using System.Data.Objects;

namespace NetSteps.Data.Common
{
    public interface IObjectContext : IDisposable
    {
        IObjectSet<T> CreateObjectSet<T>() where T : class;
    }
}
