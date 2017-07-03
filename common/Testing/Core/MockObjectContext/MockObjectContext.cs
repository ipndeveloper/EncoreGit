using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Collections.Concurrent;
using NetSteps.Data.Common;

namespace NetSteps.Testing.Core.MockObjectContext
{
    public class MockObjectContext : IUnitOfWork
    {
        private static ConcurrentDictionary<Type, object> _objectSetMap;

        public MockObjectContext()
        {
            _objectSetMap = new ConcurrentDictionary<Type, object>();
        }

        public IObjectSet<T> CreateObjectSet<T>() where T:class
        {
            if (!_objectSetMap.ContainsKey(typeof(T)))
            {
                _objectSetMap.TryAdd(typeof(T), new MockObjectSet<T>());
            }
            return (IObjectSet<T>)_objectSetMap[typeof(T)];
        }

        public void SaveChanges()
        {

        }

        public void Dispose()
        {
         
        }
    }
}
