using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class NewsSearchParameters : FilterDateRangePaginatedListParameters<News>
    {
        public int? SiteID { get; set; }
        public int? MarketID { get; set; }
        public int? NewsTypeID { get; set; }
        public bool? Active { get; set; }
        public string Title { get; set; }
    }
}
