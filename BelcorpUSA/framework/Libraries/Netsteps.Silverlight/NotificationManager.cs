using System;

namespace NetSteps.Silverlight
{
    public class NotificationManager
    {
        public SendNotificationMessage SendNotificationMethod;

        public void SendNotification(EventNotificationMessage message)
        {
            if (SendNotificationMethod != null)
                SendNotificationMethod(message);
        }

        public void SendNotification(string message)
        {
            SendNotification(new EventNotificationMessage(message));
        }

        public void SendNotification(string message, Exception ex)
        {
            SendNotification(new EventNotificationMessage(message, ex));
        }

        public void SendNotification(string message, Exception ex, string extraInfo)
        {
            SendNotification(new EventNotificationMessage(message, ex, extraInfo));
        }
    }

    public delegate void SendNotificationMessage(EventNotificationMessage message);
}
