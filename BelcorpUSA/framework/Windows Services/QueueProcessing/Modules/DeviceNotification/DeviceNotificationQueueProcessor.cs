using System;
using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DeviceNotification
{
    public class DeviceNotificationQueueProcessor : QueueProcessor<List<Data.Entities.DeviceNotification>>
    {
        public static readonly string CProcessorName = "DeviceNotificationQueueProcessor";

        public DeviceNotificationQueueProcessor()
        {
            Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            try
            {
                Logger.Info("DeviceNotificationQueueProcessor - CreateQueueItems");

                var deviceNotifications = NetSteps.Data.Entities.DeviceNotification.QueueDeviceNotifications(maxNumberToPoll);

                if (deviceNotifications.Count > 0)
                    EnqueueItem(deviceNotifications);

                Logger.Info("DeviceNotificationQueueProcessor - Enqueued {0} Items", deviceNotifications.Count);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public override void ProcessQueueItem(List<Data.Entities.DeviceNotification> notifications)
        {
            Logger.Info(String.Format("About to process {0} Device Queue Items", notifications.Count));
            var notificationSender = Data.Entities.DeviceNotification.GetDeviceNotificationSender();
            bool success = notificationSender.SendDeviceNotifications(notifications);
            Constants.QueueItemStatus status = success ? Constants.QueueItemStatus.Completed : Constants.QueueItemStatus.Failed;

            // Have to assume success at this point, 3rd party will alert us to failures through another mechanism
            notifications.ForEach(n => n.QueueItemStatusID = (short)status);
            Data.Entities.DeviceNotification.Repository.SaveBatch(notifications);
        }
    }
}
