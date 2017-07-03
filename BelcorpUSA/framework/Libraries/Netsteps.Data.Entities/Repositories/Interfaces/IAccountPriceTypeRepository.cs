using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAccountPriceTypeRepository
	{
		List<AccountPriceType> LoadAllByStoreFront(int storeFrontID);
	}
}
