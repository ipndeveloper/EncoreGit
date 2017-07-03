using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICampaignActionQueueItemRepository
    {
        PaginatedList<CampaignActionQueueItem> QueueCampaignActionItems(int numberToFetch);
    }
}
