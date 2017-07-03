using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AccountListValueBusinessLogic
	{
		public override void DefaultValues(IAccountListValueRepository repository, AccountListValue entity)
		{
			base.DefaultValues(repository, entity);

			entity.IsCorporate = false;
			entity.Editable = true;
			entity.Active = true;
		}

		public virtual List<AccountListValue> LoadAllCorporateListValues(IAccountListValueRepository repository)
		{
			try
			{
				var list = repository.LoadAllCorporateListValues();
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<AccountListValue> LoadCorporateListValuesByType(IAccountListValueRepository repository, int listValueTypeID)
		{
			try
			{
				var list = repository.LoadCorporateListValuesByType(listValueTypeID);
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual SqlUpdatableList<AccountListValue> LoadAllCorporateListValuesWithSqlDependency(IAccountListValueRepository repository)
		{
			try
			{
				var list = repository.LoadAllCorporateListValuesWithSqlDependency();
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<AccountListValue> LoadListValuesByTypeAndAccountID(IAccountListValueRepository repository, int accountID, int listValueTypeID)
		{
			try
			{
				var list = repository.LoadListValuesByTypeAndAccountID(accountID, listValueTypeID);
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}

				// Use Corporate values if Account doesn't have any values yet. - JHE
				if (list.Count == 0)
					list = SmallCollectionCache.Instance.CorporateAccountListValues.Where(alv => alv.ListValueTypeID == listValueTypeID).ToList();

				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void SaveBatch(IAccountListValueRepository repository, IEnumerable<AccountListValue> items)
		{
			try
			{
				repository.SaveBatch(items);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
