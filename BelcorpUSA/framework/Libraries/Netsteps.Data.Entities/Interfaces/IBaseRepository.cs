using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 7/7/2010
    /// </summary>
	[ContractClass(typeof(Contracts.BaseRepositoryContracts<,>))]
	public interface IBaseRepository<T, TKeyType> : IRepository
    {
        PrimaryKeyInfo PrimaryKeyInfo { get; }

        T Load(TKeyType primaryKey);
        T LoadFull(TKeyType primaryKey);
        List<T> LoadAll();
        List<T> LoadAllFull();
        List<T> LoadBatch(IEnumerable<TKeyType> primaryKeys);
        List<T> LoadBatchFull(IEnumerable<TKeyType> primaryKeys);

        SqlUpdatableList<T> LoadAllFullWithSqlDependency();
        SqlUpdatableList<T> LoadBatchWithSqlDependency(IEnumerable<TKeyType> primaryKeys);

        void Save(T obj);
        void SaveBatch(IEnumerable<T> items);
        void Delete(T obj);
        void Delete(TKeyType primaryKey);
        void DeleteBatch(IEnumerable<TKeyType> primaryKeys);

        bool Exists(TKeyType primaryKey);

        PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters param);

        T GetRandomRecord();
        T GetRandomRecordFull();

        /// <summary>
        /// Returns the number of entities in the database.
        /// </summary>
        int Count();

        /// <summary>
        /// Returns the number of entities in the database that satisfy a condition.
        /// </summary>
        int Count(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Determines whether the database contains any entities.
        /// </summary>
        bool Any();
        
        /// <summary>
        /// Determines whether the database contains any entities that satisfy a condition.
        /// </summary>
        bool Any(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Returns a filtered list of entities.
        /// </summary>
        List<T> Where(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Returns a filtered list of entities including specified related objects.
        /// </summary>
        List<T> Where(Expression<Func<T, bool>> predicate, IEnumerable<string> includes);
        
        /// <summary>
        /// Returns a filtered and projected list of entities.
        /// </summary>
        List<TSelected> WhereSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector);
        
        /// <summary>
        /// Returns the first entity that satisfies the specified condition, or null if no such entity is found.
        /// </summary>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Returns the first entity that satisfies the specified condition, or null if no such entity is found, and includes specified related objects.
        /// </summary>
        T FirstOrDefault(Expression<Func<T, bool>> predicate, IEnumerable<string> includes);
        
        /// <summary>
        /// Projects and returns the first entity that satisfies the specified condition, or returns null if no such entity is found.
        /// </summary>
        TSelected FirstOrDefaultSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IBaseRepository<,>))]
		internal abstract class BaseRepositoryContracts<T, TKeyType> : IBaseRepository<T, TKeyType>
		{
			PrimaryKeyInfo IBaseRepository<T, TKeyType>.PrimaryKeyInfo
			{
				get { throw new NotImplementedException(); }
			}

			T IBaseRepository<T, TKeyType>.Load(TKeyType primaryKey)
			{
				throw new NotImplementedException();
			}

			T IBaseRepository<T, TKeyType>.LoadFull(TKeyType primaryKey)
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.LoadAll()
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.LoadAllFull()
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.LoadBatch(IEnumerable<TKeyType> primaryKeys)
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.LoadBatchFull(IEnumerable<TKeyType> primaryKeys)
			{
				Contract.Requires<ArgumentNullException>(primaryKeys != null);

				throw new NotImplementedException();
			}

			SqlUpdatableList<T> IBaseRepository<T, TKeyType>.LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			SqlUpdatableList<T> IBaseRepository<T, TKeyType>.LoadBatchWithSqlDependency(IEnumerable<TKeyType> primaryKeys)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<T, TKeyType>.Save(T obj)
			{
				Contract.Requires<ArgumentNullException>(obj != null);

				throw new NotImplementedException();
			}

			void IBaseRepository<T, TKeyType>.SaveBatch(IEnumerable<T> items)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<T, TKeyType>.Delete(T obj)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<T, TKeyType>.Delete(TKeyType primaryKey)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<T, TKeyType>.DeleteBatch(IEnumerable<TKeyType> primaryKeys)
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<T, TKeyType>.Exists(TKeyType primaryKey)
			{
				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> IBaseRepository<T, TKeyType>.GetAuditLog(int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			T IBaseRepository<T, TKeyType>.GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			T IBaseRepository<T, TKeyType>.GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			int IBaseRepository<T, TKeyType>.Count()
			{
				throw new NotImplementedException();
			}

			int IBaseRepository<T, TKeyType>.Count(Expression<Func<T, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<T, TKeyType>.Any()
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<T, TKeyType>.Any(Expression<Func<T, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.Where(Expression<Func<T, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			List<T> IBaseRepository<T, TKeyType>.Where(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			List<TSelected> IBaseRepository<T, TKeyType>.WhereSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			T IBaseRepository<T, TKeyType>.FirstOrDefault(Expression<Func<T, bool>> predicate)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			T IBaseRepository<T, TKeyType>.FirstOrDefault(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			TSelected IBaseRepository<T, TKeyType>.FirstOrDefaultSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector)
			{
				throw new NotImplementedException();
			}
		}
	}
}