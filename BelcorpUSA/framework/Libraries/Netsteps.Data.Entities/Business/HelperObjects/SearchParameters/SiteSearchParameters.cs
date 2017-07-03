using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class SiteSearchParameters : FilterPaginatedListParameters<Site>
	{
		public int? BaseSiteID { get; set; }

		public int? SiteStatusID { get; set; }

		public string SiteName { get; set; }

		public string Url { get; set; }
	}
}
