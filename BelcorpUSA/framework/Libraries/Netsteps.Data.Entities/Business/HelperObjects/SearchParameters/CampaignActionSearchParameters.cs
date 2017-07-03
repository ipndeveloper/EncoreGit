using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class CampaignActionSearchParameters : FilterDateRangePaginatedListParameters<CampaignAction>
    {
        public bool? Active { get; set; }

        public int? CampaignID { get; set; }

        public Constants.CampaignActionType CampaignActionType { get; set; }
    }
}
