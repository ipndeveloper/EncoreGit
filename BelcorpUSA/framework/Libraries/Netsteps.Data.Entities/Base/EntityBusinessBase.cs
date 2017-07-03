using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Data.Entities.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Base class for business object to provide common functionality across all of our objects.
	/// Created: 03-17-2009
	/// </summary>
	[Serializable]
	public abstract partial class EntityBusinessBase<T, TKeyType, TRepository, TBusinessLogic> : CloneableBase<T>, IValidation, IDataErrorInfo, IKeyName<T, TKeyType>, IListValue, IDelete, IObjectWithChangeTrackerBusiness, IIsLazyLoadingEnabled, ICleanDataBeforeSave
		where T : EntityBusinessBase<T, TKeyType, TRepository, TBusinessLogic>, IObjectWithChangeTracker
		where TRepository : IBaseRepository<T, TKeyType>, IRepository
		where TBusinessLogic : IBusinessEntityLogic<T, TKeyType, TRepository>
	{
		[NonSerialized]
		private TRepository _repository;
		[NonSerialized]
		private TBusinessLogic _businessLogic;

		#region Properties
		/// <summary>
		/// Property to control whether a custom build lazy-loading should occure. - JHE
		/// This is used in conjunction with lazy loading to prevent regular loading and lazy loading from
		/// loading the data twice for members. - JHE
		/// </summary>
		public bool IsLazyLoadingEnabled { get; set; }

		/// <summary>
		/// This is the 'DataAdapter' used to persist the data to a data store of some Kind. (DataBase, Web Service, ect..)
		/// This property is static to optimize memory usage. Every Entity Type will only have one of these Repositories 
		/// instantiated per app pool. - JHE
		/// </summary>
		public static TRepository Repository
		{
			get
			{
				try
				{
					return Create.New<TRepository>();
				}
				catch (ContainerException cex)
				{
					throw new NetStepsBusinessException(string.Format("Repository not set on Entity: {0}", typeof(T)), cex);
				}
			}
		}

		/// <summary>
		/// This is the Logic used for this Entity Type - JHE
		/// </summary>
		internal static TBusinessLogic BusinessLogic
		{
			get
			{
				try
				{
                  
                    return Create.New<TBusinessLogic>();
				}
				catch (ContainerException cex)
				{
					throw new NetStepsBusinessException(string.Format("BusinessLogic not set on Entity: {0}", typeof(T)), cex);
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public EntityBusinessBase()
			: this(default(TRepository), default(TBusinessLogic))
		{
		}

		/// <summary>
		/// Constructs a new instance which will use the repository given.
		/// </summary>
		/// <param name="repository">a repository</param>
		public EntityBusinessBase(TRepository repository)
			: this(repository, default(TBusinessLogic))
		{
		}

		/// <summary>
		/// Constructs a new instance which will use the repository and business logic given.
		/// </summary>
		/// <param name="repository">the repository</param>
		/// <param name="businessLogic">the business logic</param>
		public EntityBusinessBase(TRepository repository, TBusinessLogic businessLogic)
		{
			_repository = EqualityComparer<TRepository>.Default.Equals(default(TRepository), repository)
					? Repository
					: repository;
			_businessLogic = EqualityComparer<TBusinessLogic>.Default.Equals(default(TBusinessLogic), businessLogic)
					? BusinessLogic
					: businessLogic;
			DefaultValues();
			// TODO: Consider putting a call to StartEntityTracking(); here so that newly constructed entities are tracked (but make sure it doesn't mess up the repo load methods) - JHE
		}
		#endregion

		#region Methods
		/// <summary>
		/// Establishes the instance's default values..
		/// </summary>
		protected void DefaultValues()
		{
			try
			{
				_businessLogic.DefaultValues(_repository, (T)this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public TRepository GetRepository()
		{
			return _repository;
		}

		protected static void InitializeEntity()
		{ // Nothing to do here.            
		}

		/// <summary>
		/// Method to set all child object to start tracking changes to the Entities. - JHE
		/// </summary>
		public void StartEntityTracking()
		{
			try
			{
				_businessLogic.StartEntityTracking(this as T);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Method to set all child object to start tracking changes to the Entities and enable lazy loading. - DES
		/// </summary>
		public void StartEntityTrackingAndEnableLazyLoading()
		{
			try
			{
				_businessLogic.StartEntityTrackingAndEnableLazyLoading(this as T);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void StopEntityTracking()
		{
			try
			{
				_businessLogic.StopEntityTracking(this as T);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// This method is called after saving an Entity to mark the object as unchanged in the Self Tracking Entity. - JHE
		/// </summary>
		public void AcceptEntityChanges(List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			try
			{
				_businessLogic.AcceptChanges(this as T, allTrackerItems);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		#region Basic Crud Methods

		/// <summary>
		/// This will 'Shallow' load an entity (only the entity; not child collections or child entities). - JHE
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <returns></returns>
		public static T Load(TKeyType primaryKey)
		{
			return Load(primaryKey, Repository);
		}
		/// <summary>
		/// Override to Load from a different repository. - JHE
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		public static T Load(TKeyType primaryKey, TRepository repository)
		{
			try
			{
				return BusinessLogic.Load(repository, primaryKey);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// This will load an entire entity (the entity, child collections and child entities). - JHE
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <returns></returns>
		public static T LoadFull(TKeyType primaryKey)
		{
			return LoadFull(primaryKey, Repository);
		}
		/// <summary>
		/// Override to LoadFull from a different repository. - JHE
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		public static T LoadFull(TKeyType primaryKey, TRepository repository)
		{
			try
			{
				return BusinessLogic.LoadFull(repository, primaryKey);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Batch Loads multiple Entities from a list of Primary Keys. - JHE
		/// </summary>
		/// <param name="htmlContentIds"></param>
		/// <returns></returns>
		public static List<T> LoadBatch(List<TKeyType> primaryKeys)
		{
			return LoadBatch(primaryKeys, Repository);
		}
		public static List<T> LoadBatch(List<TKeyType> primaryKeys, TRepository repository)
		{
			try
			{
				var list = repository.LoadBatch(primaryKeys);
				if (list != null)
				{
					foreach (var item in list)
					{
						item.StartTracking();
						item.IsLazyLoadingEnabled = true;
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<T> LoadBatchFull(List<TKeyType> primaryKeys)
		{
			return LoadBatchFull(primaryKeys, Repository);
		}
		public static List<T> LoadBatchFull(List<TKeyType> primaryKeys, TRepository repository)
		{
			try
			{
				var list = repository.LoadBatchFull(primaryKeys);
				foreach (var item in list)
				{
					item.StartEntityTracking();
					item.EnableLazyLoadingRecursive();
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static SqlUpdatableList<T> LoadBatchWithSqlDependency(List<TKeyType> primaryKeys)
		{
			return LoadBatchWithSqlDependency(primaryKeys, Repository);
		}
		public static SqlUpdatableList<T> LoadBatchWithSqlDependency(List<TKeyType> primaryKeys, TRepository repository)
		{
			try
			{
				var list = repository.LoadBatchWithSqlDependency(primaryKeys);
				if (list != null)
				{
					foreach (var item in list)
					{
						item.StartTracking();
						item.IsLazyLoadingEnabled = true;
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public string GetValidationErrorMessage()
		{
			return string.Format("Error Saving {0}: {1}{2}",
											this.GetType().Name,
											Environment.NewLine,
											this.BrokenRulesList.ToString(brokenRule => string.Format("{0}{1}", brokenRule.Description.ToCleanString().Replace("UTC' ", "' "), Environment.NewLine)));
		}

		/// <summary>
		/// Save Entity to the DataBase. - JHE
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public virtual void Save()
		{
			try
			{
				var thisObj = (T)this;
				var allTrackerItems = new List<IObjectWithChangeTracker>();
				thisObj.GetAllChangeTrackerItems(allTrackerItems, true, true);
				thisObj.UpdateAuditFieldsRecursive(allTrackerItems);

				(this as IObjectWithChangeTracker).CleanDataBeforeSaveRecursive();
				var result = ValidateRecursive();
				if (result.IsValid)
					_businessLogic.Save(_repository, thisObj);
				else
				{
					Type type = typeof(T);
					string errorMessage = string.Format("Invalid Entity: {0}. {1}{2}",
							type,
							Environment.NewLine,
							result.BrokenRulesList.ToString(brokenRule => string.Format("{0}.{1} - {2}{3}", brokenRule.EntityName, brokenRule.Property, brokenRule.Description, Environment.NewLine)));
					string publicMessage = string.Format("Error Saving {0}: {1}{2}",
							type.Name,
							Environment.NewLine,
							result.BrokenRulesList.ToString(brokenRule => string.Format("{0}{1}", brokenRule.Description.ToCleanString().Replace("UTC' ", "' "), Environment.NewLine)));

					var netStepsBusinessException = new NetStepsBusinessException(errorMessage);
					netStepsBusinessException.PublicMessage = publicMessage;
					throw netStepsBusinessException;
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Delete Entity from the DataBase. - JHE
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Delete()
		{
			try
			{
				_businessLogic.Delete(_repository, (T)this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Delete Entity from the DataBase by Primary Key. - JHE
		/// </summary>
		/// <param name="obj"></param>
		public static void Delete(TKeyType primaryKey)
		{
			Delete(primaryKey, Repository);
		}
		/// <summary>
		/// Override to Delete from a different repository. - JHE
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <param name="repository"></param>
		public static void Delete(TKeyType primaryKey, TRepository repository)
		{
			try
			{
				BusinessLogic.Delete(repository, primaryKey);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool Exists(TKeyType primaryKey)
		{
			return Repository.Exists(primaryKey);
		}
		#endregion

		#region Events
		/// <summary>
		/// Suppresses Entity Events from Firing, 
		/// useful when loading the entities from the database.
		/// </summary>
		private bool _suppressEntityEvents = false;

		/// <summary>
		/// Determines whether this entity is to suppress events while set to true.
		/// </summary>
		[Bindable(false)]
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool SuppressEntityEvents
		{
			get
			{
				return _suppressEntityEvents;
			}
			set
			{
				_suppressEntityEvents = value;
			}
		}

		protected virtual void RegisterEventHandlers()
		{

		}
		#endregion

		#region Validation
		[NonSerialized]
		private ValidationRules _validationRules;

		/// <summary>
		/// Returns the list of <see cref="Validation.ValidationRules"/> associated with this object.
		/// </summary>
		public ValidationRules ValidationRules
		{
			get
			{
				if (_validationRules == null)
				{
					_validationRules = new ValidationRules(this);

					// Lazy init the rules as well.
					AddValidationRules();
				}

				return _validationRules;
			}
		}

		/// <summary>
		/// Assigns validation rules to this object.
		/// </summary>
		/// <remarks>
		/// This method can be overridden in a derived class to add custom validation rules. 
		///</remarks>
		protected virtual void AddValidationRules()
		{
			try
			{
				if (this is IDatabaseValidationRules)
					(this as IDatabaseValidationRules).AddDatabaseValidationRules();

				_businessLogic.AddValidationRules((T)this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Returns <see langword="true" /> if the object is valid, 
		/// <see langword="false" /> if the object validation rules that have indicated failure. 
		/// </summary>
		public virtual bool IsValid
		{
			get
			{
				return ValidationRules.IsValid;
			}
		}

		/// <summary>
		/// Returns a list of all the validation rules that failed.
		/// </summary>
		/// <returns><see cref="Validation.BrokenRulesList" /></returns>
		public virtual BrokenRulesList BrokenRulesList
		{
			get
			{
				return ValidationRules.GetBrokenRules();
			}
		}

		/// <summary>
		/// Adds a rule to the list of validated rules.
		/// </summary>
		/// <param name="handler">The method that implements the rule.</param>
		/// <param name="propertyName">
		/// The name of the property on the target object where the rule implementation can retrieve
		/// the value to be validated.
		/// </param>
		public void AddValidationRuleHandler(ValidationRuleHandler handler, String propertyName)
		{
			ValidationRules.AddRule(handler, propertyName);
		}

		/// <summary>
		/// Adds a rule to the list of validated rules.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="handler">The method that implements the rule.</param>
		/// <param name="args">
		/// A <see cref="Validation.ValidationRuleArgs"/> object specifying the property name and other arguments
		/// passed to the rule method
		/// </param>
		public void AddValidationRuleHandler(ValidationRuleHandler handler, ValidationRuleArgs args)
		{
			ValidationRules.AddRule(handler, args);
		}

		/// <summary>
		/// Force this object to validate itself using the assigned business rules.
		/// </summary>
		/// <remarks>Validates all properties.</remarks>
		public virtual void Validate()
		{
			ValidationRules.ValidateRules();
		}

		/// <summary>
		/// Force the object to validate itself using the assigned business rules.
		/// </summary>
		/// <param name="propertyName">Name of the property to validate.</param>
		public void Validate(string propertyName)
		{
			ValidationRules.ValidateRules(propertyName);
		}

		/// <summary>
		/// Force the object to validate itself using the assigned business rules.
		/// </summary>
		/// <param name="column">Column enumeration representing the column to validate.</param>
		public void Validate(System.Enum column)
		{
			Validate(column.ToString());
		}

		/// <summary>
		/// This is a collection of property names that can be ignored on child objects when validating a parent due to the fact that
		/// Entity Framework will set their values on save. - JHE
		/// Example for Saving and Order: OrderID (gets set on child Entities)
		/// </summary>
		/// <returns></returns>
		public List<string> ValidatedChildPropertiesSetByParent()
		{
			return _businessLogic.ValidatedChildPropertiesSetByParent(_repository);
		}

		/// <summary>
		/// Method to recursively check that an Entity is valid according to Business rules of the Entity
		/// and each child entity. - JHE
		/// TODO: Test this new recursive Validation method - JHE
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual ValidationResult ValidateRecursive()
		{
			return (this as IObjectWithChangeTracker).IsValidRecursive();
		}
		#endregion

		public virtual void CleanDataBeforeSave()
		{
			_businessLogic.CleanDataBeforeSave(_repository, (T)this);
		}

		#region IDataErrorInfo Members

		/// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
		/// <value></value>
		/// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>      
		public string Error
		{
			get
			{
				string errorDescription = string.Empty;
				if (!IsValid)
				{
					errorDescription = ValidationRules.GetBrokenRules().ToString();
				}
				return errorDescription;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:String"/> with the specified column name.
		/// </summary>
		/// <value></value>
		public string this[string columnName]
		{
			get
			{
				string errorDescription = string.Empty;
				if (!IsValid)
				{
					errorDescription = ValidationRules.GetBrokenRules().GetPropertyErrorDescriptions(columnName);
				}
				return errorDescription;
			}
		}

		#endregion

		#region Serialization

		[OnDeserialized]
		internal void OnDeserializedBaseMethod(StreamingContext context)
		{
			// Re-establish fields that get severed during serialization.
			_repository = EntityBusinessBase<T, TKeyType, TRepository, TBusinessLogic>.Repository;
			_businessLogic = EntityBusinessBase<T, TKeyType, TRepository, TBusinessLogic>.BusinessLogic;

			if (!_suppressEntityEvents)
			{
				try
				{
					//Removing this since there are many circular de-serialiation paths when pulling items from session...
					//And this will happen when the Validation property is accessed & initialized
					//ValidationRules.Target = this;s
					//AddValidationRules(); 
					RegisterEventHandlers();
					Deserialized();
				}
				catch (Exception ex)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				}
			}
		}

		public virtual void Deserialized()
		{
			// To allow inheriting classes to register things - JHE
		}
		#endregion

		#region IKeyName<TKeyType> Members

		TKeyType IKeyName<T, TKeyType>.ID
		{
			get
			{
				if (_businessLogic.GetIdColumnFunc != null)
					return _businessLogic.GetIdColumnFunc((T)this);
				else
					return default(TKeyType);
			}
			set
			{
				if (_businessLogic.SetIdColumnFunc != null)
					_businessLogic.SetIdColumnFunc((T)this, value);
			}
		}

		string IKeyName<T, TKeyType>.Title
		{
			get
			{
				if (_businessLogic.GetTitleColumnFunc != null)
					return _businessLogic.GetTitleColumnFunc((T)this);
				else
					return string.Empty;
			}
			set
			{
				if (_businessLogic.SetTitleColumnFunc != null)
					_businessLogic.SetTitleColumnFunc((T)this, value);
			}
		}

		Func<T, TKeyType> IKeyName<T, TKeyType>.GetIdColumnFunc
		{
			get
			{
				return _businessLogic.GetIdColumnFunc;
			}
		}
		Action<T, TKeyType> IKeyName<T, TKeyType>.SetIdColumnFunc
		{
			get
			{
				return _businessLogic.SetIdColumnFunc;
			}
		}

		Func<T, string> IKeyName<T, TKeyType>.GetTitleColumnFunc
		{
			get
			{
				return _businessLogic.GetTitleColumnFunc;
			}
		}
		Action<T, string> IKeyName<T, TKeyType>.SetTitleColumnFunc
		{
			get
			{
				return _businessLogic.SetTitleColumnFunc;
			}
		}

		#endregion

		#region IListValue Members

		int IListValue.ID
		{
			get
			{
				return Convert.ToInt32((this as IKeyName<T, TKeyType>).ID);
			}
			set
			{
				(this as IKeyName<T, TKeyType>).ID = (TKeyType)Convert.ChangeType(value, typeof(TKeyType));
			}
		}

		string IListValue.Title
		{
			get
			{
				return (this as IKeyName<T, TKeyType>).Title;
			}
			set
			{
				(this as IKeyName<T, TKeyType>).Title = value;
			}
		}

		#endregion
	}
}