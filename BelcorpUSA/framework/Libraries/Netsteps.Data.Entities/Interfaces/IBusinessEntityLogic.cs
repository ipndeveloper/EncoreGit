using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Interfaces
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface to access common Business Logic for Entities.
	/// Created: 03-25-2010
	/// </summary>
	[ContractClass(typeof(Contracts.BusinessEntityLogicContract<,,>))]
	public interface IBusinessEntityLogic<T, TKeyType, TRepository>
	{
		Func<T, TKeyType> GetIdColumnFunc { get; }
		Action<T, TKeyType> SetIdColumnFunc { get; }
		Func<T, string> GetTitleColumnFunc { get; }
		Action<T, string> SetTitleColumnFunc { get; }

		void DefaultValues(TRepository repository, T entity);

		T Load(TRepository repository, TKeyType primaryKey);
		T LoadFull(TRepository repository, TKeyType primaryKey);
		List<T> LoadAll(TRepository repository);
		List<T> LoadAllFull(TRepository repository);

		void Save(TRepository repository, T entity);
		void Delete(TRepository repository, T entity);
		void Delete(TRepository repository, TKeyType primaryKey);

		PaginatedList<AuditLogRow> GetAuditLog(TRepository repository, int primaryKey, AuditLogSearchParameters param);

		void StartEntityTracking(T entity);
		void StartEntityTrackingAndEnableLazyLoading(T entity);
		void StopEntityTracking(T entity);
		void AcceptChanges(T entity, List<IObjectWithChangeTracker> allTrackerItems = null);

		void Validate(T entity);
		bool IsValid(T entity);
		void AddValidationRules(T entity);
		List<string> ValidatedChildPropertiesSetByParent(TRepository repository);

		void CleanDataBeforeSave(TRepository repository, T entity);
		void UpdateCreatedFields(TRepository repository, T entity);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IBusinessEntityLogic<,,>))]
		internal abstract class BusinessEntityLogicContract<T, TKeyType, TRepository> : IBusinessEntityLogic<T, TKeyType, TRepository>
		{

			Func<T, TKeyType> IBusinessEntityLogic<T, TKeyType, TRepository>.GetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<T, TKeyType> IBusinessEntityLogic<T, TKeyType, TRepository>.SetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Func<T, string> IBusinessEntityLogic<T, TKeyType, TRepository>.GetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<T, string> IBusinessEntityLogic<T, TKeyType, TRepository>.SetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.DefaultValues(TRepository repository, T entity)
			{
				Contract.Requires(repository != null);
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			T IBusinessEntityLogic<T, TKeyType, TRepository>.Load(TRepository repository, TKeyType primaryKey)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			T IBusinessEntityLogic<T, TKeyType, TRepository>.LoadFull(TRepository repository, TKeyType primaryKey)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			List<T> IBusinessEntityLogic<T, TKeyType, TRepository>.LoadAll(TRepository repository)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			List<T> IBusinessEntityLogic<T, TKeyType, TRepository>.LoadAllFull(TRepository repository)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.Save(TRepository repository, T entity)
			{
				Contract.Requires(repository != null);
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.Delete(TRepository repository, T entity)
			{
				Contract.Requires(repository != null);
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.Delete(TRepository repository, TKeyType primaryKey)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> IBusinessEntityLogic<T, TKeyType, TRepository>.GetAuditLog(TRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.StartEntityTracking(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.StartEntityTrackingAndEnableLazyLoading(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.StopEntityTracking(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.AcceptChanges(T entity, List<IObjectWithChangeTracker> allTrackerItems)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.Validate(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			bool IBusinessEntityLogic<T, TKeyType, TRepository>.IsValid(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.AddValidationRules(T entity)
			{
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			List<string> IBusinessEntityLogic<T, TKeyType, TRepository>.ValidatedChildPropertiesSetByParent(TRepository repository)
			{
				Contract.Requires(repository != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.CleanDataBeforeSave(TRepository repository, T entity)
			{
				Contract.Requires(repository != null);
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<T, TKeyType, TRepository>.UpdateCreatedFields(TRepository repository, T entity)
			{
				Contract.Requires(repository != null);
				Contract.Requires(entity != null);

				throw new NotImplementedException();
			}
		}
	}
}
