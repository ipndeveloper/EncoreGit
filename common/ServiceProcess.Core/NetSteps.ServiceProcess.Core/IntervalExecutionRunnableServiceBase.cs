using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.ServiceProcess
{
	public class IntervalExecutionRunnableServiceConfiguration
	{
		public TimeSpan ServiceExecutionInterval { get; set; }

		public bool AlignServiceExecutionToClock { get; set; }

		public bool ExecuteImmediately { get; set; }
	}

	public abstract class IntervalExecutionRunnableServiceBase : RunnableServiceBase
	{
		#region Properties

		public IntervalExecutionRunnableServiceConfiguration Configuration { get; protected set; }

		protected Timer ServiceTrigger { get; set; }

		private ElapsedEventHandler CurrentElapsed { get; set; } 

		#endregion

		#region Constructors
		
		public IntervalExecutionRunnableServiceBase()
		{
			Configuration = new IntervalExecutionRunnableServiceConfiguration() { AlignServiceExecutionToClock = true, ServiceExecutionInterval = TimeSpan.FromMinutes(15) };
			ServiceTrigger = new Timer();
		} 

		#endregion

		#region Methods
		
		protected override void OnStart(string[] args)
		{
			TimeSpan interval = Configuration.ServiceExecutionInterval;
			TimeSpan next = TimeSpan.MinValue;
			var now = DateTime.Now;
			if (Configuration.AlignServiceExecutionToClock)
			{
				this.TraceInformation("Aligning service execution to clock...");
				if (interval.Ticks < TimeSpan.TicksPerHour)
				{
					var nextExec = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
					this.TraceInformation(String.Format("Service execution sub-hourly, calculating next execution point from current hour '{0}'...", nextExec));
					while (nextExec < now)
					{
						nextExec = nextExec.Add(interval);
					}
					next = nextExec - now;
				}
				else if(interval.Ticks < TimeSpan.TicksPerDay)
				{
					this.TraceInformation(String.Format("Service execution hourly or greater, calculating next execution point from current day '{0}'...", now.Date));
					var nextExec = now.Date.Add(interval);
					while (nextExec < now)
					{
						nextExec = nextExec.Add(interval);
					}
					next = nextExec - now;
				}
				else
				{
					this.TraceInformation(String.Format("Service execution daily or greater, calculating next execution point from current day '{0}'...", now.Date));
					var nextExec = now.Date.Add(interval);
					next = nextExec - now;
				}
				
				this.TraceInformation(String.Format("Next execution in '{0}' at '{1}' and then every '{2}' thereafter.", next, now.Add(next), interval));

				CurrentElapsed = ServiceTrigger_AlignedElapsed;
				ServiceTrigger.Elapsed += CurrentElapsed;
				ServiceTrigger.Interval = next.TotalMilliseconds;
			}
			else
			{
				this.TraceInformation(String.Format("Service execution is not alligned to clock.  Next execution in '{0}' at '{1}'", interval, now.Add(interval)));

				CurrentElapsed = ServiceTrigger_Elapsed;
				ServiceTrigger.Elapsed += CurrentElapsed;
				ServiceTrigger.Interval = interval.TotalMilliseconds;
			}
			ServiceTrigger.Start();

			if(Configuration.ExecuteImmediately)
			{
				this.TraceInformation("Service execution set to Immediate, executing now...");
				Task.Factory.StartNew(() =>
				{
					ExecuteService();
				});
			}
		}

		protected override void OnPause()
		{
			this.OnStop();
		}

		protected override void OnContinue()
		{
			this.OnStart(null);
		}

		protected override void OnStop()
		{
			ServiceTrigger.Stop();
			ServiceTrigger.Elapsed -= CurrentElapsed;
			StopService();
		}

		void ServiceTrigger_AlignedElapsed(object sender, ElapsedEventArgs e)
		{
			ServiceTrigger.Stop();
			ServiceTrigger.Elapsed -= CurrentElapsed;
			CurrentElapsed = ServiceTrigger_Elapsed;
			ServiceTrigger.Elapsed += CurrentElapsed;
			ServiceTrigger.Interval = Configuration.ServiceExecutionInterval.TotalMilliseconds;
			ServiceTrigger.Start();

			ExecuteService();
		}

		void ServiceTrigger_Elapsed(object sender, ElapsedEventArgs e)
		{
			ExecuteService();
		}

		protected abstract void ExecuteService();

		protected abstract void StopService();
		#endregion
	}
}
