using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IAccountListValueBusinessLogic
	{
		List<AccountListValue> LoadAllCorporateListValues(IAccountListValueRepository repository);
		List<AccountListValue> LoadCorporateListValuesByType(IAccountListValueRepository repository, int listValueTypeID);
		SqlUpdatableList<AccountListValue> LoadAllCorporateListValuesWithSqlDependency(IAccountListValueRepository repository);
		List<AccountListValue> LoadListValuesByTypeAndAccountID(IAccountListValueRepository repository, int accountID, int listValueTypeID);
		void SaveBatch(IAccountListValueRepository repository, IEnumerable<AccountListValue> items);
	}
}
