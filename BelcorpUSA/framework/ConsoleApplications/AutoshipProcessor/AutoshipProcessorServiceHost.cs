using System;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.Wireup;
using NetSteps.ServiceProcess;
using System.Configuration;
using NetSteps.Data.Entities.Processors;
using NetSteps.Data.Entities.Interfaces;
using System.Threading;

namespace AutoshipProcessor
{
	partial class AutoshipProcessorServiceHost : IntervalExecutionRunnableServiceBase
	{
		#region Fields

		private object _consoleLock = new object();

		private object _lock = new object();
		private bool executing = false;

		#endregion

		#region Properties

		private IAutoshipProcessor Processor { get; set; }
		private DateTime? RunDate { get; set; }
		private int Threads { get; set; }
		private CancellationTokenSource CancellationToken { get; set; }

		#endregion

		#region Constructors

		public AutoshipProcessorServiceHost(string[] args)
			: base()
		{
			InitializeComponent();

			Configuration.ServiceExecutionInterval = TimeSpan.FromHours(12);
			Configuration.AlignServiceExecutionToClock = true;
			Configuration.ExecuteImmediately = true;

			try
			{
				WireupCoordinator.SelfConfigure();
			}
			catch (System.Reflection.ReflectionTypeLoadException RTLex)
			{
				WriteToConsoleAndLog(RTLex.ToString());
				foreach (var item in RTLex.LoaderExceptions)
				{
					WriteToConsoleAndLog(item.Message);
				}
				throw;
			}

			ApplicationContext.Instance.ApplicationID = NetSteps.Data.Entities.EntitiesHelpers.GetApplicationIdFromConnectionString();
			ApplicationContext.Instance.IsWebApp = false;
			ApplicationContext.Instance.CurrentUser = CorporateUser.LoadFull(1); // Defaulting user to Admin for now - JHE

			// Default run date is today, based on corporate time
			var runDate = GetRunDateValue();
			// Allow date override from command line
			if (args.Any())
			{
				DateTime argsDate;
				if (DateTime.TryParse(args[0], out argsDate))
				{
					runDate = argsDate;
				}
			}
			RunDate = runDate;

			// Get number of threads from config, else default to one
			int threads;
			if (!int.TryParse(ConfigurationManager.AppSettings["AutoshipProcessorThreads"], out threads))
			{
				threads = 1;
			}
			Threads = threads;

			// Get processor
			Processor = ProcessorProviders.AutoshipProcessor;
			// Wire up events
			Processor.ProgressMessage += processor_ProgressMessage;
		}

		private DateTime GetRunDateValue()
		{
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ApplicationContext.Instance.CorporateTimeZoneInfo).Date;
		}

		#endregion

		#region Methods

		protected override void ExecuteService()
		{
			using (var activity = this.TraceActivity("Autoship Processor Execution called..."))
			{
				if (!executing)
				{
					lock (_lock)
					{
						if (!executing)
						{
							try
							{
								WriteToConsoleAndLog("Starting Autoship Processor.");

								executing = true;
								CancellationToken = new CancellationTokenSource();
								if (!RunDate.HasValue)
								{
									RunDate = GetRunDateValue();
								}
								WriteToConsoleAndLog("Run date: {0:d}", RunDate);
								Processor.ProcessAllAutoships(RunDate.Value, Threads, CancellationToken);
							}
							catch (Exception e)
							{
								this.TraceException(e);
							}
							finally
							{
								executing = false;
								CancellationToken = null;
								RunDate = null;
							}
						}
					}
				}
			}
		}

		protected override void StopService()
		{
			if (executing && CancellationToken != null)
			{
				CancellationToken.Cancel();
			}
		}

		private void processor_ProgressMessage(object sender, ProgressMessageEventArgs e)
		{
			LogProgressMessage(e);

			lock (_consoleLock)
			{
				if (e.Progress != null)
				{
					WriteToConsole(ConsoleColor.Gray, string.Format("\r\n{0}/{1} ({2}%)", e.Progress.ItemsProcessed, e.Progress.TotalItems, e.Progress.PercentComplete.TruncateDoubleInsertCommas(2)));
				}

				if (e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Error
					|| e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Warning)
				{
					var parts = Regex.Split(e.Message, "\r\n");
					if (parts.CountSafe() > 1)
					{
						WriteToConsole(ConsoleColor.White, parts[0]);
						WriteToConsole(ConsoleColor.Red, string.Join("\r\n", parts.Skip(1)));
					}
					else
						WriteToConsole(ConsoleColor.Red, e.Message);
				}
				else if (e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Successful)
				{
					var parts = Regex.Split(e.Message, "\r\n");
					if (parts.CountSafe() > 1)
					{
						WriteToConsole(ConsoleColor.White, parts[0]);
						WriteToConsole(ConsoleColor.Green, string.Join("\r\n", parts.Skip(1)));
					}
					else
						WriteToConsole(ConsoleColor.Green, e.Message);
				}
				else
				{
					WriteToConsole(ConsoleColor.White, e.Message);
				}
			}
		}

		public void WriteToConsoleAndLog(string format, params object[] arg)
		{
			WriteToConsole(format, arg);
			Log.Info(format, arg);
		}

		public void WriteToConsole(string format, params object[] arg)
		{
			WriteToConsole(ConsoleColor.White, format, arg);
		}

		public void WriteToConsole(ConsoleColor consoleColor, string format, params object[] arg)
		{
			lock (_consoleLock)
			{
				Console.ForegroundColor = consoleColor;
				Console.WriteLine(format, arg);
			}
		}

		private void LogProgressMessage(ProgressMessageEventArgs e)
		{
			switch (e.ApplicationMessageType)
			{
				case NetSteps.Common.Constants.ApplicationMessageType.Error:
					Log.Error(e.Message);
					break;
				case NetSteps.Common.Constants.ApplicationMessageType.Warning:
					Log.Warning(e.Message);
					break;
				case NetSteps.Common.Constants.ApplicationMessageType.Standard:
					Log.Info(e.Message);
					break;
				default:
					Log.Debug(e.Message);
					break;
			}
		}

		#endregion
	}
}
