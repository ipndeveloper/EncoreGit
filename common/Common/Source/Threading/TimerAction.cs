using System;
using System.Timers;

namespace NetSteps.Common.Threading
{
	public class TimerAction : Timer
	{
		public TimerAction(double interval)
			: base(interval)
		{
			this.Elapsed -= new ElapsedEventHandler(TimerAction_Tick);
			this.Elapsed += new ElapsedEventHandler(TimerAction_Tick);
		}

		private void TimerAction_Tick(object sender, ElapsedEventArgs e)
		{
			if (Action != null)
				Action();
		}

		/// <summary>
		/// uncomment this to see when the DispatcherTimer2 is collected
		/// if you remove  t.Tick -= _onTimeout; line from _onTimeout method
		/// you will see that the timer is never collected
		/// </summary>
		//~DispatcherTimerContainingAction()
		//{
		//    throw new Exception("DispatcherTimerContainingAction is disposed");
		//}

		public Action Action { get; set; }
	}
}
