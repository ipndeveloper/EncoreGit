using System;
using System.Windows.Threading;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 11/10/2009
    /// </summary>
    public class DoubleClickDispatcherTimer
    {
        private DispatcherTimer _doubleClickTimer;

        public bool IsDoubleClick
        {
            get
            {
                return _doubleClickTimer.IsEnabled;
            }
        }

        public DoubleClickDispatcherTimer()
        {
            _doubleClickTimer = new DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
        }

        protected void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            // stop the timer after an interval
            _doubleClickTimer.Stop();
        }

        public void Stop()
        {
            _doubleClickTimer.Stop();
        }

        public void Start()
        {
            _doubleClickTimer.Start();
        }
    }
}
