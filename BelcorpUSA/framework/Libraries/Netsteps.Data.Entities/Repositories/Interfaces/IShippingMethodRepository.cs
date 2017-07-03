using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IShippingMethodRepository
	{
		List<int> LoadAllTranslationIds();
		ShippingMethod LoadByName(string name);
	}
}
