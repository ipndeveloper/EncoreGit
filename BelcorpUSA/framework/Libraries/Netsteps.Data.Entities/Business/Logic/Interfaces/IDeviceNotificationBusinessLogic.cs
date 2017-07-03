using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IDeviceNotificationBusinessLogic
    {
        DeviceNotification GetDeviceNotificationForDomainEvent(AccountDevice device, DomainEventQueueItem domainEvent);
        IDeviceNotificationSender GetDeviceNotificationSender();
    }
}
