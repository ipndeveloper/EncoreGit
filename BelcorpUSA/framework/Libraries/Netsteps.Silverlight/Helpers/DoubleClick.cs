using System;
using System.Windows.Threading;

namespace NetSteps.Silverlight
{
    public static class DoubleClick
    {
        public static object _lock = new object();
        private static DoubleClickInstance _instance;
        public static DoubleClickInstance Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DoubleClickInstance();
                        //_instance.Click += new EventHandler(ClickAction);
                        //_instance.DoubleClick += new EventHandler(DoubleClickAction);
                    }
                    return _instance;
                }
            }
        }

        public static bool IsDoubleClick(object sender)
        {
            return Instance.IsDoubleClick(sender);
        }

        public static void CheckClick(object sender, Action<object> click, Action<object> doubleClick)
        {
            ClickAction = (o, e) => { ClickAction = null; click(o); };
            DoubleClickAction = (o, e) => { DoubleClickAction = null; doubleClick(o); };

            Instance.IsDoubleClick(sender);
        }

        private static Action<object, EventArgs> ClickAction;
        private static Action<object, EventArgs> DoubleClickAction;

        public static void Stop()
        {
            Instance.Stop();
        }
    }

    public class DoubleClickInstance
    {
        private DispatcherTimer _doubleClickTimer;
        private object _originalSender;

        public DoubleClickInstance()
        {
            _doubleClickTimer = new DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
        }

        protected void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            // stop the timer after an interval
            Stop();
            if (Click != null)
                Click(_originalSender, new EventArgs());
        }

        internal bool IsDoubleClick(object sender)
        {
            if (_originalSender != null && _originalSender.Equals(sender) && _doubleClickTimer.IsEnabled)
            {
                Stop();
                if (DoubleClick != null)
                    DoubleClick(_originalSender, new EventArgs());
                return true;
            }
            else
            {
                Start(sender);
                return false;
            }
        }

        internal void Stop()
        {
            _originalSender = null;
            _doubleClickTimer.Stop();
        }

        internal void Start(object sender)
        {
            if (_doubleClickTimer.IsEnabled)
                Stop();

            _doubleClickTimer.Start();
            _originalSender = sender;
        }

        public event EventHandler Click;
        public event EventHandler DoubleClick;
    }
}
