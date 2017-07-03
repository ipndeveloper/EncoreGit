using System.Collections.Generic;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface IDeviceNotificationSender
    {
        bool SendDeviceNotifications(List<DeviceNotification> notifications);
    }
}
