using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class ArchiveCategorySearchData
	{
		public int CategoryID { get; set; }

		public int? ParentCategoryID { get; set; }

		public string Name { get; set; }

		public int ArchiveCount { get; set; }

		public int SortIndex { get; set; }
	}
}
