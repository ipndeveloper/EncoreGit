using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetSteps.Foundation.Entity.Mocks
{
    /// <summary>
    /// An in-memory <see cref="IDbSet{T}"/> for testing.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class MockDbSet<T> : IDbSet<T>
        where T : class
    {
        private readonly ICollection<T> _data;
        private readonly IQueryable<T> _query;
        private readonly Func<T, object>[] _keySelectors;

        public MockDbSet(params Func<T, object>[] keySelectors)
            : this(new HashSet<T>(), keySelectors)
        {
            Contract.Requires<ArgumentNullException>(keySelectors != null);
            Contract.Requires<ArgumentException>(keySelectors.Length > 0);
        }

        public MockDbSet(ICollection<T> data, params Func<T, object>[] keySelectors)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentNullException>(keySelectors != null);
            Contract.Requires<ArgumentException>(keySelectors.Length > 0);

            _data = data;
            _query = _data.AsQueryable();
            _keySelectors = keySelectors;
        }

        public T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public T Find(params object[] keyValues)
        {
            var entities = _data.AsEnumerable();
            for (int i = 0; i < _keySelectors.Length; i++)
            {
                entities = entities
                    .Where(x => _keySelectors[i](x).Equals(keyValues[i]))
                    .ToList()
                    .AsEnumerable();
            }
            return entities.FirstOrDefault();
        }

        private ObservableCollection<T> _local;
        public ObservableCollection<T> Local
        {
            get
            {
                if (_local == null)
                {
                    _local = new ObservableCollection<T>();
                }
                return _local;
            }
        }

        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _query.ElementType; }
        }

        public Expression Expression
        {
            get { return _query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _query.Provider; }
        }
    }
}
