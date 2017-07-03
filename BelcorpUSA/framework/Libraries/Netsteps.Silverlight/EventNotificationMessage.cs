using System;
using System.Windows.Media;
using NetSteps.Silverlight.Exceptions;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight
{
    public class EventNotificationMessage
    {
        public string Message { get; set; }
        public ImageSource IconUrl { get; set; }
        public Exception Exception { get; set; }

        public EventNotificationMessage(string message)
        {
            Message = message;
        }

        public EventNotificationMessage(string message, ImageSource iconUrl)
        {
            Message = message;
            IconUrl = iconUrl;
        }

        public EventNotificationMessage(string message, Exception exception)
        {
            Message = message;

            if (!(exception is GeneralException))
                Exception = new GeneralException(exception);
            else
                Exception = exception;
        }

        public EventNotificationMessage(string message, Exception exception, string queryString)
        {
            Message = message;

            if (!(exception is GeneralException))
                Exception = new GeneralException(exception, queryString);
            else
                Exception = exception;
        }

        private static ImageSource acceptImage = null;
        private static ImageSource alertImage = null;

        public EventNotificationMessage(string message, NotificationMessageDefaultIcons defaultIcon)
        {
            SendEventNotificationMessage(message, defaultIcon, null);
        }
        public EventNotificationMessage(Exception ex)
        {
            SendEventNotificationMessage(ex.Message, NotificationMessageDefaultIcons.Alert, ex);
        }
        public EventNotificationMessage(string message, NotificationMessageDefaultIcons defaultIcon, Exception ex)
        {
            SendEventNotificationMessage(message, defaultIcon, ex);
        }

        public void SendEventNotificationMessage(string message, NotificationMessageDefaultIcons defaultIcon, Exception ex)
        {
            Message = message;
            try
            {
                switch (defaultIcon)
                {
                    case NotificationMessageDefaultIcons.Accept:
                        if (acceptImage == null)
                            //acceptImage = "/Images/acrobatICO.png".ToImageSource(); //debugging
                            acceptImage = "/Images/accept.png".ToImageSource();
                        IconUrl = acceptImage;
                        break;
                    case NotificationMessageDefaultIcons.Alert:
                        if (alertImage == null)
                            //alertImage = "/Images/ActivityLogIco.png".ToImageSource(); //debugging
                            alertImage = "/Images/AlertIcon.png".ToImageSource();
                        IconUrl = alertImage;
                        break;
                    default:
                        break;
                }
            }
            catch (UnauthorizedAccessException) { }
        }
    }

    public enum NotificationMessageDefaultIcons
    {
        Accept,
        Alert
    }
}
