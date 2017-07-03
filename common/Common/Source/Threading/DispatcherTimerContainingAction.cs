using System;
using System.Windows.Threading;

namespace NetSteps.Common.Threading
{
	public class DispatcherTimerAction : DispatcherTimer
	{
		public DispatcherTimerAction()
		{
			this.Tick -= new EventHandler(DispatcherTimerContainingAction_Tick);
			this.Tick += new EventHandler(DispatcherTimerContainingAction_Tick);
		}

		private void DispatcherTimerContainingAction_Tick(object sender, EventArgs e)
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
