using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class CampaignSearchParameters : FilterDateRangePaginatedListParameters<Campaign>
    {
        public bool? Active { get; set; }

        public int? MarketID { get; set; }


    }
}
