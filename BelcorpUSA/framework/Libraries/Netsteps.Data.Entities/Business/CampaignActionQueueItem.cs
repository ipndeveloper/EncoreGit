using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class CampaignActionQueueItem
    {
        public static void AddCampaignActionQueueItemToQueue(int campaignActionID, int? orderID, int? accountID, int? partyID)
        {
            try
            {
                CampaignActionQueueItem campaignActionQueueItem = new CampaignActionQueueItem();

                if (orderID.HasValue || accountID.HasValue || partyID.HasValue)
                {
                    if (campaignActionQueueItem.EventContext == null)
                        campaignActionQueueItem.EventContext = new EventContext();

                    campaignActionQueueItem.EventContext.OrderID = orderID;
                    campaignActionQueueItem.EventContext.AccountID = accountID;
                    campaignActionQueueItem.EventContext.PartyID = partyID;
                }

                campaignActionQueueItem.QueueItemPriorityID = Constants.QueueItemPriority.Normal.ToShort();
                campaignActionQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Queued.ToShort();
                campaignActionQueueItem.CampaignActionID = campaignActionID;

                campaignActionQueueItem.Save();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<CampaignActionQueueItem> QueueCampaignActionItems(int maxNumberToPoll)
        {
            try
            {
                var list = Repository.QueueCampaignActionItems(maxNumberToPoll);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        item.StartTracking();
                        item.IsLazyLoadingEnabled = true;
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


    }
}
