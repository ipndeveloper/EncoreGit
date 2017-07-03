using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using BelcorpUSA.Edi.Common;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;
using BelcorpUSA.Edi.ServiceHost.Configuration;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Configuration;
using NetSteps.ServiceProcess;
using System.Threading;

namespace BelcorpUSA.Edi.ServiceHost
{
	public partial class BelcorpUSAEdiServiceHost : IntervalExecutionRunnableServiceBase
	{
		private object _lock = new object();
		private bool executing = false;

		public HostConfigurationSection EdiConfiguration { get; set; }
		private IEdiService EdiService { get; set; }

		public BelcorpUSAEdiServiceHost()
			: base()
		{
			InitializeComponent();

			this.CanPauseAndContinue = true;

			EdiConfiguration = ConfigurationUtility.GetSection<HostConfigurationSection>();

			Configuration.AlignServiceExecutionToClock = EdiConfiguration.AlignServiceExecutionToClock;
			Configuration.ServiceExecutionInterval = EdiConfiguration.ServiceExecutionInterval;
			Configuration.ExecuteImmediately = EdiConfiguration.ExecuteImmediately;

			WireupCoordinator.SelfConfigure();

			EdiService = Create.New<IEdiService>();
		}

		protected override void ExecuteService()
		{
			using (var activity = this.TraceActivity("Edi Service Execution called..."))
			{
				if (!executing)
				{
					lock (_lock)
					{
						if (!executing)
						{
							try
							{
								executing = true;

								using (var processOrdersActivity = this.TraceActivity("EDI Service Process Confirmations"))
								{
									EdiService.ProcessConfirmations();
								}
								using (var processShipNoticesActivity = this.TraceActivity("EDI Service Process Ship Notices"))
								{
									EdiService.ProcessShipNotices();
								}
								using (var processOrdersActivity = this.TraceActivity("EDI Service Process Orders"))
								{
									EdiService.ProcessOrders();
								}
							}
							catch (Exception e)
							{
								this.TraceException(e);
							}
							finally
							{
								executing = false;
							}
						}
					}
				}
				else
				{
					this.TraceInformation("Execution already running, bypassing this exectuion...");
				}
			}
		}

		protected override void StopService()
		{
			while (executing)
			{
				//EDI Service currently doesn not support stopping in mid process, let it finish then return...
				Thread.Sleep(250);
			}
		}
	}
}
