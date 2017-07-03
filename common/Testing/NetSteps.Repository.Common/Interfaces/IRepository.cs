using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Repository.Common.Interfaces
{
    //NOTES:
    //give the IUnitOfWork to the Repository, not the other way around
    //make sure IUnitOfWork is a short lived singleton instance wherever it is used(container life cycle scoped to http request usually)
    //call IUnitOfWork.SaveChanges() or CommitTransaction in one place at the end
    public interface IRepository
    {
          /// <summary>
        /// Get a object by int key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetByKey<T>(int key) where T : class, IEntity, new();
        /// <summary>
        /// Allows filtering by a specification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(ISpecification<T> specification) where T : class, IEntity, new();
        /// <summary>
        /// Must be a unique result exactly of one other wise should throw and exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        T Single<T>(ISpecification<T> specification) where T : class, IEntity, new();
        /// <summary>
        /// Returns the first result by provided specification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        T First<T>(ISpecification<T> specification) where T : class, IEntity, new();
        /// <summary>
        /// Adds and entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class, IEntity, new();
        /// <summary>
        /// Updates the entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class, IEntity, new();
        /// <summary>
        /// Deletes the entity, should return the count of how many entities were actually deleted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        int Delete<T>(ISpecification<T> specification) where T : class, IEntity, new();
        /// <summary>
        /// returns count 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        int Count<T>(ISpecification<T> specification) where T : class, IEntity, new();
    }
}