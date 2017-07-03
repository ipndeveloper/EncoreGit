using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class ArchiveSearchParameters : FilterDateRangePaginatedListParameters<Archive>
	{
		public int SiteID { get; set; }
		public int? CategoryID { get; set; }
		public bool? Active { get; set; }
		public bool? IsDownloadable { get; set; }
		new public int? LanguageID { get; set; }
		public string Query { get; set; }

		public List<Constants.FileType> FileTypes { get; set; }
	}
}
