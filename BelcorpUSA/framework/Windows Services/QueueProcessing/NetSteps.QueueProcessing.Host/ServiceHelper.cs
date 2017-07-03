using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;

namespace NetSteps.QueueProcessing.Host
{
    #region ServiceShell
    /// <summary>
    ///  This is a shell class to help create a service that can be run either as a service or as a normal application.
    /// </summary>
    class ServiceShell : ServiceBase
    {
        private DebuggableService debuggableService;

        public ServiceShell(DebuggableService serviceHelper)
        {
            debuggableService = serviceHelper;
        }

        protected override void OnContinue()
        {
            debuggableService.OnContinue();
        }
        protected override void OnCustomCommand(int command)
        {
            debuggableService.OnCustomCommand(command);
        }
        protected override void OnPause()
        {
            debuggableService.OnPause();
        }
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return debuggableService.OnPowerEvent(powerStatus);
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            debuggableService.OnSessionChange(changeDescription);
        }
        protected override void OnShutdown()
        {
            debuggableService.OnShutdown();
        }
        protected override void OnStart(string[] args)
        {
            debuggableService.OnStart(args);
        }
        protected override void OnStop()
        {
            debuggableService.OnStop();
        }

        public IntPtr PublicServiceHandle { get { return ServiceHandle; } }

        private void InitializeComponent()
        {
            // 
            // ServiceShell
            // 
            this.ServiceName = "fsv_gys_BelcorpBRA_Queue";

        }
    }
    #endregion

    /// <summary>
    /// When creating a service, inherit from this class instead of ServiceBase. This class allows the program to be run with 
    /// a /debug option. When debugging it doesn't run as a service. This class was created to allow the program to run either way.
    /// </summary>
    public class DebuggableService : IServiceHost
    {
        protected IQueueProcessorLogger Logger {get;set;}

		/*
		 * I commented all of this stuff out because if somebody tries to get any of those values, it will blow up.
		 * Mainly because the serviceShell isn't being set.
		 * Also, I checked and it looked like none of those things were even being called...
		 */

		//private ServiceShell serviceShell;

        public DebuggableService()
        {
            Logger = Create.New<IQueueProcessorLogger>();
        }

		//public int MaxNameLength { get { return ServiceShell.MaxNameLength; } }

		//public bool AutoLog { get { return serviceShell.AutoLog; } set { serviceShell.AutoLog = value; } }

		//public bool CanHandlePowerEvent { get { return serviceShell.CanHandlePowerEvent; } set { serviceShell.CanHandlePowerEvent = value; } }

		//public bool CanHandleSessionChangeEvent { get { return serviceShell.CanHandleSessionChangeEvent; } set { serviceShell.CanHandleSessionChangeEvent = value; } }

		//public bool CanPauseAndContinue { get { return serviceShell.CanPauseAndContinue; } set { serviceShell.CanPauseAndContinue = value; } }

		//public bool CanShutdown { get { return serviceShell.CanShutdown; } set { serviceShell.CanShutdown = value; } }

		//public bool CanStop { get { return serviceShell.CanStop; } set { serviceShell.CanStop = value; } }

		//public int ExitCode { get { return serviceShell.ExitCode; } set { serviceShell.ExitCode = value; } }

		//protected IntPtr ServiceHandle { get { return serviceShell.PublicServiceHandle; } }

		//public void RequestAdditionalTime(int milliseconds)
		//{
		//    serviceShell.RequestAdditionalTime(milliseconds);
		//}
		//public void ServiceMainCallback(int argCount, IntPtr argPointer)
		//{
		//    serviceShell.ServiceMainCallback(argCount, argPointer);
		//}
		//public void Stop()
		//{
		//    serviceShell.Stop();
		//}
        public virtual void OnContinue()
        {
        }
        public virtual void OnCustomCommand(int command)
        {
        }
        public virtual void OnPause()
        {
        }
        public virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return true;
        }
        public virtual void OnSessionChange(SessionChangeDescription changeDescription)
        {
        }
        public virtual void OnShutdown()
        {
        }
        public virtual void OnStart(string[] args)
        {
        }
        public virtual void OnStop()
        {
        }

        public bool IsDebug = false;

        static object persistObject = null;

        public void Run(string[] args)
        {
            // case insensitive contains 
            foreach (string s in args)
            {
                // case insenitive compare, allows /Debug to be in any case
	            if(string.Compare(s, "/debug", true) == 0)
	            {
		            IsDebug = true;
	            }

                // case insenitive compare, allows /Debug to be in any case
                if (string.Compare(s, "/u", true) == 0 || string.Compare(s, "/uninstall", true) == 0 ||
                        string.Compare(s, "/i", true) == 0 || string.Compare(s, "/install", true) == 0)
                {
                    InstallOrUninstall(args);
                    return; // exit after uninstall
                }
            }

            if (IsDebug)
            {
                // this is a very odd need. When we run in debug mode we don't have a persistent reference to the main program because we lanuched threads and then returned
                // this is needed to keep a reference around so the garbage collector won't terminate the program.
                persistObject = this;
                OnStart(args);

            }
            else
            {
                var ServicesToRun = new ServiceBase[] 
					{ 
						new ServiceShell(this) 
					};
                ServiceBase.Run(ServicesToRun);
            }
        }

        ~DebuggableService()
        {
            // if we're debugging then we need to call the on stop method in the finalizer because it won't get called automatically like
            // it does when it's a service
            Logger.Debug("Debuggable Service Finalizer, IsDebug: {0}", IsDebug);
            if (IsDebug)
            {
                OnStop();
            }
        }

        private void InstallOrUninstall(string[] args)
        {
            // because sometimes the program is run without the .exe this adds it if needed
            var list = new List<string>();
            string programName = Environment.GetCommandLineArgs()[0];
	        if(Path.GetExtension(programName).ToLower() == "")
	        {
		        programName += ".exe";
	        }

            list.Add(programName);
            list.AddRange(args);
            ManagedInstallerClass.InstallHelper(list.ToArray());
        }
    }

    [RunInstaller(true)]
    public class ServiceHelperInstaller : Installer
    {
        static public ServiceAccount ServiceAccount = ServiceAccount.LocalService;
        static public ServiceStartMode ServiceStartType = ServiceStartMode.Automatic;
        static public string ServiceName = "NetstepsService40"; // default value should be set in Main()
        static public string ServiceDisplayName = "Netsteps Service40";
        static public string ServiceDescription = "Netsteps Service40";
        //static public string ServiceUserName = "NETSTEPS"+"\\"+"SVC_BUS_PRD_PROCESS";
        //static public string ServicePassword = "gePTF1!R&gSn";

        private ServiceInstaller serviceInstaller1;
        private ServiceProcessInstaller processInstaller;

        public ServiceHelperInstaller()
        {
            // Instantiate installers for process and services.
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller1 = new ServiceInstaller();

            processInstaller.Account = ServiceAccount;
            //processInstaller.Username = ServiceUserName;
            //processInstaller.Password = ServicePassword;

            serviceInstaller1.StartType = ServiceStartType;
            serviceInstaller1.ServiceName = ServiceName;
            serviceInstaller1.DisplayName = ServiceDisplayName;
            serviceInstaller1.Description = ServiceDescription;

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller1);
            Installers.Add(processInstaller);
        }
    }
}