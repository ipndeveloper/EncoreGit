using NetSteps.Common.Base;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{

	public partial interface IProductPropertyRepository
	{
		IEnumerable<ProductProperty> LoadByProductID(int productId);
	}
}
