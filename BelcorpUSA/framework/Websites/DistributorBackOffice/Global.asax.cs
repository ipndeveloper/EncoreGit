using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DistributorBackOffice.Controllers;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Sites.Common;
using NetSteps.Web;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Business.Exceptions;
using NetSteps.Web.Mvc.Business.Inheritance;
using NetSteps.Web.Mvc.Business.Routing;
using NetSteps.Web.Mvc.Controls.Controllers;
using NetSteps.Web.Mvc.Helpers;
using StackExchange.Profiling;
using NetSteps.Diagnostics.Utilities;
using System.Globalization;

namespace DistributorBackOffice
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			ControllerBuilder.Current.DefaultNamespaces.Add("DistributorBackOffice.Controllers");

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon\.ico(/.*)?" });
			routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg|png)(/.*)?" });
			routes.IgnoreRoute("Content/{*path}");
			routes.IgnoreRoute("FileUploads/{*path}");
			routes.IgnoreRoute("Scripts/{*path}");

			routes.MapRoute("Login", "Login", new { controller = "Security", action = "Login", id = "" });
			routes.MapRoute("Logout", "Logout", new { controller = "Security", action = "Logout", id = "" });
			routes.MapRoute("ForgotPassword", "ForgotPassword", new { controller = "Security", action = "ForgotPassword", id = "" });
			routes.MapRoute("ResetPassword", "ResetPassword", new { controller = "Security", action = "ResetPassword", id = "" });
			routes.MapRoute("Optout", "Optout", new { area = "Communication", controller = "Email", action = "Optout", id = "" });

			routes.MapRoute("Enrollment", "Enrollment", new { area = "Account", controller = "Autoships", action = "RedirectToPWSEnrollment", id = "" });
			routes.MapRoute("Enroll", "Enroll", new { area = "Account", controller = "Autoships", action = "RedirectToPWSEnrollmentPC", id = "" });

			routes.MapRoute("GoToUrl", "GoToUrl/{name}", new { controller = "IFrame", action = "GoToUrl", name = "" });

			routes.MapRoute("Edit", "Edit/{action}/{id}", new { controller = "Edit", action = "Index", id = UrlParameter.Optional }, new string[] { "NetSteps.Web.Mvc.Controls.Controllers" });
			routes.MapRoute("SelfTest", "SelfTest", new { controller = "SelfTest", action = "Index" }, new string[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			RegisterRedirectRoutes(routes);

			routes.MapRoute("Default", "{*path}", new { controller = "Master", action = "GenerateView", id = "" }, new { CmsPage = new CmsPageRouteConstraint() });
            routes.MapRoute("Home", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("GlobalSearch", "Home/GlobalSearch", new { controller = "Home", action = "GlobalSearch" });

			routes.MapRoute("Addresses", "Addresses/{action}", new { controller = "AddressesController" }, new string[] { "NetSteps.Addresses.UI.Mvc.Controllers" });
			routes.MapRoute("MediaLibrary", "MediaLibrary/ThumbImage", new { controller = "MediaLibrary", action = "ThumbImage" }, new string[] { "NetSteps.Web.Mvc.Controls.Controllers" });
		}

		protected void Application_Start()
		{
			this.TraceInformation(String.Format("Starting {0}", GetType().FullName));
			try
			{
				if (System.Configuration.ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					MiniProfilerEF.InitializeEF42(); //Note, this should be a temporary solution (for a problem with the normal Initialize method) until EF 4.2 is widely available.
					MiniProfiler.Settings.ShowControls = true;
					MiniProfiler.Settings.StackMaxLength = 120000;
				}

				WireupCoordinator.SelfConfigure();

				ViewEngines.Engines.Clear();
				ViewEngines.Engines.Add(new RazorViewEngine());
				ViewEngines.Engines.Add(new WebFormViewEngine());

				AreaRegistration.RegisterAllAreas();
				OverridableSite.Register();

				RegisterRoutes(RouteTable.Routes);
				BundleConfig.RegisterBundles(BundleTable.Bundles);
                //int Language = 0;

                //if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ApplicationDefaultLanguageID"], out Language) == true)
                //{
                //    NetSteps.Common.ApplicationContextCommon.Instance.CurrentLanguageID = Language;
                //    ApplicationContext.Instance.CurrentLanguageID = Language;
                
                //}
                   

				ApplicationContext.Instance.ApplicationID = EntitiesHelpers.GetApplicationIdFromConnectionString();
                //NetSteps.Common.ApplicationContextCommon.Instance.CurrentLanguageID = 1;
               

				ExceptionLogger.SetWebContextValues = ExceptionHelpers.SetWebContextValues;

                //var pto = CachedData.Translation; 
				Translation.TermTranslation = CachedData.Translation;

				EditController.SetViewData = controller =>
				{
					try
					{
						Site currentSite;
						if (controller is BaseController)
						{
							currentSite = ((BaseController)controller).CurrentSite;
						}
						else
						{
							currentSite = BaseController.GetCurrentSite();
						}
						HtmlSection section = currentSite.GetHtmlSectionByName("Logo");
						controller.ViewData["Logo"] = section;
					}
					catch (Exception)
					{
						//Session["LastException"] = ex;
					}
				};
				EditController.VerifyEditModeAction = returnUrl => BaseController.GetPageMode() != NetSteps.Common.Constants.ViewingMode.Edit ? new RedirectResult(returnUrl) : null;
				EditController.CorporateLoggedIn = () => CoreContext.CurrentUser != null && CoreContext.CurrentUser is CorporateUser;
				EditController.CorporateUnauthorizedAction = (context) =>
				{
					if (context.Request.IsAjaxRequest())
					{
						return new JsonResult()
						{
							Data = new { result = false, message = Translation.GetTerm("MustBeLoggedIn", "You must be logged in.") },
							JsonRequestBehavior = JsonRequestBehavior.AllowGet
						};
					}
					else
					{
						return new RedirectResult("~/Home");
					}
				};

				var accountAlertUIService = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Communication.UI.Common.IAccountAlertUIService>();
				accountAlertUIService.Providers.Add(
					NetSteps.Communication.Common.CommunicationConstants.AccountAlertProviderKey.Promotion,
					new Lazy<NetSteps.Communication.UI.Common.IAccountAlertUIProvider>(() => NetSteps.Encore.Core.IoC.Create.New<NetSteps.Promotions.UI.Common.Interfaces.IPromotionAccountAlertUIProvider>())
					);

               
				ModelBinders.Binders.Add(typeof(IAddressUIModel), new NetSteps.Addresses.UI.Mvc.ModelBinders.AddressModelBinder());
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
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw ex;
			}
		}

		/// <summary>
		/// This is to simulate a load test on the downline cache to test it's 
		/// stability (and proper 'locking')/ thread safty. - JHE
		/// </summary>
		public void LoadTestDownlineCacheHits()
		{
			try
			{
				var options = new ParallelOptions();
				Parallel.For(0, 1000, options, item => DownlineCache.GetDownline(200811));
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw;
			}
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (System.Configuration.ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				MiniProfiler.Start();
			}
			this.DenyRequestsFromUnauthorizedIPs(System.Configuration.ConfigurationManager.AppSettings["MaintenancePagePath"] ?? "~/_app_offline.htm");

			var siteService = Create.New<ISiteService>();
			var settings = siteService.GetGoogleAnalyticsSettings(HttpContext.Current.Request.Url);
			HttpContext.Current.Items.Add("CurrentSiteAnalyticsSettings", settings);


           
            // Inicio : 12042017 ==> Agregado por IPN ==> para cambiar el culture info establecido propio del servidor donde es desplegado el app y seteando la configuración establecida para definir el idioma del app.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            // Fin  : 12042017

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

			var context = app.Context;
			var ex = app.Server.GetLastError();
			try
			{
				if (ex != null && !(ex is HttpException))
				{
					int? orderId = null;
					try
					{
						if (WebContext.IsSessionAvailable() && HttpContext.Current.Session["CurrentOrder"] != null)
						{
							orderId = (HttpContext.Current.Session["CurrentOrder"] as Order).OrderID;
						}
					}
					catch { }

					int? accountID = null;
					try
					{
						if (WebContext.IsSessionAvailable() && HttpContext.Current.Session["CurrentAccount"] != null)
							accountID = (HttpContext.Current.Session["CurrentAccount"] as Account).AccountID;
					}
					catch { }

					var nsEx = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId, accountID: accountID);
					if (nsEx != null && nsEx.ErrorLog != null && nsEx.ErrorLog is ErrorLog && WebContext.IsSessionAvailable())
					{
						Session["ErrorLogID"] = ((ErrorLog)nsEx.ErrorLog).ErrorLogID;
					}
				}
			}
			catch { }

#if DEBUG
			if (Request.IsLocal)
				return;
#endif

			// redirect to error page
			bool isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);
			context.Response.Clear();
			context.ClearError();
			var httpException = ex as HttpException;

			var routeData = new RouteData();
			routeData.Values["controller"] = "error";
			routeData.Values["exception"] = ex;
			routeData.Values["action"] = isAjaxCall ? "ajaxError" : "index";
			if (!isAjaxCall && httpException != null)
			{
				switch (httpException.GetHttpCode())
				{
					case 404:
						routeData.Values["action"] = "http404";
						break;
					case 403:
						routeData.Values["action"] = "http403";
						break;
					case 500:
						routeData.Values["action"] = "http500";
						break;
				}
			}
			Response.TrySkipIisCustomErrors = true;
			IController controller = new ErrorController();
			controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
		}

		protected void Application_End()
		{
			this.TraceInformation(String.Format("Stopping {0}", GetType().FullName));
			DataAccess.CloseAllSqlDependencies();
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			// http://forums.asp.net/p/1046935/1576341.aspx - JHE
			// Code that runs when a new session is started

			//Ensure SessionID in order to prevent the following exception
			//when the Application Pool Recycles
			//[HttpException]: Session state has created a session id, but cannot 
			//				   save it because the response was already flushed by 
			string sessionId = Session.SessionID;

			//Session.Timeout = Request.IsLocal ? 300 : Session.Timeout;
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
							(short)   NetSteps.Data.Entities.Constants.SiteType.BackOffice
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
