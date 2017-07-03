using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetSteps.Foundation.Entity
{
    [ContractClass(typeof(Contracts.EntityModelRepositoryContracts<,,>))]
    public interface IEntityModelRepository<TEntity, TModel, TContext>
    {
        TEntity Add(TContext context, TModel model);
        bool Any(TContext context);
        bool Any(TContext context, Expression<Func<TEntity, bool>> predicate);
        int Count(TContext context);
        int Count(TContext context, Expression<Func<TEntity, bool>> predicate);
        TModel First(TContext context);
        TModel First(TContext context, params object[] keyValues);
        TModel First(TContext context, TModel model);
        TModel First(TContext context, Expression<Func<TEntity, bool>> predicate);
        TModel FirstOrDefault(TContext context);
        TModel FirstOrDefault(TContext context, params object[] keyValues);
        TModel FirstOrDefault(TContext context, TModel model);
        TModel FirstOrDefault(TContext context, Expression<Func<TEntity, bool>> predicate);
        Expression<Func<TEntity, bool>> GetPredicateForModel(TModel model);
        void Remove(TContext context, params object[] keyValues);
        void RemoveWhere(TContext context, Expression<Func<TEntity, bool>> predicate);
        IList<TModel> ToList(TContext context);
        TEntity Update(TContext context, TModel model);
        void UpdateEntity(TEntity entity, TModel model);
        void UpdateModel(TModel model, TEntity entity);
        IList<TModel> Where(TContext context, Expression<Func<TEntity, bool>> predicate);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IEntityModelRepository<,,>))]
        internal abstract class EntityModelRepositoryContracts<TEntity, TModel, TContext> : IEntityModelRepository<TEntity, TModel, TContext>
        {
            TEntity IEntityModelRepository<TEntity, TModel, TContext>.Add(TContext context, TModel model)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            bool IEntityModelRepository<TEntity, TModel, TContext>.Any(TContext context)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                throw new NotImplementedException();
            }

            bool IEntityModelRepository<TEntity, TModel, TContext>.Any(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }

            int IEntityModelRepository<TEntity, TModel, TContext>.Count(TContext context)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                throw new NotImplementedException();
            }

            int IEntityModelRepository<TEntity, TModel, TContext>.Count(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.First(TContext context)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.First(TContext context, params object[] keyValues)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(keyValues != null);
                Contract.Requires<ArgumentException>(keyValues.Length > 0);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.First(TContext context, TModel model)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.First(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.FirstOrDefault(TContext context)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.FirstOrDefault(TContext context, params object[] keyValues)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(keyValues != null);
                Contract.Requires<ArgumentException>(keyValues.Length > 0);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.FirstOrDefault(TContext context, TModel model)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            TModel IEntityModelRepository<TEntity, TModel, TContext>.FirstOrDefault(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }

            Expression<Func<TEntity, bool>> IEntityModelRepository<TEntity, TModel, TContext>.GetPredicateForModel(TModel model)
            {
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            void IEntityModelRepository<TEntity, TModel, TContext>.Remove(TContext context, params object[] keyValues)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(keyValues != null);
                Contract.Requires<ArgumentException>(keyValues.Length > 0);
                throw new NotImplementedException();
            }

            void IEntityModelRepository<TEntity, TModel, TContext>.RemoveWhere(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }

            IList<TModel> IEntityModelRepository<TEntity, TModel, TContext>.ToList(TContext context)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                throw new NotImplementedException();
            }

            TEntity IEntityModelRepository<TEntity, TModel, TContext>.Update(TContext context, TModel model)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            void IEntityModelRepository<TEntity, TModel, TContext>.UpdateEntity(TEntity entity, TModel model)
            {
                Contract.Requires<ArgumentNullException>(entity != null);
                Contract.Requires<ArgumentNullException>(model != null);
                throw new NotImplementedException();
            }

            void IEntityModelRepository<TEntity, TModel, TContext>.UpdateModel(TModel model, TEntity entity)
            {
                Contract.Requires<ArgumentNullException>(model != null);
                Contract.Requires<ArgumentNullException>(entity != null);
                throw new NotImplementedException();
            }

            IList<TModel> IEntityModelRepository<TEntity, TModel, TContext>.Where(TContext context, Expression<Func<TEntity, bool>> predicate)
            {
                Contract.Requires<ArgumentNullException>(context != null);
                Contract.Requires<ArgumentNullException>(predicate != null);
                throw new NotImplementedException();
            }
        }
    }
}
