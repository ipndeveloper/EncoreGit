using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.DomainEventTaskHandlers
{
    class BreakingNewsDomainEventTaskHandler : DomainEventTaskHandlerBase
    {
        protected override bool Run(DomainEventQueueItem domainEventQueueItem)
        {
            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.NewsID.HasValue)
            {
                Logger.Info("About to process domainEventQueueItem {0}", domainEventQueueItem.DomainEventQueueItemID);
                var maxNumberToProcess = ConfigurationManager.GetAppSetting("MobileNewsNotification_MaxNumberToPoll", 10000);
                Logger.Info("Max number to process: {0}", maxNumberToProcess);
                var newsUrl = ConfigurationManager.GetAppSetting("MobileNewsNotificationUrl", "{0}");
                Logger.Info("News URL: ", newsUrl);
                var devices = AccountDevice.GetDevicesForNewsNotifications(domainEventQueueItem.EventContext.NewsID.Value, maxNumberToProcess);
                Logger.Info("Number of devices: {0}", devices.Count);
                var deviceNotifications = (from device in devices
                                           select DeviceNotification.GetDeviceNotificationForDomainEvent(device, domainEventQueueItem)).ToList();

                DeviceNotification.Repository.SaveBatch(deviceNotifications);

                // If there all devices have been notified of this breaking news, this item is complete
                if (devices.Count == 0)
                {
                    return true;
                }
            }
            else
            {
                Logger.Info("DomainEventQueueProcessor.ProcessQueueItem NewsID is null for DomainEventQueueItemID {0}.", domainEventQueueItem.DomainEventQueueItemID);
            }

            return false;
        }
    }
}
