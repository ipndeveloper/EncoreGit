using System;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class DeviceNotificationRepository
    {
        public PaginatedList<DeviceNotification> QueueDeviceNotifications(int maxNumberToPoll)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<DeviceNotification>();

                    int attemptCountLimit = 5;
                    short queuedStatusID = Constants.QueueItemStatus.Queued.ToShort();
                    short runningStatusID = Constants.QueueItemStatus.Running.ToShort();
                    short failedStatusID = Constants.QueueItemStatus.Failed.ToShort();
                    TimeSpan retryInterval = TimeSpan.FromMinutes(10);
                    DateTime cutoffTime = DateTime.UtcNow.Subtract(retryInterval);


                    IQueryable<DeviceNotification> notifications = from n in context.DeviceNotifications
                                                                   where n.QueueItemStatusID == queuedStatusID ||
                                                                   (n.QueueItemStatusID == failedStatusID && n.AttemptCount < attemptCountLimit && n.LastRunDateUTC <= cutoffTime) ||
                                                                   (n.QueueItemStatusID == runningStatusID && n.LastRunDateUTC <= cutoffTime)
                                                                   select n;

                    notifications = notifications.Take(maxNumberToPoll);

                    // Update DB to mark the polled items as running - JHE
                    var items = notifications.ToList();
                    if (items != null && items.Count > 0)
                    {
                        foreach (var notification in items)
                        {
                            // Update the items to failed status after 5 unsuccessful attempts - JHE
                            if (notification.AttemptCount >= attemptCountLimit)
                                notification.QueueItemStatusID = Constants.QueueItemStatus.Failed.ToShort();
                            else
                            {
                                notification.AttemptCount++;
                                notification.QueueItemStatusID = Constants.QueueItemStatus.Running.ToShort();
                                notification.LastRunDate = DateTime.Now;
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
