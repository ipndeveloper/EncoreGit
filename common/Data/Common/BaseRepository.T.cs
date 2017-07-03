using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Common
{
    //TODO: prevent calling the context if it hasn't been set (i.e. the parameterless constructor).

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected virtual string[] Includes { get { return new string[0]; } }

        /// <summary>
        /// The context object for the database
        /// </summary>
        protected internal IObjectContext _context;

        /// <summary>
        /// Sets the data context.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public void SetDataContext(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork;
        }

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<T> Fetch()
        {
            if (_context is ObjectContext)
            {
                ObjectQuery<T> query = (_context as ObjectContext).CreateObjectSet<T>();
                foreach (string include in Includes)
                {
                    query = query.Include(include);
                }
                return query;
            }
            return _context.CreateObjectSet<T>();
        }

        /// <summary>
        /// Gets all records as an IEnumberable
        /// </summary>
        /// <returns>An IEnumberable object containing the results of the query</returns>
        public IEnumerable<T> GetAll()
        {
            return Fetch().AsEnumerable();
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A collection containing the results of the query</returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return Fetch().Where<T>(predicate);
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public T Single(Func<T, bool> predicate)
        {
            return Fetch().Single<T>(predicate);
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier) or default(T) 
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public T SingleOrDefault(Func<T, bool> predicate)
        {
            return Fetch().SingleOrDefault<T>(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public T First(Func<T, bool> predicate)
        {
            return Fetch().First<T>(predicate);
        }

        /// <summary>
        /// Deletes the specified entitiy
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.CreateObjectSet<T>().DeleteObject(entity);
        }

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        public void Delete(Func<T, bool> predicate)
        {
            var records = Fetch().Where<T>(predicate);
            var objectSet = _context.CreateObjectSet<T>();

            foreach (T record in records)
            {
                _context.CreateObjectSet<T>().DeleteObject(record);
            }
        }

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.CreateObjectSet<T>().AddObject(entity);
        }

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="entity">Entity to attach</param>
        public void Attach(T entity)
        {
            _context.CreateObjectSet<T>().Attach(entity);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
