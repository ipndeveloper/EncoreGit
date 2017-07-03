using System;
using NetSteps.Silverlight.Threading;

namespace NetSteps.Silverlight
{
    public class TimerHelper
    {
        /// <summary>
        /// Example:
        /// TimerHelper.SetTimeout(1, () => { DrawConnectingLines(downline); });
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="func"></param>
        public static void SetTimeout(int milliseconds, Action func)
        {
            AppFactory.Dispatcher.BeginInvoke(() =>
           {
               var timer = new DispatcherTimerAction
               {
                   Interval = new TimeSpan(0, 0, 0, 0, milliseconds),
                   Action = func
               };
               timer.Tick += _onTimeout;
               timer.Start();
           });
        }

        private static void _onTimeout(object sender, EventArgs arg)
        {
            var t = sender as DispatcherTimerAction;
            t.Stop();
            t.Action();
            t.Tick -= _onTimeout;
        }

        public class Timer
        {
            private TimeSpan _elapsedTime = new TimeSpan();
            public TimeSpan ElapsedTime
            {
                get
                {
                    _elapsedTime = TimeSpan.FromTicks(DateTime.Now.Ticks - TotalTicks);
                    return _elapsedTime;
                }
            }
            public long TotalTicks = 0;

            public void Start()
            {
                TotalTicks = DateTime.Now.Ticks;
            }

            public void Stop()
            {

            }

            public void Reset()
            {
                _elapsedTime = new TimeSpan();
                TotalTicks = 0;
            }

            public TimeSpan GetAverage(int loops)
            {
                if (loops == 0)
                    return new TimeSpan();
                else
                    return TimeSpan.FromTicks((DateTime.Now.Ticks - TotalTicks) / loops);
            }
        }
    }

}
