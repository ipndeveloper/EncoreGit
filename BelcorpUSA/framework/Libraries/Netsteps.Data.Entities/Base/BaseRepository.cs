using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Expressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace NetSteps.Data.Entities.Repositories
{
    using System.Diagnostics.Contracts;
    using Extensibility.Core;

    /// <summary>
    /// Author: John Egbert
    /// Description: BaseRepository class to reuse common CRUD functionality where possible. 
    /// Created: 03-11-2010
    /// </summary>
    [Serializable]
    public class BaseRepository<T, TKeyType, TContext> : IBaseRepository<T, TKeyType>
        where T : class, IObjectWithChangeTracker, IObjectWithChangeTrackerBusiness
        where TContext : ObjectContext
    {
        protected TContext DataContext = CreateContext();

        #region Members
        protected readonly object Lock = new object();
        private const MergeOption DefaultLoadMergeOption = MergeOption.AppendOnly;

        private Type _entityType = null;
        protected Type EntityType
        {
            get
            {
                if (_entityType == null)
                    _entityType = typeof(T);
                return _entityType;
            }
        }

        public PrimaryKeyInfo PrimaryKeyInfo
        {
            get
            {
                if (_entityPrimaryKeyInfo == null)
                {
                    using (TContext context = CreateContext())
                    {
                        _entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                    }
                }

                return _entityPrimaryKeyInfo;
            }
        }

        private PrimaryKeyInfo _entityPrimaryKeyInfo = null;
        protected PrimaryKeyInfo GetEntityPrimaryKeyInfo(TContext context)
        {
            if (_entityPrimaryKeyInfo == null)
                _entityPrimaryKeyInfo = context.GetPrimaryKeyInfo(EntityType);
            return _entityPrimaryKeyInfo;
        }

        private string _entitySetName = null;
        /// <summary>
        /// The Entities table name in the DB. - JHE
        /// </summary>
        protected virtual string EntitySetName
        {
            get
            {
                if (_entitySetName == null)
                {
                    using (TContext context = CreateContext())
                    {
                        _entitySetName = context.GetEntitySetName(EntityType);
                    }
                }
                return _entitySetName;
            }
        }

        /// <summary>
        /// Override this to load appropriate child collections and objects. - JHE
        /// NOTE: Consider depreciating this if the loadAllFullQuery with dynamic expression adding performance seems acceptable. - JHE
        /// </summary>
        protected virtual Func<TContext, TKeyType, IQueryable<T>> loadFullQuery
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Override this to load appropriate child collections and objects. - JHE
        /// </summary>
        protected virtual Func<TContext, IQueryable<T>> loadAllFullQuery
        {
            get
            {
                return null;
            }
        }

        public bool IsExtensibleType
        {
            get
            {
                return EntityType.IsAssignableFrom(typeof(IExtensibleDataObject));
            }
        }

        protected static TContext CreateContext()
        {
           
            
            return Create.New<TContext>();
        }
        #endregion

        #region Basic CRUD Methods
        /// <summary>
        /// This will 'Shallow' load an entity (only the entity; not child collections or child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T Load(TKeyType primaryKey)
        {

            if (EntityType.PropertyExists("Translations"))
                return GetSingleResult(primaryKey, GetLoadQueryWithTranslations);
            else
                return GetSingleResult(primaryKey, GetLoadQuery);
        }

        /// <summary>
        /// This will load an entire entity (the entity, child collections and child entities). - JHE
        /// The query used will first be the overridden loadFullQuery if implemented, then loadAllFullQuery, else the 'Shallow' load. - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T LoadFull(TKeyType primaryKey)
        {
            if (loadFullQuery != null)
                return GetSingleResult(primaryKey, GetLoadFullQueryOverridden);
            else if (loadAllFullQuery != null)
                return GetSingleResult(primaryKey, GetLoadFullQuery);
            else
                return Load(primaryKey);
        }

        /// <summary>
        /// This will 'Shallow' load all entities in the DB table (only the entity; not child collections or child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual List<T> LoadAll()
        {
            if (EntityType.PropertyExists("Translations"))
                return GetListResults(GetLoadALLQueryWithTranslations);
            else
                return GetListResults(GetLoadALLQuery);
        }

        /// <summary>
        /// This will load load all entities in the DB table (the entity, child collections and child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual List<T> LoadAllFull()
        {
            if (loadAllFullQuery == null)
                return LoadAll();
            else
                return GetListResults(GetLoadALLFullQuery);
        }

        /// <summary>
        /// Batch 'Shallow' Loads multiple Entities from a list of Primary Keys. - JHE
        /// </summary>
        /// <param name="htmlContentIds"></param>
        /// <returns></returns>
        public virtual List<T> LoadBatch(IEnumerable<TKeyType> primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Count() == 0)
                return null;

            ValidatePrimaryKeysForLoad(primaryKeys);

            if (EntityType.PropertyExists("Translations"))
                return GetListResults(primaryKeys, GetLoadBatchQueryWithTranslations);
            else
                return GetListResults(primaryKeys, GetLoadBatchQuery);
        }

        /// <summary>
        /// Batch Loads multiple Entities (the entity, child collections and child entities). from a list of Primary Keys. - JHE
        /// This required loadAllFullQuery to be implemented (overridden) on the Extending Repository. - JHE
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public virtual List<T> LoadBatchFull(IEnumerable<TKeyType> primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Count() == 0)
                return null;

            ValidatePrimaryKeysForLoad(primaryKeys);

            return GetListResults(primaryKeys, GetLoadBatchFullQuery);
        }

        /// <summary>
        /// This Method will return all the Entities with a SqlDependency object to enable reloading of data when modified. 
        /// This was created primarily for SmallCollection cache; to expire the cache when modified and allow them to be reloaded.
        /// This method should not be used for loading large object/collections. - JHE
        /// 
        /// http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldependency.aspx
        /// http://blog.cincura.net/231337-new-translate-and-executestorequery-executestorecommand-on-objectcontext-in-entity-framework-v4/
        /// http://mtaulty.com/CommunityServer/blogs/mike_taultys_blog/archive/2007/05/04/9302.aspx
        /// http://thedatafarm.com/blog/ado-net-2/it-s-working-sqldependency-here-s-how-i-did-it/
        /// 
        /// http://technet.microsoft.com/en-us/library/ms166057.aspx
        /// 
        /// Make sure SqlDependency is enabled in the DB - JHE
        ///     ALTER DATABASE NSFramework_Test SET ENABLE_BROKER -- Enable Broker
        ///     ALTER DATABASE NSFramework_Test SET NEW_BROKER WITH ROLLBACK IMMEDIATE;
        ///     SELECT databasepropertyex('NSFramework_Test', 'IsBrokerEnabled') -- Check if it's enabled
        /// </summary>
        /// <returns></returns>
        public virtual SqlUpdatableList<T> LoadAllFullWithSqlDependency()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityQuery = GetLoadALLQuery(context);
                    return LoadListWithSqlDependency(entityQuery);
                }
            });
        }

        public virtual SqlUpdatableList<T> LoadBatchWithSqlDependency(IEnumerable<TKeyType> primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Count() == 0)
                return null;

            ValidatePrimaryKeysForLoad(primaryKeys);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityQuery = GetLoadBatchFullQuery(primaryKeys, context);
                    return LoadListWithSqlDependency(entityQuery);
                }
            });
        }

        /// <summary>
        /// Start The SQL Dependench Cache for a connection
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="disableForCommissions">defaults to false</param>
        /// <returns></returns>
        private bool StartSqlDependencyCache(IDbConnection conn, bool disableForCommissions = false)
        {
            bool usingSqlDependency = ConfigurationManager.UseSqlDependencyCache;

            try
            {
                if (conn.GetConnectionStringInfo().InitialCatalog.ToLower().Contains("Commissions".ToLower()) && disableForCommissions)
                    usingSqlDependency = false;
                else if (usingSqlDependency)
                {
                    if (!DataAccess.SqlDependencyOpenConnectionStrings.ContainsIgnoreCase(conn.ConnectionString))
                    {
                        lock (Lock)
                        {
                            if (!DataAccess.SqlDependencyOpenConnectionStrings.ContainsIgnoreCase(conn.ConnectionString))
                            {
                                SqlDependency.Start(conn.ConnectionString);
                                DataAccess.SqlDependencyOpenConnectionStrings.Add(conn.ConnectionString);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If the broker is not enabled in the Database, log the exception but still return the results without SqlDependency. - JHE
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                usingSqlDependency = false;
            }
            return usingSqlDependency;
        }


        /// <summary>
        /// This is a protected helper method to make it easier to execute queries with SqlDependencies. - JHE
        /// This is intended to only be used by this class and other Repositories that extent this class. - JHE
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual SqlUpdatableList<T> LoadListWithSqlDependency(ObjectQuery<T> entityQuery)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    string sql = entityQuery.ToSql();

                    SqlUpdatableList<T> loadedObj = new SqlUpdatableList<T>();

                    IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;

                    bool usingSqlDependency = StartSqlDependencyCache(conn, disableForCommissions: true);

                    try
                    {
                        AdoLoadList(context, loadedObj, sql, conn.ConnectionString, usingSqlDependency);
                    }
                    catch (Exception ex)
                    {
                        ex.Log(Constants.NetStepsExceptionType.NetStepsDataException, internalMessage: string.Format("Error loading list with Dependency. Sql: {1}", ex.Message, sql));

                        // Load without SqlDependency this time, due to some Kind of error with SqlDependency - JHE
                        if (ex.Message == "When using SqlDependency without providing an options value, SqlDependency.Start() must be called prior to execution of a command added to the SqlDependency instance.")
                        {
                            try
                            {
                                lock (Lock)
                                {
                                    if (DataAccess.SqlDependencyOpenConnectionStrings.ContainsIgnoreCase(conn.ConnectionString))
                                        DataAccess.SqlDependencyOpenConnectionStrings.Remove(conn.ConnectionString);
                                }

                                usingSqlDependency = false;
                                AdoLoadList(context, loadedObj, sql, conn.ConnectionString, usingSqlDependency);
                            }
                            catch (Exception ex2)
                            {
                                ex2.Log(Constants.NetStepsExceptionType.NetStepsDataException, internalMessage: string.Format("Error loading list without Dependency. Sql: {1}", ex2.Message, sql));
                                try
                                {
                                    var list = LoadAllFull();
                                    loadedObj.AddRange(list);
                                    loadedObj.SqlDependency = null;
                                    return loadedObj;
                                }
                                catch (Exception ex4)
                                {
                                    throw new NetStepsDataException(string.Format("Error loading full entity list without Dependency. Ex: {0} Sql: {1}", ex4.Message, sql), ex4);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                var list = LoadAllFull();
                                loadedObj.AddRange(list);
                                loadedObj.SqlDependency = null;
                                return loadedObj;
                            }
                            catch (Exception ex2)
                            {
                                throw new NetStepsDataException(string.Format("Error loading list without Dependency. Ex: {0} Sql: {1}", ex2.Message, sql));
                            }
                        }
                    }

                    if (loadedObj != null)
                        return loadedObj;
                    else
                        throw new NetStepsDataException(string.Format("Error attempting to load list with Dependency. SQL: {0}", sql));
                }
            });
        }

        protected virtual SqlUpdatableItem<T> LoadWithSqlDependency(TContext context, ObjectQuery<T> entityQuery)
        {
            var result = ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                string sql = entityQuery.ToSql();

                SqlUpdatableItem<T> loadedObj = new SqlUpdatableItem<T>();

                IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;

                bool usingSqlDependency = this.StartSqlDependencyCache(conn);

                try
                {
                    AdoLoadItem(context, loadedObj, sql, conn.ConnectionString, usingSqlDependency);
                }
                catch (Exception ex)
                {
                    ex.Log(Constants.NetStepsExceptionType.NetStepsDataException, internalMessage: string.Format("Error loading item with Dependency. Sql: {1}", ex.Message, sql));

                    // Load without SqlDependency this time, due to some Kind of error with SqlDependency - JHE
                    if (ex.Message == "When using SqlDependency without providing an options value, SqlDependency.Start() must be called prior to execution of a command added to the SqlDependency instance.")
                    {
                        lock (Lock)
                        {
                            if (DataAccess.SqlDependencyOpenConnectionStrings.ContainsIgnoreCase(conn.ConnectionString))
                                DataAccess.SqlDependencyOpenConnectionStrings.Remove(conn.ConnectionString);
                        }

                        usingSqlDependency = false;
                        AdoLoadItem(context, loadedObj, sql, conn.ConnectionString, usingSqlDependency);
                    }
                    else
                    {
                        ex.Log(Constants.NetStepsExceptionType.NetStepsDataException, internalMessage: string.Format("Error loading item without Dependency. Sql: {1}", ex.Message, sql));
                    }
                }

                if (loadedObj != null)
                    return loadedObj;
                else
                    throw new NetStepsDataException(string.Format("Error attempting to load item with Dependency. SQL: {0}", sql));

            });
            return result;
        }

        protected virtual SqlUpdatableItem<T> LoadWithSqlDependency(ObjectQuery<T> entityQuery)
        {
            using (var context = CreateContext())
            {
                return LoadWithSqlDependency(context, entityQuery);
            }
        }

        // http://msdn.microsoft.com/en-us/library/bb738523.aspx Transactions - JHE
        /// <summary>
        /// Save Entity from the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Save(T obj)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    Save(obj, context);
                }
            });
        }

        public virtual void SaveBatch(IEnumerable<T> items)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    try
                    {
                        foreach (var obj in items)
                        {
                            try
                            {
                                obj.DisableLazyLoadingRecursive();

                                if (obj.IsModifiedRecursive())
                                    obj.UpdateAuditFieldsRecursive();

                                if (context.Connection.State != ConnectionState.Open) // Manually Open Connection - JHE
                                    context.Connection.Open();

                                ApplyChanges(context, EntitySetName, obj);
                            }
                            catch (Exception ex)
                            {
                                CheckForSpecificException(ex, obj);
                                throw;
                            }
                        }
                        context.SaveChanges();
                        foreach (var obj in items)
                            obj.AcceptEntityChanges();
                    }
                    finally
                    {
                        context.Connection.Close();
                        foreach (var obj in items)
                            obj.EnableLazyLoadingRecursive();
                    }
                }
            });
        }

        /// <summary>
        /// Delete Entity from the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(T obj)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                // TODO: We still need to check and handle the deletes with regards to the Audit trigger
                //  so deletes are recorded in the Audit trail. - JHE

                if (obj.ChangeTracker.State != ObjectState.Added)
                    obj.MarkAsDeleted();
                Save(obj);
            });
        }

        /// <summary>
        /// Delete Entity from the DataBase by Primary Key. - JHE
        /// http://omaralzabir.com/linq_to_sql__delete_an_entity_using_primary_key_only/ - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        public virtual void Delete(TKeyType primaryKey)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    // TODO: Do not remove the code below. Entity Framework doesn't support this Kind of DELETE functionality
                    //  in Entity Sql yet. So for now I will be doing the OLD way of deleting by Primary Key. - JHE
                    var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                    string entitySql = string.Format("DELETE FROM [{0}] WHERE [{1}] = @p0", EntitySetName, entityPrimaryKeyInfo.ColumnName);

                    context.ExecuteStoreCommand(entitySql, primaryKey);
                }
            });
        }

        /// <summary>
        /// Batch Delete Entities from the DataBase by Primary Key. - DES
        /// </summary>
        /// <param name="primaryKey"></param>
        public virtual void DeleteBatch(IEnumerable<TKeyType> primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Count() == 0)
                return;

            ValidatePrimaryKeysForLoad(primaryKeys);

            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                    string entitySql = string.Format("DELETE FROM [{0}] WHERE [{1}] IN ({2})", EntitySetName, entityPrimaryKeyInfo.ColumnName, primaryKeys.Join(","));
                    context.ExecuteStoreCommand(entitySql);
                }
            });
        }

        // TODO: Consider adding a DeleteBatch(List<TKeyType> primaryKeys) method - JHE
        // http://omaralzabir.com/linq_to_sql__delete_an_entity_using_primary_key_only/

        public virtual bool Exists(TKeyType primaryKey)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                    var entityQuery = GetLoadQuery(primaryKey, context).Count();

                    return (entityQuery > 0);
                }
            });
        }

        /// <summary>
        /// Helper method to just execute Entity Sql - JHE
        /// </summary>
        /// <param name="entitySql"></param>
        public virtual void ExecuteStoreCommand(string entitySql)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    context.ExecuteStoreCommand(entitySql);
                }
            });
        }

        /// <summary>
        /// Method to get a random row in a table - JHE
        /// Test this method - JHE
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        public virtual T GetRandomRecord()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    string sql = string.Format("SELECT TOP 1 {0} FROM {1} ORDER BY newid();", PrimaryKeyInfo.ColumnName, EntitySetName);
                    IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
                    IDbCommand dbCommand = null;

                    try
                    {
                        dbCommand = DataAccess.SetCommand(sql, connectionString: conn.ConnectionString);
                        dbCommand.CommandType = CommandType.Text;
                        var result = (TKeyType)Convert.ChangeType(DataAccess.GetValue(dbCommand), typeof(TKeyType));
                        return Load(result);
                    }
                    finally
                    {
                        DataAccess.Close(dbCommand);
                    }
                }
            });
        }

        /// <summary>
        /// Method to get a random row in a table - JHE
        /// Test this method - JHE
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        public virtual T GetRandomRecordFull()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    string sql = string.Format("SELECT TOP 1 {0} FROM {1} ORDER BY newid();", PrimaryKeyInfo.ColumnName, EntitySetName);
                    IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
                    IDbCommand dbCommand = null;

                    try
                    {
                        dbCommand = DataAccess.SetCommand(sql, connectionString: conn.ConnectionString);
                        dbCommand.CommandType = CommandType.Text;
                        var result = (TKeyType)Convert.ChangeType(DataAccess.GetValue(dbCommand), typeof(TKeyType));
                        return LoadFull(result);
                    }
                    finally
                    {
                        DataAccess.Close(dbCommand);
                    }
                }
            });
        }

        /// <summary>
        /// Returns the number of entities in the database.
        /// </summary>
        public virtual int Count()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Count();
                }
            });
        }

        /// <summary>
        /// Returns the number of entities in the database that satisfy a condition.
        /// </summary>
        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Count(predicate);
                }
            });
        }

        /// <summary>
        /// Deterss whether the database contains any entities.
        /// </summary>
        public virtual bool Any()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Any();
                }
            });
        }

        /// <summary>
        /// Determines whether the database contains any entities that satisfy a condition.
        /// </summary>
        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Any(predicate);
                }
            });
        }

        /// <summary>
        /// Returns a filtered list of entities.
        /// </summary>
        public virtual List<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return Where(predicate, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Returns a filtered list of entities including specified related objects.
        /// </summary>
        public virtual List<T> Where(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (includes == null)
            {
                throw new ArgumentNullException("includes");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    ObjectQuery<T> query = context.CreateObjectSet<T>();
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                    return query
                        .Where(predicate)
                        .ToList();
                }
            });
        }

        /// <summary>
        /// Returns a filtered and projected list of entities.
        /// </summary>
        public virtual List<TSelected> WhereSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Where(predicate)
                        .Select(selector)
                        .ToList();
                }
            });
        }

        /// <summary>
        /// Returns the first entity that satisfies the specified condition, or null if no such entity is found.
        /// </summary>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return FirstOrDefault(predicate, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Returns the first entity that satisfies the specified condition, or null if no such entity is found, and includes specified related objects.
        /// </summary>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (includes == null)
            {
                throw new ArgumentNullException("includes");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    ObjectQuery<T> query = context.CreateObjectSet<T>();
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                    return query
                        .Where(predicate)
                        .FirstOrDefault();
                }
            });
        }

        /// <summary>
        /// Projects and returns the first entity that satisfies the specified condition, or returns null if no such entity is found.
        /// </summary>
        public virtual TSelected FirstOrDefaultSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    return context
                        .CreateObjectSet<T>()
                        .Where(predicate)
                        .Select(selector)
                        .FirstOrDefault();
                }
            });
        }

        /// <summary>
        /// Projects and returns the first entity that satisfies the specified condition, or returns null if no such entity is found.
        /// </summary>
        public virtual TSelected FirstOrDefaultSelect<TSelected>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSelected>> selector, string orderBy, bool descending)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var result = context
                            .CreateObjectSet<T>()
                            .Where(predicate);

                    if (descending)
                        result.OrderByDescending(orderBy);
                    else
                        result.OrderBy(orderBy);

                    return result.Select(selector).FirstOrDefault();
                }
            });
        }
        #endregion

        #region Audit Trails
        /// <summary>
        /// This is the New LINQ prototype to replace the usp_get_audit_trail proc implementation above. 
        /// It was originally done as a proc for the Left Join which we need to make sure the bottom Entity's
        /// implementation fixes. - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public virtual PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = primaryKey
            });
            return GetAuditLog(list, searchParameters);
        }
        protected virtual PaginatedList<AuditLogRow> GetAuditLog(IEnumerable<AuditTableValueItem> tableValueList, AuditLogSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    if (typeof(TContext) != typeof(NetStepsEntities))
                        throw new Exception("This method is only supported on the NetStepsEntities right now.");

                    NetStepsEntities nContext = context as NetStepsEntities;

                    PaginatedList<AuditLogRow> result = new PaginatedList<AuditLogRow>(searchParameters);

                    var matchingAuditLogs = from a in nContext.AuditLogs
                                            join at in nContext.AuditTables on a.AuditTableID equals at.AuditTableID
                                            join act in nContext.AuditChangeTypes on a.AuditChangeTypeID equals act.AuditChangeTypeID
                                            join am in nContext.AuditMachineNames on a.AuditMachineNameID equals am.AuditMachineNameID
                                            join asu in nContext.AuditSqlUserNames on a.AuditSqlUserNameID equals asu.AuditSqlUserNameID
                                            join atc in nContext.AuditTableColumns on a.AuditTableColumnID equals atc.AuditTableColumnID
                                            join u in nContext.Users on a.UserID equals u.UserID into u_t
                                            from u in u_t.DefaultIfEmpty()
                                            select new
                                            {
                                                ChangeType = act.Name,
                                                TableName = at.Name,
                                                a.PK,
                                                atc.ColumnName,
                                                a.OldValue,
                                                a.NewValue,
                                                a.DateChanged,
                                                u.Username,
                                                SqlUserName = asu.Name,
                                                MachineName = am.Name,
                                                a.ApplicationID
                                            };

                    if (!searchParameters.TableName.IsNullOrEmpty())
                        matchingAuditLogs = matchingAuditLogs.Where(m => m.TableName.Contains(searchParameters.TableName));
                    if (!searchParameters.ColumnName.IsNullOrEmpty())
                        matchingAuditLogs = matchingAuditLogs.Where(m => m.ColumnName.Contains(searchParameters.ColumnName));
                    if (searchParameters.PK.HasValue && searchParameters.PK.Value != 0)
                        matchingAuditLogs = matchingAuditLogs.Where(m => m.PK == searchParameters.PK.Value);

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingAuditLogs = matchingAuditLogs.Where(a => a.DateChanged >= startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingAuditLogs = matchingAuditLogs.Where(a => a.DateChanged <= endDateUTC);
                    }

                    // Create and/or statement for all items in tableValueList to include Audit Logs on all Children Entities and Collections. - JHE
                    var predicate = PredicateBuilder.False(matchingAuditLogs);
                    int count = 0;
                    foreach (var item in tableValueList)
                    {
                        string tableName = item.TableName;
                        int pk = item.PrimaryKey;

                        var exp = matchingAuditLogs.GetExpression(a => a.TableName == tableName && a.PK == pk);
                        if (count == 0)
                            predicate = exp;
                        else
                        {
                            var newExp = exp.Or(predicate);
                            predicate = newExp;
                        }
                        count++;
                    }
                    matchingAuditLogs = matchingAuditLogs.Where(predicate);


                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                        if (searchParameters.OrderByDirection == Constants.SortDirection.Ascending)
                            matchingAuditLogs = matchingAuditLogs.OrderBy(searchParameters.OrderBy);
                        else
                            matchingAuditLogs = matchingAuditLogs.OrderByDescending(searchParameters.OrderBy);
                    }
                    else
                        matchingAuditLogs = matchingAuditLogs.OrderByDescending(a => a.DateChanged);

                    result.TotalCount = matchingAuditLogs.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                        matchingAuditLogs = matchingAuditLogs.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    foreach (var a in matchingAuditLogs.ToList())
                        result.Add(new AuditLogRow()
                        {
                            ChangeType = a.ChangeType,
                            TableName = a.TableName,
                            PK = a.PK,
                            ColumnName = GetMeaningfulAuditColumnName(a.TableName, a.ColumnName, a.OldValue),
                            OldValue = GetMeaningfulAuditValue(a.TableName, a.ColumnName, a.OldValue),
                            NewValue = GetMeaningfulAuditValue(a.TableName, a.ColumnName, a.NewValue),
                            DateChanged = a.DateChanged,
                            Username = a.Username,
                            SqlUserName = a.SqlUserName,
                            MachineName = a.MachineName,
                            ApplicationName = SmallCollectionCache.Instance.Applications.GetById(a.ApplicationID).Name,
                        });

                    return result;
                }
            });
        }
        protected virtual string GetMeaningfulAuditValue(string tableName, string columnName, string value)
        {
            try
            {
                if (!value.IsValidInt() && !value.IsValidDateTime())
                    return value;

                if (columnName == "AccountStatusChangeReasonID")
                {
                    var item = SmallCollectionCache.Instance.AccountStatusChangeReasons.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName == "AccountStatusID")
                {
                    var item = SmallCollectionCache.Instance.AccountStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "AccountTypeID")
                {
                    var item = SmallCollectionCache.Instance.AccountTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "GenderID")
                {
                    var item = SmallCollectionCache.Instance.Genders.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "DefaultLanguageID")
                {
                    var item = SmallCollectionCache.Instance.Languages.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderStatusID")
                {
                    var item = SmallCollectionCache.Instance.OrderStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderPaymentStatusID")
                {
                    var item = SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "BillingCountryID" || columnName == "CountryID")
                {
                    var item = SmallCollectionCache.Instance.Countries.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "BillingStateProvinceID" || columnName == "StateProvinceID")
                {
                    var item = SmallCollectionCache.Instance.StateProvinces.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName == "PaymentTypeID")
                {
                    var item = SmallCollectionCache.Instance.PaymentTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderShipmentStatusID")
                {
                    var item = SmallCollectionCache.Instance.OrderShipmentStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ShippingMethodID")
                {
                    var item = SmallCollectionCache.Instance.ShippingMethods.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName == "OrderItemParentTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderItemParentTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderCustomerTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderCustomerTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ProductPriceTypeID")
                {
                    var item = SmallCollectionCache.Instance.ProductPriceTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderItemTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderItemTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "CurrencyID")
                {
                    var item = SmallCollectionCache.Instance.Currencies.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ReturnTypeID")
                {
                    var item = SmallCollectionCache.Instance.ReturnTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ReturnReasonID")
                {
                    var item = SmallCollectionCache.Instance.ReturnReasons.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderPaymentStatusID")
                {
                    var item = SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderCustomerTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderCustomerTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "CountryID")
                {
                    var item = SmallCollectionCache.Instance.Countries.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "PaymentTypeID")
                {
                    var item = SmallCollectionCache.Instance.PaymentTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ProductPriceTypeID")
                {
                    var item = SmallCollectionCache.Instance.ProductPriceTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderShipmentStatusID")
                {
                    var item = SmallCollectionCache.Instance.OrderShipmentStatuses.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "OrderItemTypeID")
                {
                    var item = SmallCollectionCache.Instance.OrderItemTypes.GetById(value.ToShort());
                    return (item != null) ? item.GetTerm() : string.Empty;
                }
                else if (columnName == "ShippingMethodID")
                {
                    var item = SmallCollectionCache.Instance.ShippingMethods.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName == "CreatedByUserID" || columnName == "ModifiedByUserID" || columnName == "UserID")
                {
                    var item = CachedData.GetUser(value.ToInt());
                    return (item != null) ? item.Username : string.Empty;
                }
                else if (columnName == "ModifiedByApplicationID")
                {
                    var item = SmallCollectionCache.Instance.Applications.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName == "UserStatusID")
                {
                    var item = SmallCollectionCache.Instance.UserStatuses.GetById(value.ToShort());
                    return (item != null) ? item.Name : string.Empty;
                }
                else if (columnName.EndsWith("UTC"))
                {
                    // Change DateTimes to local for end users - JHE
                    DateTime? date = value.ToDateTimeNullable();
                    if (date.HasValue)
                        return date.UTCToLocal(TimeZoneInfo.Local).ToString();
                    else
                        return value;
                }
                else
                    return value;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                return value;
            }
        }

        protected virtual string GetMeaningfulAuditColumnName(string tableName, string columnName, string value)
        {
            try
            {
                if (columnName == "CreatedByUserID" || columnName == "ModifiedByUserID" || columnName == "UserID" || columnName == "ReturnReasonID" ||
                   columnName == "ReturnTypeID" || columnName == "CurrencyID" || columnName == "OrderTypeID" || columnName == "OrderStatusID" ||
                   columnName == "DefaultLanguageID" || columnName == "GenderID" || columnName == "AccountTypeID" || columnName == "AccountStatusID" ||
                   columnName == "AccountStatusChangeReasonID" || columnName == "OrderPaymentStatusID" || columnName == "OrderCustomerTypeID" ||
                   columnName == "CountryID" || columnName == "OrderShipmentStatusID" || columnName == "PaymentTypeID" || columnName == "ProductPriceTypeID" ||
                   columnName == "OrderItemTypeID" || columnName == "ShippingMethodID")
                {
                    return columnName.SubstringSafe(0, columnName.Length - 2);
                }
                else if (columnName == "ModifiedByApplicationID")
                {
                    return "ModifiedByApplication";
                }
                else if (columnName.EndsWith("UTC"))
                {
                    if (value.IsValidDateTime() || value.IsNullOrEmpty())
                        return columnName.SubstringSafe(0, columnName.Length - 3);
                    else
                        return columnName;
                }
                else
                    return columnName;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                return value;
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// This method makes sure that the primaryKey passed is of an 'Integer' type and that it is greater than 0. - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        protected void ValidatePrimaryKeyForLoad(TKeyType primaryKey)
        {
            if (typeof(TKeyType) == typeof(int) || typeof(TKeyType) == typeof(Int64) || typeof(TKeyType) == typeof(short))
                if (Convert.ToInt64(primaryKey) == 0)
                    throw new NetStepsDataException(string.Format("Error loading Entity: {0}. Invalid primaryKey: {1}. Must be greater than 0.", EntityType.Name, primaryKey));
        }

        protected void ValidatePrimaryKeysForLoad(IEnumerable<TKeyType> primaryKeys)
        {
            if (typeof(TKeyType) == typeof(int) || typeof(TKeyType) == typeof(Int64) || typeof(TKeyType) == typeof(short))
            {
                foreach (var primaryKey in primaryKeys)
                {
                    if (Convert.ToInt64(primaryKey) == 0)
                        throw new NetStepsDataException(string.Format("Error loading Entity: {0}. Invalid primaryKey: {1}. Must be greater than 0.", EntityType.Name, primaryKey));
                }
            }
        }

        protected T GetSingleResult(TKeyType primaryKey, Func<TKeyType, TContext, ObjectQuery<T>> getQueryMethod)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityQuery = getQueryMethod(primaryKey, context);

                    using (var result = entityQuery.Execute(DefaultLoadMergeOption))
                    {
                        var first = result.FirstOrDefault();
                        if (first == null)
                        {
                            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                            throw new NetStepsDataException(string.Format("No {2} found with {0} = {1}.", entityPrimaryKeyInfo.ColumnName, primaryKey, EntityType.Name));
                        }
                        return first;
                    }
                }
            });
        }

        protected T ExecuteQueryForSingleResult(TContext context, TKeyType primaryKey, ObjectQuery<T> entityQuery)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(entityQuery != null);
            Contract.Ensures(Contract.Result<T>() != null);

            ValidatePrimaryKeyForLoad(primaryKey);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var result = entityQuery.Execute(DefaultLoadMergeOption))
                {
                    var first = result.FirstOrDefault();
                    if (first == null)
                    {
                        var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                        throw new NetStepsDataException(string.Format("No {2} found with {0} = {1}.", entityPrimaryKeyInfo.ColumnName, primaryKey, EntityType.Name));
                    }
                    return first;
                }
            });
        }

        protected T ExecuteQueryForSingleResult(TKeyType primaryKey, ObjectQuery<T> entityQuery)
        {
            Contract.Requires<ArgumentNullException>(entityQuery != null);
            Contract.Ensures(Contract.Result<T>() != null);
            
            using (TContext context = CreateContext())
            {
                return ExecuteQueryForSingleResult(context, primaryKey, entityQuery);
            }
        }

        protected List<T> GetListResults(Func<TContext, ObjectQuery<T>> getQueryMethod)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityQuery = getQueryMethod(context);

                    using (var result = entityQuery.Execute(DefaultLoadMergeOption))
                    {
                        var list = result.ToList<T>();
                        return list;
                    }
                }
            });
        }

        protected List<T> GetListResults(IEnumerable<TKeyType> primaryKeys, Func<IEnumerable<TKeyType>, TContext, ObjectQuery<T>> getQueryMethod)
        {
            foreach (var primaryKey in primaryKeys)
                ValidatePrimaryKeyForLoad(primaryKey);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (TContext context = CreateContext())
                {
                    var entityQuery = getQueryMethod(primaryKeys, context);

                    using (var result = entityQuery.Execute(DefaultLoadMergeOption))
                    {
                        var list = result.ToList<T>();
                        if (list.Count == 0)
                            throw new NetStepsDataException(string.Format("1 or more IDs not found ({0}): {1}.", EntityType.Name, primaryKeys));
                        else
                            return list;
                    }
                }
            });
        }

        // TODO: Create a bulk save method in 1 transaction - JHE

        /// <summary>
        /// This is must be called within a TContext.
        /// This should not be called directly. Intended to be called with the repository classes. - JHE
        /// </summary>
        /// <param name="obj"></param>
        internal virtual void Save(T obj, TContext context)
        {
            var guid = Guid.NewGuid();
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
                IObjectWithChangeTrackerExtensions.GetAllChangeTrackerItems(obj, allTrackerItems, true, true);

                var extensionProviderRegistry = Create.New<IDataObjectExtensionProviderRegistry>();
                var extensibleTrackerItems = allTrackerItems
                    .Where(x => x is IExtensibleDataObject)
                    .Cast<IExtensibleDataObject>()
                    .Select(x => new
                    {
                        Item = x,
                        Provider = extensionProviderRegistry.RetrieveExtensionProvider(x.ExtensionProviderKey)
                    })
                    .ToList();

                // TODO: Consider wrapping this in a retry handler http://msdn.microsoft.com/en-us/library/bb738523.aspx
                // Disabled until we have DTC enabled - Lundy
                //using (var transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    // Process deleted extensions
                    foreach (var deletedExtensibleTrackerItem in extensibleTrackerItems.Where(x => ((IObjectWithChangeTracker)x.Item).ChangeTracker.State == ObjectState.Deleted))
                    {
                        deletedExtensibleTrackerItem.Provider.DeleteDataObjectExtension(deletedExtensibleTrackerItem.Item);
                    }

                    try
                    {
                        // Process object changes
                        obj.DisableLazyLoadingRecursive(allTrackerItems);
                        if (obj.IsModifiedRecursive(allTrackerItems))
                        {
                            obj.UpdateAuditFieldsRecursive(allTrackerItems);
                        }
                        if (context.Connection.State != ConnectionState.Open)
                        {
                            context.Connection.Open();
                        }
                        ApplyChanges(context, EntitySetName, obj);
                        context.SaveChanges(SaveOptions.None);
                    }
                    catch (OptimisticConcurrencyException ex)
                    {
                        try
                        {
                            // http://msdn.microsoft.com/en-us/library/bb738618.aspx#Y379 - JHE
                            // http://msdn.microsoft.com/en-us/library/bb896255.aspx - JHE
                            // Resolve the concurrency conflict by refreshing the 
                            // object context before re-saving changes. 
                            context.Refresh(RefreshMode.ClientWins, obj);

                            // Save changes.
                            context.SaveChanges(SaveOptions.None);
                        }
                        catch (Exception ex2)
                        {
                            CheckForSpecificException(ex2, obj);
                            EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, internalMessage: ("Guid:" + guid));
                            EntityExceptionHelper.GetAndLogNetStepsException(ex2, Constants.NetStepsExceptionType.NetStepsBusinessException, internalMessage: ("Guid:" + guid));
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        CheckForSpecificException(ex, obj);
                        EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, internalMessage: ("Guid:" + guid));
                        throw;
                    }
                    finally
                    {
                        if (context.Connection.State != ConnectionState.Closed)
                        {
                            context.Connection.Close();
                        }
                        obj.EnableLazyLoadingRecursive();
                    }

                    // Process extensions
                    foreach (var extensibleTrackerItem in extensibleTrackerItems)
                    {
                        switch (((IObjectWithChangeTracker)extensibleTrackerItem.Item).ChangeTracker.State)
                        {
                            case ObjectState.Added:
                                extensibleTrackerItem.Provider.SaveDataObjectExtension(extensibleTrackerItem.Item);
                                break;
                            case ObjectState.Deleted:
                                // do nothing (unless it's a parent object?)
                                break;
                            case ObjectState.Modified:
                                extensibleTrackerItem.Provider.UpdateDataObjectExtension(extensibleTrackerItem.Item);
                                break;
                            case ObjectState.Unchanged:
                                if (extensibleTrackerItem.Item.Extension != null)
                                {
								extensibleTrackerItem.Provider.UpdateDataObjectExtension(extensibleTrackerItem.Item);
                                }
                                break;
                        }
                    }

                    // Disabled until we have DTC enabled - Lundy
                    //transaction.Complete();
                }

                // The transaction is now closed. Reset the change trackers.
                obj.AcceptEntityChanges(allTrackerItems);
            });
        }

        internal virtual void CheckForSpecificException(Exception ex, T obj)
        {
            if (ex.Message.Contains("AcceptChanges cannot continue because the object's key values conflict with another object in the ObjectStateManager."))
            {
                NetStepsDataException netStepsException = new NetStepsDataException(ex);
                List<IObjectWithChangeTracker> duplicateEntities = obj.FindDuplicateEntitiesInObjectGraph();

                string message = string.Empty;
                var distinctDuplicateEntities = duplicateEntities.Distinct<IObjectWithChangeTracker>(new LambdaComparer<IObjectWithChangeTracker>((x, y) => x.GetType().Name == y.GetType().Name && (x as IListValue).ID == (y as IListValue).ID, (x) => x.GetType().Name.GetHashCode() + (x as IListValue).ID.GetHashCode()));
                foreach (var item in distinctDuplicateEntities)
                    message += string.Format("{0} ID:({1}), ", item.GetType().Name, (item as IListValue).ID);

                if (message.EndsWith(", "))
                    message = message.Substring(0, message.Length - 2);

                netStepsException.PublicMessage = string.Format("Error saving object graph. There are multiple entities with that same key in the object graph: {0}", message);
                throw netStepsException;
            }
        }

        protected virtual void AdoLoadList(TContext context, SqlUpdatableList<T> loadedObj, string sql, string connectionString, bool usingSqlDependency)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                DbCommand dbCommand = new ProfiledDbCommand(command, sqlConnection, MiniProfiler.Current);
                if (usingSqlDependency)
                    loadedObj.SqlDependency = new SqlDependency(command);
                using (DbDataReader reader = dbCommand.ExecuteReader())
                {
                    var results = context.Translate<T>(reader).ToList();
                    foreach (var item in results)
                        loadedObj.Add(item);
                    reader.Close();
                }
                sqlConnection.Close();
            }
        }

        protected virtual void AdoLoadItem(TContext context, SqlUpdatableItem<T> loadedObj, string sql, string connectionString, bool usingSqlDependency)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                DbCommand dbCommand = new ProfiledDbCommand(command, sqlConnection, MiniProfiler.Current);

                if (usingSqlDependency)
                    loadedObj.SqlDependency = new SqlDependency(command);
                using (DbDataReader reader = dbCommand.ExecuteReader())
                {
                    var results = context.Translate<T>(reader).ToList();
                    foreach (var item in results)
                        loadedObj.Item = item;
                }
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Calls the appropriate ApplyChanges method based on the given ObjectContext.
        /// </summary>
        /// <param name="context">The ObjectContext to which changes will be applied.</param>
        /// <param name="entitySetName">The EntitySet name of the entity.</param>
        /// <param name="entity">The entity serving as the entry point of the object graph that contains changes.</param>
        private void ApplyChanges(TContext context, string entitySetName, T entity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentException("String parameter cannot be null or empty.", "entitySetName");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (context is NetSteps.Data.Entities.NetStepsEntities)
            {
                NetSteps.Data.Entities.SelfTrackingEntitiesContextExtensions.ApplyChanges(context, entitySetName, entity);
            }
            else if (context is NetSteps.Data.Entities.Mail.MailEntities)
            {
                NetSteps.Data.Entities.Mail.SelfTrackingEntitiesContextExtensions.ApplyChanges(context, EntitySetName, entity);
            }
            else
            {
                throw new Exception("Unable to locate the matching ApplyChanges method for the given ObjectContext.");
            }
        }
        #endregion

        #region Get Query Methods
        protected ObjectQuery<T> GetLoadQuery(TKeyType primaryKey, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o WHERE o.{2} = {3}", typeof(TContext).Name, EntitySetName, entityPrimaryKeyInfo.ColumnName, primaryKey);
            var entityQuery = context.CreateQuery<T>(entitySql);
            return entityQuery;
        }

        protected ObjectQuery<T> GetLoadQueryWithTranslations(TKeyType primaryKey, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o WHERE o.{2} = {3}", typeof(TContext).Name, EntitySetName, entityPrimaryKeyInfo.ColumnName, primaryKey);
            var entityQuery = context.CreateQuery<T>(entitySql);
            entityQuery = entityQuery.Include("Translations");
            return entityQuery;
        }

        /// <summary>
        /// Uses/required overridden loadAllFullQuery (implemented 'overridden' on the Extending Repository) to create query. - JHE
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ObjectQuery<T> GetLoadFullQuery(TKeyType primaryKey, TContext context)
        {
            return GetLoadFullQuery(primaryKey, context, loadAllFullQuery).ToObjectQuery();
        }
        protected ObjectQuery<T> GetLoadFullQuery(TKeyType primaryKey, TContext context, Func<TContext, IQueryable<T>> query)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            var newQuery = query(context);
            string pkName = entityPrimaryKeyInfo.ColumnName;

            var predicate = ExpressionHelper.MakeExpression<T, bool>(pkName, ComparisonType.Equal, primaryKey);
            newQuery = newQuery.Where(predicate);
            return newQuery.ToObjectQuery();
        }

        /// <summary>
        /// Uses/required overridden loadFullQuery (implemented 'overridden' on the Extending Repository) to create query. - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ObjectQuery<T> GetLoadFullQueryOverridden(TKeyType primaryKey, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            var newQuery = loadFullQuery(context, primaryKey);
            return newQuery.ToObjectQuery();
        }

        protected ObjectQuery<T> GetLoadALLQuery(TContext context)
        {
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o", typeof(TContext).Name, EntitySetName);
            var entityQuery = context.CreateQuery<T>(entitySql);
            return entityQuery;
        }

        protected ObjectQuery<T> GetLoadALLQueryWithTranslations(TContext context)
        {
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o", typeof(TContext).Name, EntitySetName);
            var entityQuery = context.CreateQuery<T>(entitySql);
            entityQuery = entityQuery.Include("Translations");
            return entityQuery;
        }

        protected ObjectQuery<T> GetLoadALLFullQuery(TContext context)
        {
            var newQuery = loadAllFullQuery(context);
            return newQuery.ToObjectQuery();
        }

        protected ObjectQuery<T> GetLoadBatchQuery(IEnumerable<TKeyType> primaryKeys, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            string inClauseIds = "{" + primaryKeys.ToCommaSeparatedString().RemoveSpaces() + "}";
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o WHERE o.{2} IN {3}", typeof(TContext).Name, EntitySetName, entityPrimaryKeyInfo.ColumnName, inClauseIds);
            var entityQuery = context.CreateQuery<T>(entitySql);
            return entityQuery;
        }

        protected ObjectQuery<T> GetLoadBatchQueryWithTranslations(IEnumerable<TKeyType> primaryKeys, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            string inClauseIds = "{" + primaryKeys.ToCommaSeparatedString().RemoveSpaces() + "}";
            string entitySql = string.Format("SELECT VALUE o FROM {0}.{1} as o WHERE o.{2} IN {3}", typeof(TContext).Name, EntitySetName, entityPrimaryKeyInfo.ColumnName, inClauseIds);
            var entityQuery = context.CreateQuery<T>(entitySql);
            entityQuery = entityQuery.Include("Translations");
            return entityQuery;
        }

        /// <summary>
        /// This required loadAllFullQuery to be implemented (overridden) on the Extending Repository. - JHE
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ObjectQuery<T> GetLoadBatchFullQuery(IEnumerable<TKeyType> primaryKeys, TContext context)
        {
            var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
            var newQuery = loadAllFullQuery(context);
            string pkName = entityPrimaryKeyInfo.ColumnName;

            var predicate = PredicateBuilder.False(newQuery);
            int count = 0;
            foreach (var primaryKey in primaryKeys)
            {
                var exp = ExpressionHelper.MakeExpression<T, bool>(pkName, ComparisonType.Equal, primaryKey);
                if (count == 0)
                    predicate = exp;
                else
                {
                    var newExp = exp.Or(predicate);
                    predicate = newExp;
                }
                count++;
            }
            newQuery = newQuery.Where(predicate);
            return newQuery.ToObjectQuery();
        }
        #endregion
    }
}
