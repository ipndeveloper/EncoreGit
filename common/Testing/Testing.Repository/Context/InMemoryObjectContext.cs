using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuCore.Util;
using NetSteps.Repository.Common.Interfaces;

namespace Testing.Repository.Context
{
    public class InMemoryObjectContext : IObjectContext
    {
        readonly Cache<Type, HashSet<object>> _objectHashSets;

        public InMemoryObjectContext()
        {
            _objectHashSets = new Cache<Type, HashSet<object>>(x => new HashSet<object>());
        }

        public IEnumerable<T> GetObjects<T>() where T : IEntity, new()
        {
            var type = typeof(T);
            return _objectHashSets[type].Cast<T>();
        }

        public void Add<T>(T entity) where T : IEntity, new()
        {
            var type = typeof(T);
            _objectHashSets[type].Add(entity);
        }

        public void Delete<T>(T entity) where T : IEntity, new()
        {
            var type = typeof(T);
            var hashSet = _objectHashSets[type];
            if (hashSet.Contains(entity))
            {
                hashSet.Remove(entity);
            }
        }
    }
}
