using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
	public class CategorySearchData
	{
		public int TreeID { get; set; }

		public string TreeName { get; set; }

		public IEnumerable<UsedBySearchData> UsedBy { get; set; }

		public int ProductCount { get; set; }

		public class UsedBySearchData
		{
			public int CatalogID { get; set; }

			public string CatalogName { get; set; }

			public string StoreFronts { get; set; }
		}
	}
}
