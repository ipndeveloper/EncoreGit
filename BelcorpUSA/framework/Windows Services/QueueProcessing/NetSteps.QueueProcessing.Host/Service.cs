using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using NetSteps.QueueProcessing.Host;

namespace NetSteps.QueueProcessing.Windows.Host
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <param name="args">
		/// The args.
		/// </param>
		public static void Main(string[] args)
		{
			try
			{
				NetSteps.Encore.Core.Wireup.WireupCoordinator.SelfConfigure();

				var logger = Create.New<IQueueProcessorLogger>();

				try
				{
					// Command line Parameters: /debug - to start in normal mode (not as a service)
					//                         /i     - to install
					//                         /u     - to uninstall
					logger.Info("QueueProcessingService Starting");

					Initialize();

					DebuggableService program = new QueueProcessingHost();

					if (args.Any(x => x == "/unitTest"))
					{
						return;
					}

					program.Run(args);
				}
				catch (ReflectionTypeLoadException reflectionTypeLoadException)
				{
					if (reflectionTypeLoadException.LoaderExceptions.CountSafe() > 0)
					{
						throw reflectionTypeLoadException.LoaderExceptions[0];
					}

					throw;
				}
				catch (Exception ex)
				{
					logger.Info(ex.Message + " " + ex.StackTrace);
					EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
					throw;
				}
			}
			catch (ReflectionTypeLoadException reflectionTypeLoadException)
			{
				if (reflectionTypeLoadException.LoaderExceptions.CountSafe() > 0)
				{
					throw reflectionTypeLoadException.LoaderExceptions[0];
				}

				throw;
			}
		}

		private static void Initialize()
		{
			var logger = Create.New<IQueueProcessorLogger>();
			try
			{
				logger.Info("Beginning application initialization");
				InitializeApplicationContext();

				NetSteps.Common.Globalization.Translation.TermTranslation = NetSteps.Data.Entities.Cache.CachedData.Translation;

				InitializeServiceHelper();

				logger.Info("Finished application initialization");
			}
			catch (Exception excp)
			{
				logger.Error("Failed to initialize program: {0}", excp.ToString());
				throw;
			}
		}

		private static void InitializeServiceHelper()
		{
			var logger = Create.New<IQueueProcessorLogger>();
			try
			{
				logger.Info("Beginning service helper initialization");
				// Settings used when install/uninstall this app as a Windows Service (using /i option)
				ServiceHelperInstaller.ServiceAccount = ServiceAccount.LocalService;
				ServiceHelperInstaller.ServiceStartType = ServiceStartMode.Automatic;
				ServiceHelperInstaller.ServiceName = "NetSteps Queue Processer";
				ServiceHelperInstaller.ServiceDisplayName = "NetSteps Queue Processer";
				ServiceHelperInstaller.ServiceDescription = "Service to process email queue";
				logger.Info("Finished service helper initialization");
			}
			catch (Exception excp)
			{
				logger.Error("Failed to initialize service helper: {0}", excp.ToString());
				throw;
			}
		}

		private static void InitializeApplicationContext()
		{
			var logger = Create.New<IQueueProcessorLogger>();

			try
			{
				logger.Info("Beginning application context initialization");

				ApplicationContext.Instance.IsWebApp = false;
				ApplicationContext.Instance.CurrentUser = LoadCurrentUser();
				ApplicationContext.Instance.ApplicationID = GetApplicationId();

				logger.Info("Finished application context initialization");
			}
			catch (Exception excp)
			{
				logger.Error("Failed to initialize application context: {0}", excp.ToString());
				throw;
			}
		}

		private static short GetApplicationId()
		{
			var logger = Create.New<IQueueProcessorLogger>();
			try
			{
				logger.Info("Getting application id");
				var applicationId = EntitiesHelpers.GetApplicationIdFromConnectionString();
				logger.Info("Got application id: {0}", applicationId);
				return applicationId;
			}
			catch (Exception excp)
			{
				logger.Error("Failed to get application id: {0}", excp.ToString());
				throw;
			}
		}

		private static IUser LoadCurrentUser()
		{
			var logger = Create.New<IQueueProcessorLogger>();

			try
			{
				logger.Info("Loading current user");
				var currentUser = CorporateUser.LoadFull(1); // Defaulting user to Admin for now - JHE 
				logger.Info("Loaded current user with userId {0}", currentUser.UserID);
				return currentUser;
			}
			catch (Exception excp)
			{
				logger.Error("Failed to load current user: {0}", excp.ToString());
				throw;
			}
		}
	}
}