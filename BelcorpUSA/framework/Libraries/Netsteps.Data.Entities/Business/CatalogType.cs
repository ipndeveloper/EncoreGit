using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
	public partial class CatalogType
	{
		public static IEnumerable<int> GetShoppableCatalogTypeIds()
		{
		    return BusinessLogic.GetShoppableCatalogTypeIds();
		}		
	}
}

