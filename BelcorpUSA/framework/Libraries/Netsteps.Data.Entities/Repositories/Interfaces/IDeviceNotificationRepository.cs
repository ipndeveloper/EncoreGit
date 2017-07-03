using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IDeviceNotificationRepository
    {
        PaginatedList<DeviceNotification> QueueDeviceNotifications(int maxNumberToPoll);
    }
}
