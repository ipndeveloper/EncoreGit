using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
	public partial class ProductProperty
	{
		public static IEnumerable<ProductProperty> LoadByProductID(int productID)
		{
			return Repository.LoadByProductID(productID);
		}
	}
}
