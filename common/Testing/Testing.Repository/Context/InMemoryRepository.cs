using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Repository.Common.Interfaces;

namespace Testing.Repository.Context
{
    /// <summary>
    /// Example of InMemoryRepository
    /// </summary>
    public class InMemoryRepository : IRepository
    {
        readonly IObjectContext _context;

        /// <summary>
        /// The InMemoryContext that i made.. this should be switch outable
        /// </summary>
        /// <param name="context"></param>
        public InMemoryRepository(IObjectContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get by an int key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByKey<T>(int key) where T : class,IEntity, new()
        {
            return _context.GetObjects<T>().FirstOrDefault(x => x.Id == key);
        }
        /// <summary>
        /// Get all the objects that match this specification return as Iqueryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(ISpecification<T> specification) where T : class, IEntity, new()
        {
            var objects = _context.GetObjects<T>();
            return objects.Where(specification.IsSatisfied().Compile());
        }

        /// <summary>
        /// return unique singular t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public T Single<T>(ISpecification<T> specification) where T : class, IEntity, new()
        {
            var objects = _context.GetObjects<T>();
            return objects.Single(specification.IsSatisfied().Compile());
        }

        /// <summary>
        /// returns first entity where it meets specification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public T First<T>(ISpecification<T> specification) where T : class, IEntity, new()
        {
            var objects = _context.GetObjects<T>();
            return objects.FirstOrDefault(specification.IsSatisfied().Compile());
        }
        /// <summary>
        /// Add an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Add<T>(T entity) where T : class,IEntity, new()
        {
            _context.Add(entity);
        }
        /// <summary>
        /// update an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class, IEntity, new()
        {
            var item = _context.GetObjects<T>().Any(x => x.Id == entity.Id);
            if (!item) return;

            _context.Delete(entity);
            _context.Add(entity);
        }
        /// <summary>
        /// deletes entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public int Delete<T>(ISpecification<T> specification) where T : class, IEntity, new()
        {
            var objects = _context.GetObjects<T>();
            var expression = specification.IsSatisfied();
            var filteredQuery = objects.Where(expression.Compile()).ToList();
            var deletedCount = 0;

            foreach (var item in filteredQuery)
            {
                deletedCount++;
                _context.Delete(item);
            }
            return deletedCount;
        }
        /// <summary>
        /// gets counts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public int Count<T>(ISpecification<T> specification) where T : class, IEntity, new()
        {
            var objects = _context.GetObjects<T>();
            return objects.Count(specification.IsSatisfied().Compile());
        }
    }
}
