using System.Collections.Generic;
using NetSteps.Repository.Common.Interfaces;

namespace Testing.Repository.Context
{
    public interface IObjectContext
    {
        /// <summary>
        /// Get objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetObjects<T>() where T : IEntity, new();
        /// <summary>
        /// Add objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : IEntity, new();
        /// <summary>
        /// delete objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : IEntity, new();
    }
}