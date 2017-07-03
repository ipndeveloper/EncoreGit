using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
	public partial class DescriptionTranslation
	{
		public static IEnumerable<DescriptionTranslation> LoadByProductID(int productID)
		{
			return Repository.LoadByProductID(productID);
		}
	}
}
