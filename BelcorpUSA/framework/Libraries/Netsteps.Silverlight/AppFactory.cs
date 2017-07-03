
using System.Windows.Threading;
namespace NetSteps.Silverlight
{
    public static class AppFactory
    {
        private static Dispatcher _dispatcher = null;
        public static Dispatcher Dispatcher 
        {
            get
            {
                return _dispatcher;
            }
            set
            {
                _dispatcher = value;
            }
        }

        private static ExceptionManager _exceptionManager = null;
        public static ExceptionManager ExceptionManager
        {
            get
            {
                if (_exceptionManager == null)
                {
                    _exceptionManager = new ExceptionManager();
                }
                return _exceptionManager;
            }
            set
            {
                _exceptionManager = value;
            }
        }

        private static PopupManager _popupManager = null;
        public static PopupManager PopupManager
        {
            get
            {
                if (_popupManager == null)
                {
                    _popupManager = new PopupManager();
                }
                return _popupManager;
            }
            set
            {
                _popupManager = value;
            }
        }

        private static NotificationManager _notificationManager = null;
        public static NotificationManager NotificationManger
        {
            get
            {
                if (_notificationManager == null)
                {
                    _notificationManager = new NotificationManager();
                }
                return _notificationManager;
            }
            set
            {
                _notificationManager = value;
            }
        }

        public static string _serverFailureMessage = string.Empty;
        public static string ServerFailureMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_serverFailureMessage))
                {
                    _serverFailureMessage = "Call to the server failed.";
                }
                return _serverFailureMessage;
            }
            set
            {
                _serverFailureMessage = value;
            }
        }
    }
}
