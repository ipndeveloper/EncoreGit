using System;
using System.Timers;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Loggers
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to send a 'heart beat' to the DB to show applications currently running that are connected to the DB.
	/// Created: 03/23/2010
	/// </summary>
	public class ApplicationRunningInstanceLogger
	{
		#region Singleton
        public static object _syncRoot = new object();
		public static ApplicationRunningInstanceLogger Instance
		{
			get { return Singleton.instance; }
		}

        private ApplicationRunningInstanceLogger()
        {
            _applicationPulseTimer.Elapsed -= new ElapsedEventHandler(_applicationPulseTimer_Elapsed);
            _applicationPulseTimer.Elapsed += new ElapsedEventHandler(_applicationPulseTimer_Elapsed);
            _applicationPulseTimer.Enabled = true;
            _applicationPulseTimer.Start();
        }

        private abstract class Singleton
        {
            static Singleton() { } // DO NOT REMOVE: Static constructor required to prevent beforefieldinit flag from being set
            public static readonly ApplicationRunningInstanceLogger instance = new ApplicationRunningInstanceLogger();
        }
		#endregion

		#region Members
		private ApplicationRunningInstance _applicationRunningInstance = null;
		private Timer _applicationPulseTimer = new Timer(TimeSpan.FromSeconds(15).TotalMilliseconds);
		#endregion

		#region Properties
		public Action<ApplicationRunningInstance> SetWebContextValues = null;
		#endregion

		#region Methods
		public void Start()
		{
			LogApplicationPulse();
		}

		public void Start(Action<ApplicationRunningInstance> setWebContextValues)
		{
			SetWebContextValues = setWebContextValues;
			LogApplicationPulse();
		}

		public void LogApplicationPulse()
		{
			lock (_syncRoot)
			{
				_applicationPulseTimer.Stop();

				if (_applicationRunningInstance == null)
					_applicationRunningInstance = new ApplicationRunningInstance() { StartDate = DateTime.Now.ApplicationNow() };

				_applicationRunningInstance.ApplicationID = ApplicationContext.Instance.ApplicationID;
				_applicationRunningInstance.MachineName = System.Environment.MachineName;
				_applicationRunningInstance.IpAddress = string.Empty; // TODO: Set this to the right IP address. - JHE
				_applicationRunningInstance.LastPingDate = DateTime.Now;

				if (SetWebContextValues != null)
					SetWebContextValues(_applicationRunningInstance);

				_applicationRunningInstance.Save();

				_applicationPulseTimer.Start();
			}
		}

		// TODO: Maybe clean out old instances - JHE
		public void CleanOutOldInstances()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Event Handlers
		void _applicationPulseTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			LogApplicationPulse();
		}
		#endregion
	}
}
