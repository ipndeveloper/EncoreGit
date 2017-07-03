using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CampaignActionQueueItemRepository
    {
        protected override Func<NetStepsEntities, IQueryable<CampaignActionQueueItem>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<CampaignActionQueueItem>>(
                 (context) => context.CampaignActionQueueItems
                     .Include("EventContext")
                     );
            }
        }

        public PaginatedList<CampaignActionQueueItem> QueueCampaignActionItems(int maxNumberToPoll)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<CampaignActionQueueItem>();

                    int attemptCountLimit = 5;
                    short queuedStatusID = Constants.QueueItemStatus.Queued.ToShort();
                    short runningStatusID = Constants.QueueItemStatus.Running.ToShort();
                    short failedStatusID = Constants.QueueItemStatus.Failed.ToShort();
                    TimeSpan retryInterval = TimeSpan.FromMinutes(10);
                    DateTime cutoffTime = DateTime.UtcNow.Subtract(retryInterval);

                    IQueryable<CampaignActionQueueItem> campaignActionQueueItems = from a in context.CampaignActionQueueItems.Include("EventContext")
                                                                                   where a.QueueItemStatusID == queuedStatusID ||
                                                                                   (a.QueueItemStatusID == failedStatusID && a.AttemptCount < attemptCountLimit && a.LastRunDateUTC <= cutoffTime) ||
                                                                                   (a.QueueItemStatusID == runningStatusID && a.LastRunDateUTC <= cutoffTime)
                                                                                   orderby a.QueueItemPriorityID descending
                                                                                   select a;
                    results.TotalCount = campaignActionQueueItems.Count();

                    campaignActionQueueItems = campaignActionQueueItems.Take(maxNumberToPoll);

                    // Update DB to mark the polled items as running - JHE
                    var items = campaignActionQueueItems.ToList();
                    if (items != null && items.Count > 0)
                    {
                        foreach (var campaignActionQueueItem in items)
                        {
                            // Update the items to failed status after 5 unsuccessful attempts - JHE
                            if (campaignActionQueueItem.AttemptCount >= attemptCountLimit)
                                campaignActionQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Failed.ToShort();
                            else
                            {
                                campaignActionQueueItem.AttemptCount++;
                                campaignActionQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Running.ToShort();
                                campaignActionQueueItem.LastRunDate = DateTime.Now;
                            }
                        }
                        context.SaveChanges();

                        results.AddRange(items);
                    }

                    return results;
                }
            });
        }
    }
}
