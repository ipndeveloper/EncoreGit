using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class CatalogTypeBusinessLogic
	{
		public virtual IEnumerable<int> GetShoppableCatalogTypeIds()
		{
            var catalogTypesToSearch = new[] { (int)Constants.CatalogType.Normal, (int)Constants.CatalogType.FeaturedItems };
		    return catalogTypesToSearch;
		}
	}
}
