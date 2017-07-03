using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class AccountListValue : IListValue, ITermName
	{
		#region Properties
		#endregion

		#region Constructors
		public AccountListValue(int accountID, short listValueTypeID, string value)
		{
			if (!this.ChangeTracker.ChangeTrackingEnabled)
				this.StartEntityTracking();
			DefaultValues();

			this.AccountID = accountID;
			this.ListValueTypeID = listValueTypeID;
			this.Value = value.ToCleanString();
			this.Editable = true;
			this.IsCorporate = false;
			this.Active = true;
		}
		#endregion

		#region Methods
		public static List<AccountListValue> LoadAllCorporateListValues()
		{
			try
			{
				return BusinessLogic.LoadAllCorporateListValues(Repository);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<AccountListValue> LoadCorporateListValuesByType(int listValueTypeID)
		{
			try
			{
				return BusinessLogic.LoadCorporateListValuesByType(Repository, listValueTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static SqlUpdatableList<AccountListValue> LoadAllCorporateListValuesWithSqlDependency()
		{
			try
			{
				return BusinessLogic.LoadAllCorporateListValuesWithSqlDependency(Repository);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<AccountListValue> LoadListValuesByTypeAndAccountID(int accountID, int listValueTypeID)
		{
			try
			{
				return BusinessLogic.LoadListValuesByTypeAndAccountID(Repository, accountID, listValueTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static List<AccountListValue> CloneListForAccountOverrides(int accountID, short listValueTypeID)
		{
			try
			{
				List<AccountListValue> list = new List<AccountListValue>();
				foreach (var item in SmallCollectionCache.Instance.CorporateAccountListValues.Where(v => v.ListValueTypeID == listValueTypeID))
				{
					AccountListValue accountListValue = new AccountListValue();
					accountListValue.StartTracking();
					accountListValue.AccountID = accountID;
					accountListValue.IsCorporate = false;
					accountListValue.Editable = true;
					accountListValue.Active = true;
					accountListValue.ListValueTypeID = listValueTypeID;
					accountListValue.Value = item.Value.ToCleanString();

					list.Add(accountListValue);
				}

				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void SaveBatch(IEnumerable<AccountListValue> items)
		{
			try
			{
				BusinessLogic.SaveBatch(Repository, items);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		#region IListValue Members

		int IListValue.ID
		{
			get
			{
				return this.AccountListValueID;
			}
			set
			{
				this.AccountListValueID = value;
			}
		}

		string IListValue.Title
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = value;
			}
		}

		void IListValue.Save()
		{
			base.Save();
		}

		#endregion

		#region ITermName Members

		string ITermName.Name
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = value;
			}
		}

		string ITermName.TermName
		{
			get
			{
				return this.TermName;
			}
			set
			{
				this.TermName = value;
			}
		}

		#endregion
	}
}

