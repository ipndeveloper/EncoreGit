using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Common.Configuration;
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
using NetSteps.Web.Mvc.Business.Routing;
using NetSteps.Web.Mvc.Controls.Controllers;
using nsDistributor.Controllers;
using StackExchange.Profiling;
using NetSteps.Diagnostics.Utilities;
namespace nsDistributor
{
	public class MvcApplication : HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			ControllerBuilder.Current.DefaultNamespaces.Add("nsDistributor.Controllers");

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon\.ico(/.*)?" });
			routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg|png)(/.*)?" });
			routes.IgnoreRoute("Content/{*path}");
			routes.IgnoreRoute("FileUploads/{*path}");
			routes.IgnoreRoute("Scripts/{*path}");

			bool enablePwsPathRedirect = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.EnablePwsPathRedirect, false);
			bool isSubdomain = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
			string prefix = isSubdomain ? string.Empty : "{distributor}/";

			routes.MapRoute("GoogleAnalytics", "GoogleAnalytics", new { controller = "Analytics", action = "GoogleAnalytics" }, new[] { "NetSteps.Web.Mvc.Controls.Controllers" });
			routes.MapRoute("Home", prefix + "Home", new { controller = "Static", action = "Home" });
			routes.MapRoute("ContactMe", prefix + "ContactMe", new { controller = "Static", action = "ContactMe" });
            routes.MapRoute("SearchState", prefix + "SearchState", new { controller = "Static", action = "SearchState" });
			routes.MapRoute("ContactCorporate", prefix + "ContactCorporate", new { controller = "Static", action = "ContactMe" });
			routes.MapRoute("DesignCenter", prefix + "DesignCenter", new { controller = "Static", action = "DesignCenter" });
			routes.MapRoute("Article", prefix + "News/Article/{id}", new { controller = "Static", action = "Article" });
			routes.MapRoute("News", prefix + "News", new { controller = "Static", action = "News" });
			routes.MapRoute("GetNews", prefix + "GetNews", new { controller = "Static", action = "GetNews" });
			routes.MapRoute("TrackClick", prefix + "TrackClick", new { controller = "Static", action = "TrackClick" });
			routes.MapRoute("OptOut", prefix + "OptOut", new { controller = "Static", action = "OptOut" });
			routes.MapRoute("GetLastPendingOrderByAccount", prefix + "GetLastPendingOrderByAccount", new { controller = "Cart", action = "GetLastPendingOrderByAccount" });

			routes.MapRoute("ReloadInventory", prefix + "Products/ReloadInventory", new { controller = "Shop", action = "ReloadInventory" });

			routes.MapRoute("Account", prefix + "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Shop", prefix + "Shop/{action}/{id}", new { controller = "Shop", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Cart", prefix + "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Checkout", prefix + "Checkout/{action}/{id}", new { controller = "Checkout", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Edit", prefix + "Edit/{action}/{id}", new { controller = "Edit", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Admin", prefix + "Admin/{action}/{id}", new { controller = "Admin", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("HostedParties", prefix + "HostedParties/{action}/{id}", new { controller = "HostedParties", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("RSVP", prefix + "RSVP/{action}/{id}", new { controller = "RSVP", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Error", prefix + "Error/{action}/{id}", new { controller = "Error", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Locate", prefix + "Locate/{action}/{id}", new { controller = "Locate", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Autoship", prefix + "Autoship/{action}/{id}", new { controller = "Autoship", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Design", prefix + "Design/{action}/{id}", new { controller = "Design", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute("Analytics", prefix + "Analytics/{action}/{id}", new { controller = "Analytics", action = "Google", id = UrlParameter.Optional });

			routes.MapRoute("Login", prefix + "Login", new { controller = "Security", action = "Login" });
			routes.MapRoute("Logout", prefix + "Logout", new { controller = "Security", action = "Logout" });
			routes.MapRoute("SignUp", prefix + "SignUp", new { controller = "Security", action = "SignUp" });
			routes.MapRoute("ForgotPassword", prefix + "ForgotPassword", new { controller = "Security", action = "ForgotPassword" });
			routes.MapRoute("ResetPassword", prefix + "ResetPassword", new { controller = "Security", action = "ResetPassword" });
			routes.MapRoute("PasswordUpdated", prefix + "PasswordUpdated", new { controller = "Security", action = "PasswordUpdated" });
			routes.MapRoute("SetLanguage", prefix + "SetLanguage", new { controller = "Security", action = "SetLanguage" });
			routes.MapRoute("SelfTest", "SelfTest", new { controller = "SelfTest", action = "Index" }, new string[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			routes.MapRoute("Addresses", "Addresses/{action}", new { controller = "AddressesController" }, new string[] { "NetSteps.Addresses.UI.Mvc.Controllers" });
			routes.MapRoute("MediaLibrary", "MediaLibrary/ThumbImage", new { controller = "MediaLibrary", action = "ThumbImage" }, new string[] { "NetSteps.Web.Mvc.Controls.Controllers" });

			RegisterRedirectRoutes(routes);

            routes.MapRoute("Default", "Enroll/{controller}/{action}/{id}", new { controller = "Landing", action = "Index", id = UrlParameter.Optional });

            //if (enablePwsPathRedirect)
            //{
            //    routes.MapRoute("Default2", "{subdomain}/{*path}", new { controller = "Master", action = "RedirectToPws" }, new { CmsPage = new PwsRouteConstraint() });
            //}

            //if (isSubdomain)
            //{
            //    routes.MapRoute("Default3", "{*path}", new { controller = "Master", action = "GenerateView" }, new { CmsPage = new CmsPageRouteConstraint() });
            //}
            //else
            //{
            //    routes.MapRoute("Default", prefix + "{*path}", new { controller = "Master", action = "GenerateView" }, new { CmsPage = new CmsPageRouteConstraint() });
            //}

			routes.MapRoute("Home2", "{controller}/{action}/{id}", new { controller = "Static", action = "Home", id = UrlParameter.Optional });

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (System.Configuration.ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				MiniProfiler.Start();
			}
			this.DenyRequestsFromUnauthorizedIPs(System.Configuration.ConfigurationManager.AppSettings["MaintenancePagePath"] ?? "~/_app_offline.htm");

                        
                
            // Inicio : 12042017 ==> Agregado por IPN ==> para cambiar el culture info establecido por del servidor donde es desplegado el app y forzando por lo establecido para definir el idioma del app.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.Name);
            // Fin  : 12042017


			var siteService = Create.New<ISiteService>();
			var settings = siteService.GetGoogleAnalyticsSettings(HttpContext.Current.Request.Url);
			HttpContext.Current.Items.Add("CurrentSiteAnalyticsSettings", settings);
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			MiniProfiler.Stop();
		}

		protected void Application_Start()
		{
			try
			{
				if (System.Configuration.ConfigurationManager.AppSettings["MvcMiniProfilerEnabled"].ToCleanString().Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					MiniProfilerEF.InitializeEF42(); // Note, this should be a temporary solution (for a problem with the normal Initialize method) until EF 4.2 is widely available.
					MiniProfiler.Settings.ShowControls = true;
					MiniProfiler.Settings.StackMaxLength = 120000;
				}
				WireupCoordinator.SelfConfigure();

				// HttpApplicationExtensions.SetupAllowedIpAddressFileSystemWatcher();
				ViewEngines.Engines.Clear();
				ViewEngines.Engines.Add(new RazorViewEngine());
				ViewEngines.Engines.Add(new WebFormViewEngine());

				// Allow client projects to add routes first

				OverridableSite.Register();
				AreaRegistration.RegisterAllAreas();
				RegisterRoutes(RouteTable.Routes);
				BundleConfig.RegisterBundles(BundleTable.Bundles);

				// MvcContrib has been removed for now.
				//var routeDebuggerEnabledValue = ConfigurationManager.AppSettings["RouteDebuggerEnabled"] ?? "false";

				//if (routeDebuggerEnabledValue.ToBool().Equals(true))
				//{
				//    MvcContrib.Routing.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
				//}

				ModelBinders.Binders.Add(typeof(IEnrollmentContext), new NetSteps.Web.Mvc.Controls.Infrastructure.EnrollmentContextModelBinder());
				ModelBinders.Binders.Add(typeof(IAddressUIModel), new NetSteps.Addresses.UI.Mvc.ModelBinders.AddressModelBinder());

				// Remove default MVC validators because they aren't term-translated - JGL
				DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
				var clientDataTypeModelValidatorProvider = ModelValidatorProviders.Providers.FirstOrDefault(x => x is ClientDataTypeModelValidatorProvider);
				if (clientDataTypeModelValidatorProvider != null)
				{
					ModelValidatorProviders.Providers.Remove(clientDataTypeModelValidatorProvider);
				}

				ApplicationContext.Instance.ApplicationID = EntitiesHelpers.GetApplicationIdFromConnectionString();
				ExceptionLogger.SetWebContextValues = ExceptionHelpers.SetWebContextValues;
				Translation.TermTranslation = CachedData.Translation;

				Func<HttpContextBase, ActionResult> unauthorizedAction = (context) =>
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

				EditController.SetViewData = controller =>
				{
					try
					{
						controller.ViewBag.Skin = BaseController.CurrentSite.Settings["Skin"] == null || string.IsNullOrEmpty(BaseController.CurrentSite.Settings["Skin"].Value) ? string.Empty : ("~/Content/Styles/Skin-" + BaseController.CurrentSite.Settings["Skin"].Value + ".css").ResolveUrl();

						controller.ViewBag.Logo = BaseController.CurrentSite.GetHtmlSectionByName("Logo");
						controller.ViewBag.Header = BaseController.CurrentSite.GetHtmlSectionByName("Header");
						controller.ViewBag.Footer = BaseController.CurrentSite.GetHtmlSectionByName("Footer");

						controller.ViewBag.MyPhoto = BaseController.CurrentSite.GetHtmlSectionByName("MyPhoto");

						if (BaseController.SiteOwner != null)
						{
							var contactInfo = BaseController.SiteOwner.AccountPublicContactInfo ?? new AccountPublicContactInfo() { HideName = false, HideEmailAddress = true, HideAddress = true, HidePhoneNumber = true };
							controller.ViewBag.AccountName = contactInfo.HideName ? string.Empty : string.IsNullOrEmpty(contactInfo.Name) ? BaseController.SiteOwner.FullName : contactInfo.Name;
							controller.ViewBag.AccountEmail = contactInfo.HideEmailAddress ? string.Empty : string.IsNullOrEmpty(contactInfo.EmailAddress) ? BaseController.SiteOwner.EmailAddress : contactInfo.EmailAddress;
							controller.ViewBag.AccountPhone = contactInfo.HidePhoneNumber ? string.Empty : string.IsNullOrEmpty(contactInfo.PhoneNumber) ? BaseController.SiteOwner.MainPhone : contactInfo.PhoneNumber;
							controller.ViewBag.AccountTitle = contactInfo.HideTitle ? string.Empty : string.IsNullOrWhiteSpace(contactInfo.Title) ? string.Empty : contactInfo.Title;
						}
					}
					catch (Exception)
					{
						//Session["LastException"] = ex;
					}
				};
				EditController.LoggedIn = () => BaseController.OwnerLoggedIn;
				EditController.UnauthorizedAction = unauthorizedAction;
				EditController.VerifyEditModeAction = returnUrl => BaseController.PageMode != NetSteps.Common.Constants.ViewingMode.Edit ? new RedirectResult(returnUrl) : null;
				EditController.CorporateLoggedIn = () => CoreContext.CurrentUser is CorporateUser;
				EditController.CorporateUnauthorizedAction = unauthorizedAction;

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
			DataAccess.CloseAllSqlDependencies();
		}

		protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
		{
			// only apply session cookie persistence to requests requiring session information
			if ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState) &&
								 Context.Handler is MvcHandler &&
								 (Context.Handler as MvcHandler).RequestContext.RouteData.Values["controller"] != null &&
								 Regex.IsMatch(System.Web.HttpContext.Current.Request.Url.LocalPath, @"^/\w+") &&
								 !ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true))
			{
				var sessionState = System.Configuration.ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
				var cookieName = sessionState != null && !string.IsNullOrEmpty(sessionState.CookieName) ? sessionState.CookieName : "ASP.NET_SessionId";

				var timeout = sessionState != null ? sessionState.Timeout : TimeSpan.FromMinutes(60);

				// Ensure ASP.NET Session Cookies are accessible throughout the subdomains.
				if (Request.Cookies[cookieName] != null && Session != null && Session.SessionID != null)
				{
					var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
					var distributor = !Regex.IsMatch(localPath, @"^/\w+") ? string.Empty : localPath.Substring(0, localPath.IndexOf('/', 1) > 0 ? localPath.IndexOf('/', 1) : localPath.Length - 1);

					// Redirect to the base site if no vanity name was supplied - DES
					if (string.IsNullOrEmpty(distributor))
					{
						var url = SiteCache.GetSiteByID(ConfigurationManager.GetAppSetting<int>("BaseEnglishSiteID")).SiteUrls.FirstOrDefault(su => Regex.IsMatch(su.Url, @"/\w+$") && su.IsPrimaryUrl);
						if (url != null)
							distributor = url.Url.Substring(url.Url.LastIndexOf("/"));
					}
					Response.Cookies[cookieName].Value = Session.SessionID;
					Response.Cookies[cookieName].Path = distributor;
					Response.Cookies[cookieName].Expires = DateTime.Now.Add(timeout);
				}
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
				// We have to apply redirects for both PWS & CWS site types because we don't know which site type we are yet.
				// TODO: We need to separate PWS and CWS sites so they can have unique Web.configs, then this can be fixed. - Lundy
				routes.RegisterRedirectRoutes(
					 UrlRedirect.GetUrlRedirects(
						  new[]
						{
							(short)Constants.SiteType.Replicated,
							(short)Constants.SiteType.Corporate
						}
					 ),
					 ex => ex.Log()
				);
			}
			catch (Exception ex)
			{
				// If this fails, don't bring down the whole site. Just log and continue.
				ex.Log();
			}
		}
	}
}
