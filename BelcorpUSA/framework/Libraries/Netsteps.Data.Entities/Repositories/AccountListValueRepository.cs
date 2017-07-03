using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountListValueRepository : BaseRepository<AccountListValue, int, NetStepsEntities>, IDefaultImplementation
	{
		#region Members
		#endregion

		#region Methods
		// TODO: Change this to have an SqlDependency and add them to SmallCollectionCache - JHE
		public List<AccountListValue> LoadAllCorporateListValues()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var accountListValues = from a in context.AccountListValues
											where a.IsCorporate == true
											select a;

					return accountListValues.ToList();
				}
			});
		}

		// TODO: Change this to have an SqlDependency and add them to SmallCollectionCache - JHE
		public List<AccountListValue> LoadCorporateListValuesByType(int listValueTypeID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var accountListValues = from a in context.AccountListValues
											where a.ListValueTypeID == listValueTypeID && a.IsCorporate == true
											select a;

					return accountListValues.ToList();
				}
			});
		}

		public SqlUpdatableList<AccountListValue> LoadAllCorporateListValuesWithSqlDependency()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var accountListValues = from a in context.AccountListValues
											where a.IsCorporate == true
											select a;

					var entityQuery = (ObjectQuery<AccountListValue>)accountListValues;
					//var entityQuery = GetLoadALLQuery(context);
					return LoadListWithSqlDependency(entityQuery);
				}
			});
		}

		public List<AccountListValue> LoadListValuesByTypeAndAccountID(int accountID, int listValueTypeID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var overriddenListValues = context.AccountListValues.Where(alv => alv.AccountID == accountID && alv.ListValueTypeID == listValueTypeID);
					if(overriddenListValues.Any())
					{
						return overriddenListValues.ToList();
					}

					return context.AccountListValues.Where(alv => alv.ListValueTypeID == listValueTypeID && alv.IsCorporate).ToList();
				}
			});
		}

		#endregion
	}
}