using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Sites.Common;
using NetSteps.Web;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Business.Exceptions;
using NetSteps.Web.Mvc.Business.Inheritance;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using nsCore.Extensions;
using StackExchange.Profiling;
using System.IO;
using NetSteps.Diagnostics.Utilities;
using System.Globalization;
using System.Threading;
using NetSteps.Web.Mvc.Helpers;
using System.Collections.Generic;
namespace nsCore
{
	public class MvcApplication : HttpApplication
	{
		private static Task _runningTask;

		public static void RegisterRoutes(RouteCollection routes)
		{
			ControllerBuilder.Current.DefaultNamespaces.Add("nsCore.Controllers");

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon\.ico(/.*)?" });
			routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg|png)(/.*)?" });
			routes.IgnoreRoute("Content/{*path}");
			routes.IgnoreRoute("FileUploads/{*path}");
			routes.IgnoreRoute("Scripts/{*path}");

			routes.MapRoute("GoogleAnalytics", "GoogleAnalytics", new { controller = "Analytics", action = "GoogleAnalytics" }, new[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			routes.MapRoute("Login", "Login", new { controller = "Security", action = "Login", id = "" });
			routes.MapRoute("Logout", "Logout", new { controller = "Security", action = "Logout", id = "" });
			routes.MapRoute("SelfTest", "SelfTest", new { controller = "SelfTest", action = "Index" }, new[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			routes.MapRoute("Edit", "Edit/{action}/{id}", new { controller = "Edit", action = "Index", id = UrlParameter.Optional }, new[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			routes.MapRoute("Addresses", "Addresses/{action}", new { controller = "AddressesController" }, new[] { "NetSteps.Addresses.UI.Mvc.Controllers" });
			routes.MapRoute("MediaLibrary", "MediaLibrary/ThumbImage", new { controller = "MediaLibrary", action = "ThumbImage" }, new[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			routes.MapRoute("Authorization", "Security/Authorize", new { controller = "Security", action = "Authorize" }, new[] { "nsCore.Controllers" });
			routes.MapRoute("GetAuditHistory", "Security/GetAuditHistory", new { controller = "Security", action = "GetAuditHistory"}, new[] { "nsCore.Controllers" });

			RegisterRedirectRoutes(routes);

			routes.MapAreaRoute("Default", "{controller}/{action}/{id}", "Accounts", new { controller = "Landing", action = "Index", id = "" });
		}

		protected void Application_Start()
		{
			try
			{
				if (ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					MiniProfilerEF.InitializeEF42(); //Note, this should be a temporary solution (for a problem with the normal Initialize method) until EF 4.2 is widely available.
					MiniProfiler.Settings.ShowControls = true;
					MiniProfiler.Settings.StackMaxLength = 120000;
				}
				WireupCoordinator.SelfConfigure();

				if (_runningTask != null)
				{
					_runningTask.Wait();
				}

				AreaRegistration.RegisterAllAreas();
				OverridableSite.Register();

				RegisterRoutes(RouteTable.Routes);
				BundleConfig.RegisterBundles(BundleTable.Bundles);

               
               //DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("he-il");
               
				





                //DateTimeContextModelBinder.culture = CoreContext.CurrentCultureInfo.ToString();
                //ModelBinders.Binders.Add(typeof(DateTime), new DateTimeContextModelBinder());
                    
				var configValue = ConfigurationManager.AppSettings["ApplicationId"];
				short applicationId;
				var configValueValid = short.TryParse(configValue, out applicationId);
				if (!configValueValid)
				{
					applicationId = EntitiesHelpers.GetApplicationIdFromConnectionString();
				}
				ApplicationContext.Instance.ApplicationID = applicationId;
				ExceptionLogger.SetWebContextValues = ExceptionHelpers.SetWebContextValues;
				Translation.TermTranslation = CachedData.Translation;

               

                ModelBinders.Binders.Add(typeof(IEnrollmentContext), new EnrollmentContextModelBinder());

                ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeModelBinder());

                ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

                ModelBinders.Binders.Add(typeof(decimal?), new NullableDecimalModelBinder());
                
                //DecimalModelBinder.culture = CoreContext.CurrentCultureInfo;
                //ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
                //ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());



                //var cu = ApplicationContext.Instance.CurrentCultureInfo;
                //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CoreContext.CurrentCultureInfo.ToString());
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

     


     
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				MiniProfiler.Start();
			}

            // Inicio : 12042017 ==> Agregado por IPN ==> para cambiar el culture info establecido propio del servidor donde es desplegado el app y seteando la configuración establecida para definir el idioma del app.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            // Fin  : 12042017

            
			this.DenyRequestsFromUnauthorizedIPs(ConfigurationManager.AppSettings["MaintenancePagePath"] ?? "~/_app_offline.htm");

			var siteService = Create.New<ISiteService>();
			var settings = siteService.GetGoogleAnalyticsSettings(HttpContext.Current.Request.Url);
			HttpContext.Current.Items.Add("CurrentSiteAnalyticsSettings", settings);
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			MiniProfiler.Stop();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			var app = (MvcApplication)sender;
			this.TraceError(new DeferredTraceEvent(w =>
			{
				if (app != null && app.Context != null)
				{
					if (app.Context.Request != null)
					{
						w.WriteLine("Url: {0}", app.Context.Request.Url);
						w.WriteLine("Headers");
						foreach (string key in app.Context.Request.Headers.AllKeys) w.WriteLine("{0}: {1}", key, app.Context.Request.Headers[key]);
						w.WriteLine("Error: {0}", app.Context.Error);
					}
					if (app.Context.AllErrors.Length > 1) w.WriteLine("This request resulted in multiple exceptions see subsequent log messages for exception details");
				}
				else w.WriteLine("An application error occurred but no application context could be found");
			}));
			if (app.Context != null && app.Context.AllErrors.Length > 1)
			{
				for (int index = 0; app.Context.AllErrors.Length > index; index++) this.TraceException(app.Context.AllErrors[index]);
			}

			new NetSteps.Web.Mvc.Controls.Controllers.ErrorController().Application_Error(
				sender as MvcApplication,
				e
			);
		}

		protected void Application_End()
		{
			if (_runningTask == null)
			{
				_runningTask = new Task(DataAccess.CloseAllSqlDependencies);
			}
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			Session.Timeout = Request.IsLocal ? 300 : Session.Timeout;
		}

		/// <summary>
		/// Applies UrlRedirects from the database.
		/// </summary>
		private static void RegisterRedirectRoutes(RouteCollection routes)
		{
			try
			{
				routes.RegisterRedirectRoutes(
					UrlRedirect.GetUrlRedirects(
						new[]
						{
							(short)Constants.SiteType.GlobalManagementPortal
						}),
					ex => ex.Log());
			}
			catch (Exception ex)
			{
				// If this fails, don't bring down the whole site. Just log and continue.
				ex.Log();
			}
		}
	}
}
