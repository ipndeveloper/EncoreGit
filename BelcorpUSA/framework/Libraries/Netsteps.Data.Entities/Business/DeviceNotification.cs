using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities
{
    public partial class DeviceNotification
    {
        public static DeviceNotification GetDeviceNotificationForDomainEvent(AccountDevice device, DomainEventQueueItem domainEvent)
        {
            return BusinessLogic.GetDeviceNotificationForDomainEvent(device, domainEvent);
        }

        public static PaginatedList<DeviceNotification> QueueDeviceNotifications(int maxNumberToPoll)
        {
            try
            {
                var list = Repository.QueueDeviceNotifications(maxNumberToPoll);
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

        public static IDeviceNotificationSender GetDeviceNotificationSender()
        {
            return BusinessLogic.GetDeviceNotificationSender();
        }
    }
}
