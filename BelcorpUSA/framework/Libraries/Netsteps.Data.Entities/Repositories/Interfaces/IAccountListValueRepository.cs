using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAccountListValueRepository
	{
		List<AccountListValue> LoadAllCorporateListValues();
		List<AccountListValue> LoadCorporateListValuesByType(int listValueTypeID);
		SqlUpdatableList<AccountListValue> LoadAllCorporateListValuesWithSqlDependency();
		List<AccountListValue> LoadListValuesByTypeAndAccountID(int accountID, int listValueTypeID);
	}
}
