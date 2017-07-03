using System;
using System.Collections.Generic;
using System.Diagnostics;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Data.Entities.Business.Logic
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Base Logic class for all Entities to contain base/default logic for Entities.
    /// Created: 03-30-2010
    /// </summary>
    [Serializable]
    public class BusinessLogicBase<T, TKeyType, TRepository, TBusinessLogic> : IBusinessEntityLogic<T, TKeyType, TRepository>
        where T : EntityBusinessBase<T, TKeyType, TRepository, TBusinessLogic>, IObjectWithChangeTracker
        where TRepository : IBaseRepository<T, TKeyType>, IRepository
        where TBusinessLogic : IBusinessEntityLogic<T, TKeyType, TRepository>
    {
        static TraceSource ts = new TraceSource("traceSource");

        #region Properties
        /// <summary>
        /// Override this so the GetById method will work:
        /// Example:
        ///     protected override Func<AccountStatus, Int32> GetIdColumn
        ///     {
        ///         get
        ///         {
        ///             return i => i.AccountStatusID;
        ///         }
        ///     }
        /// </summary>
        public virtual Func<T, TKeyType> GetIdColumnFunc
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// This should be overridden to provide a name/title to represent the Entity in a list of Entities. - JHE
        /// Example:
        ///     public override Action<AccountStatus, Int32> SetIdColumnFunc
        ///     {
        ///         get
        ///         {
        ///             return (AccountStatus i, int id) => i.AccountStatusID = id;
        ///         }
        ///     }
        /// </summary>
        public virtual Action<T, TKeyType> SetIdColumnFunc
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// This should be overridden to provide a name/title to represent the Entity in a list of Entities. - JHE
        /// Example:
        ///     protected override Func<AccountStatus, string> GetTitleColumn
        ///     {
        ///         get
        ///         {
        ///             return i => i.Name;
        ///         }
        ///     }
        /// </summary>
        public virtual Func<T, string> GetTitleColumnFunc
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// This should be overridden to provide a name/title to represent the Entity in a list of Entities. - JHE
        /// Example:
        ///     public override Action<AccountStatus, string> SetTitleColumnFunc
        ///     {
        ///         get
        ///         {
        ///             return (AccountStatus i, string title) => i.Name = title;
        ///         }
        ///     }
        /// </summary>
        public virtual Action<T, string> SetTitleColumnFunc
        {
            get
            {
                return null;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// This method is called in the construction of a new Entity to default values of the class. - JHE
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entity"></param>
        public virtual void DefaultValues(TRepository repository, T entity)
        {
            // This is where the default values of the object when a new entity is created. - JHE
        }

        /// <summary>
        /// This will 'Shallow' load an entity (only the entity; not child collections or child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T Load(TRepository repository, TKeyType primaryKey)
        {
            using (var loadAllFullTrace = this.TraceActivity(string.Format("Load for {0} {1}", this.GetType(), primaryKey)))
            {
                if (repository != null)
                {
                    var type = repository.GetType();
                    var assembly = type.Assembly;
                    repository.TraceInformation(string.Format("repository type: {0}", type));
                    repository.TraceInformation(string.Format("repository location: {0}", assembly.CodeBase));
                    repository.TraceInformation(string.Format("repository assembly fullname: {0}", assembly.FullName));
                }
                else
                {
                    this.TraceError("repository was null");
                }

                var obj = repository.Load(primaryKey);
                if (obj != null)
                {
                    obj.StartEntityTrackingAndEnableLazyLoading();
                }
                return obj;
            }
        }

        /// <summary>
        /// This will load an entire entity (the entity, child collections and child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T LoadFull(TRepository repository, TKeyType primaryKey)
        {
            using (var loadAllFullTrace = this.TraceActivity(string.Format("LoadFull for {0} {1}", this.GetType(), primaryKey)))
            {
                if (repository != null)
                {
                    var type = repository.GetType();
                    var assembly = type.Assembly;
                    repository.TraceInformation(string.Format("repository type: {0}", type));
                    repository.TraceInformation(string.Format("repository location: {0}", assembly.CodeBase));
                    repository.TraceInformation(string.Format("repository assembly fullname: {0}", assembly.FullName));
                }
                else
                {
                    this.TraceError("repository was null");
                }

                var obj = repository.LoadFull(primaryKey);
                obj.StartEntityTrackingAndEnableLazyLoading();
                return obj;
            }
        }

        /// <summary>
        /// This will 'Shallow' load all entities in the DB table (only the entity; not child collections or child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual List<T> LoadAll(TRepository repository)
        {
            using (var loadAllFullTrace = this.TraceActivity(string.Format("LoadAll for {0}", this.GetType())))
            {
                if (repository != null)
                {
                    var type = repository.GetType();
                    var assembly = type.Assembly;
                    repository.TraceInformation(string.Format("repository type: {0}", type));
                    repository.TraceInformation(string.Format("repository location: {0}", assembly.CodeBase));
                    repository.TraceInformation(string.Format("repository assembly fullname: {0}", assembly.FullName));
                }
                else
                {
                    this.TraceError("repository was null");
                }

                var obj = repository.LoadAll();
                foreach (var item in obj)
                {
                    item.StartEntityTrackingAndEnableLazyLoading();
                }
                return obj;
            }
        }

        /// <summary>
        /// This will load load all entities in the DB table (the entity, child collections and child entities). - JHE
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual List<T> LoadAllFull(TRepository repository)
        {
            using (var loadAllFullTrace = this.TraceActivity(string.Format("LoadAllFull for {0}", this.GetType())))
            {
                if (repository != null)
                {
                    var type = repository.GetType();
                    var assembly = type.Assembly;
                    repository.TraceInformation(string.Format("repository type: {0}", type));
                    repository.TraceInformation(string.Format("repository location: {0}", assembly.CodeBase));
                    repository.TraceInformation(string.Format("repository assembly fullname: {0}", assembly.FullName));
                }
                else
                {
                    this.TraceError("repository was null");
                }

                var obj = repository.LoadAllFull();
                foreach (var item in obj)
                {
                    item.StartEntityTrackingAndEnableLazyLoading();
                }
                return obj;
            }
        }

        /// <summary>
        /// Save Entity from the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Save(TRepository repository, T entity)
        {
            using (var saveTrace = this.TraceActivity(string.Format("save {0}", entity.GetType())))
            {
                repository.Save((T)entity);
                entity.StartEntityTracking();
            }
        }

        /// <summary>
        /// Delete Entity from the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(TRepository repository, T entity)
        {
            repository.Delete(entity);
        }

        /// <summary>
        /// Delete Entity from the DataBase by Primary Key. - JHE
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(TRepository repository, TKeyType primaryKey)
        {
            repository.Delete(primaryKey);
        }

        public virtual PaginatedList<AuditLogRow> GetAuditLog(TRepository repository, int primaryKey, AuditLogSearchParameters param)
        {
            try
            {
                return repository.GetAuditLog(primaryKey, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
            }
        }

        public virtual void StartEntityTracking(T entity)
        {
            (entity as IObjectWithChangeTracker).StartTrackingRecursive();
        }

        public virtual void StartEntityTrackingAndEnableLazyLoading(T entity)
        {
            (entity as IObjectWithChangeTracker).StartTrackingAndEnableLazyLoadingRecursive();
        }

        public virtual void StopEntityTracking(T entity)
        {
            (entity as IObjectWithChangeTracker).StopTrackingRecursive();
        }

        public virtual void AcceptChanges(T entity, List<IObjectWithChangeTracker> allTrackerItems = null)
        {
            //(entity as IObjectWithChangeTracker).AcceptChanges();
            entity.AcceptAllChangesRecursive(); // For Recursive acceptance of all changes. - JHE
        }

        public virtual void Validate(T entity)
        {
            entity.Validate();
        }

        public virtual bool IsValid(T entity)
        {
            return entity.IsValid;
        }

        public virtual void AddValidationRules(T entity)
        {

        }

        /// <summary>
        /// This is a collection of property names that can be ignored on child objects when validating a parent due to the fact that
        /// Entity Framework will set their values on save. - JHE
        /// Example for Saving and Order: OrderID (gets set on child Entities)
        /// </summary>
        /// <returns></returns>
        public virtual List<string> ValidatedChildPropertiesSetByParent(TRepository repository)
        {
            List<string> list = new List<string>();

            // By default, adding the primary key of current Entity, since this is the most likely property to connect child Entities. - JHE
            var primaryKeyInfo = repository.PrimaryKeyInfo;
            if (primaryKeyInfo != null)
                list.Add(primaryKeyInfo.ColumnName);

            return list;
        }

        /// <summary>
        /// Override this method to trim strings, ect.. on data in an object before save - JHE
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entity"></param>
        public virtual void CleanDataBeforeSave(TRepository repository, T entity)
        {
            // TODO: Override this method to trim strings, ect.. on data in an object before save - JHE
        }

        public virtual void UpdateCreatedFields(TRepository repository, T entity)
        {
            Audit.UpdateCreatedFields(entity);
        }
        #endregion
    }
}
