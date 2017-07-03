using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Foundation.Entity
{
    public abstract class EntityModelRepository<TEntity, TModel, TContext> : IEntityModelRepository<TEntity, TModel, TContext>
        where TEntity : class, new()
        where TContext : IDbContext
    {
        public abstract Expression<Func<TEntity, bool>> GetPredicateForModel(TModel model);
        public abstract void UpdateEntity(TEntity entity, TModel model);
        public abstract void UpdateModel(TModel model, TEntity entity);

        public virtual TEntity Add(TContext context, TModel model)
        {
            var entity = new TEntity();
            
            UpdateEntity(entity, model);
            
            return context
                .Set<TEntity>()
                .Add(entity);
        }

        public virtual bool Any(TContext context)
        {
            return context
                .Set<TEntity>()
                .Any();
        }

        public virtual bool Any(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            return context
                .Set<TEntity>()
                .Any(predicate);
        }

        public virtual int Count(TContext context)
        {
            return context
                .Set<TEntity>()
                .Count();
        }

        public virtual int Count(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            return context
                .Set<TEntity>()
                .Count(predicate);
        }

        public virtual TModel First(TContext context)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault();

            if (entity == null)
            {
                throw new Exception();
            }

            return CreateModel(entity);
        }

        public virtual TModel First(TContext context, params object[] keyValues)
        {
            var entity = context
                .Set<TEntity>()
                .Find(keyValues);

            if (entity == null)
            {
                throw new Exception();
            }

            return CreateModel(entity);
        }

        public virtual TModel First(TContext context, TModel model)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault(GetPredicateForModel(model));

            if (entity == null)
            {
                throw new Exception();
            }

            return CreateModel(entity);
        }

        public virtual TModel First(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault(predicate);

            if (entity == null)
            {
                throw new Exception();
            }

            return CreateModel(entity);
        }

        public virtual TModel FirstOrDefault(TContext context)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault();

            if (entity == null)
            {
                return default(TModel);
            }

            return CreateModel(entity);
        }

        public virtual TModel FirstOrDefault(TContext context, params object[] keyValues)
        {
            var entity = context
                .Set<TEntity>()
                .Find(keyValues);

            if (entity == null)
            {
                return default(TModel);
            }

            return CreateModel(entity);
        }

        public virtual TModel FirstOrDefault(TContext context, TModel model)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault(GetPredicateForModel(model));

            if (entity == null)
            {
                return default(TModel);
            }

            return CreateModel(entity);
        }

        public virtual TModel FirstOrDefault(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault(predicate);

            if (entity == null)
            {
                return default(TModel);
            }

            return CreateModel(entity);
        }

        public virtual void Remove(TContext context, params object[] keyValues)
        {
            var dbSet = context.Set<TEntity>();
            var entity = dbSet.Find(keyValues);

            if (entity == null)
            {
                throw new Exception();
            }

            dbSet.Remove(entity);
        }

        public virtual void RemoveWhere(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = context.Set<TEntity>();
            var entities = dbSet
                .Where(predicate)
                .ToList();

            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }
        }

        public virtual IList<TModel> ToList(TContext context)
        {
            return context
                .Set<TEntity>()
                .ToList()
                .Select(x => CreateModel(x))
                .ToList();
        }

        public virtual TEntity Update(TContext context, TModel model)
        {
            var entity = context
                .Set<TEntity>()
                .FirstOrDefault(GetPredicateForModel(model));

            if (entity == null)
            {
                throw new Exception();
            }

            UpdateEntity(entity, model);
            return entity;
        }

        public virtual IList<TModel> Where(TContext context, Expression<Func<TEntity, bool>> predicate)
        {
            return context
                .Set<TEntity>()
                .Where(predicate)
                .ToList()
                .Select(x => CreateModel(x))
                .ToList();
        }

        protected virtual TModel CreateModel(TEntity entity)
        {
            Contract.Requires<ArgumentNullException>(entity != null);

            var model = Create.New<TModel>();
            UpdateModel(model, entity);
            return model;
        }
    }
}
