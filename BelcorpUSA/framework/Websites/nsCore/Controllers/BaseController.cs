using System.Linq;
using System.Web.Mvc;
using NetSteps.Auth.UI.Common;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace nsCore.Controllers
{
	// All GMP controllers default to "DontCache".
	// You can place an OutputCache attribute on derived controllers or actions to override this behavior.
	[OutputCache(CacheProfile = "DontCache")]
	public abstract class BaseController : NetSteps.Web.Mvc.Business.Controllers.BaseController
	{
		public static NetSteps.Common.SSL.AwsSslPolicy SslPolicy = new NetSteps.Common.SSL.AwsSslPolicy();

		/// <summary>
		/// Business logic provider.
		/// </summary>
		public IDomainServicesFactory DomainServicesFactory { get; private set; }

		/// <summary>
		/// Data provider.
		/// </summary>
		public IDomainServicesFactory DataAdapterFactory { get; private set; }
		
		/// <summary>
		/// Adds CountryUS, CurrentUser, CurrentAccount to the ViewBag.
		/// </summary>
		protected override void SetViewData()
		{
			base.SetViewData();

			ViewBag.CountryUS = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.CountryCode.ToUpper() == "US");
			ViewBag.CurrentUser = CoreContext.CurrentUser;
			ViewBag.CurrentAccount = CurrentAccount;
		}

		protected virtual Account CurrentAccount
		{
			get { return Session["CurrentAccount"] as Account; }
			set { Session["CurrentAccount"] = value; }
		}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var cultura = Thread.CurrentThread.CurrentCulture.Name;
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Request.InputStream.Position = 0;
            using (var reader = new StreamReader(filterContext.HttpContext.Request.InputStream))
            {
                string json = reader.ReadToEnd();
            }

            var keys = filterContext.RequestContext.HttpContext.Request.Headers.AllKeys;

            // Get the request key value pairs
            var requestVars = keys.Select(key => new KeyValuePair<string, string>(key, filterContext.RequestContext.HttpContext.Request.Headers.Get(key)));
        }


        //private static void LogRequestVars(string methodName, ActionExecutingContext values)
        //{
        //    // Get the request keys
        //    var keys = values.RequestContext.HttpContext.Request.Headers.AllKeys;

        //    // Get the request key value pairs
        //    var requestVars = keys.Select(key => new KeyValuePair<string, string>(key, values.RequestContext.HttpContext.Request.Headers.Get(key)));

        //    // Write to Debug log
        //    Debug.WriteLine("Method: {0}", methodName);

        //    foreach (var keyValuePair in requestVars)
        //    {
        //        Debug.WriteLine("{0} - {1}", keyValuePair.Key, keyValuePair.Value);
        //    }
        //}
   
		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}

			var reqUri = Request.Url;
			if (reqUri != null)
			{
				if (ConfigurationManager.ForceSSL && SslPolicy.IsHttps(Request) == NetSteps.Common.Constants.IsHttpsReturnStatus.IsNotHttps)
				{
					var secureUrl = reqUri.ToString().Replace("http:", "https:");
					filterContext.Result = new RedirectResult(secureUrl);
					return;
				}

				if (CoreContext.CurrentUser == null && filterContext.RouteData.Values["action"].ToString() != "Login" && filterContext.RouteData.Values["action"].ToString() != "ReloadInventory" && reqUri.LocalPath != "/favicon.ico")
				{
					if (Request.IsAjaxRequest())
						filterContext.Result = Json(new { result = false, message = Translation.GetTerm("YourSessionHasTimedOutPleaseRefreshthePage", "Your session has timed out.  Please refresh the page.") });
					else
						filterContext.Result = RedirectToRoute(new { controller = "Security", action = "Login", area = "", returnUrl = TempData["AttemptedPage"] ?? reqUri.PathAndQuery });
					return;
				}

				if (CoreContext.CurrentUser != null && CoreContext.CurrentUserMarkets.Count == 0 && filterContext.RouteData.Values["action"].ToString() != "Login")
				{
					TempData["NoMarkets"] = true;
					filterContext.Result = RedirectToRoute(new { controller = "Security", action = "Login", area = "" });
					return;
				}

                if (CoreContext.CurrentUser != null && filterContext.RouteData.Values["controller"].ToString() != "ManualBonusEntry")
                {
                    TempData["ManualBonus"] = null;
                }

				if (CoreContext.CurrentUser != null && CoreContext.CurrentUserMarkets.Any() && filterContext.RouteData.Values["action"].ToString() != "Login"
					&& CoreContext.CurrentUserMarkets.All(m => m.MarketID != CoreContext.CurrentMarketId))
				{
					CoreContext.CurrentMarketId = CoreContext.CurrentUserMarkets.First().MarketID;
				}
			}
			base.OnAuthorization(filterContext);
		}

		protected virtual IAuthenticationUIService GetAuthUIService()
		{
			return Create.New<IAuthenticationUIService>();
		}

        public static string CurrentSiteUrl
        {
            get
            {
                bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                var distributor = !isSubdomain && !string.IsNullOrEmpty(localPath) && Regex.IsMatch(localPath, @"^/\w+") ? localPath.Substring(1, localPath.IndexOf('/', 1) > 0 ? localPath.IndexOf('/', 1) : localPath.Length - 1) : "";
                string currentSiteUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath + distributor;
                return currentSiteUrl;
            }
        }

        public static Site CurrentSite
        {
            get
            {
                var site = SiteCache.GetSiteByUrl(CurrentSiteUrl.EldDecode());
                if (site == null)
                {
                    return null;
                }
                // Update Session SiteOwner var if applicable (not null for base site) - JHE
                int currentSiteOwnerAccountID = 0;

                if (System.Web.HttpContext.Current.Session["SiteOwner"] != null)
                {
                    currentSiteOwnerAccountID = (System.Web.HttpContext.Current.Session["SiteOwner"] as Account).AccountID;
                }

                if (site != null && site.AccountID != null && site.AccountID > 0 && (currentSiteOwnerAccountID == 0 || currentSiteOwnerAccountID != site.AccountID))
                {
                    System.Web.HttpContext.Current.Session["SiteOwner"] = Account.LoadFull(site.AccountID.Value);
                }

                // 2011-11-10, JWL, Added check for current market id to set session variable.
                if (System.Web.HttpContext.Current.Session["CurrentMarketID"] == null)
                {
                    System.Web.HttpContext.Current.Session["CurrentMarketID"] = site.MarketID;
                }
                return site;
            }
        }

        public static int CurrentLanguageID
        {
            get
            {
                return CoreContext.CurrentLanguageID;
            }
            set
            {
                CoreContext.CurrentLanguageID = value;
            }
        }

        public static Account SiteOwner
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["SiteOwner"] == null && CurrentSite.AccountID.HasValue && CurrentSite.AccountID > 0)
                {
                    System.Web.HttpContext.Current.Session["SiteOwner"] = Account.LoadFull(CurrentSite.AccountID.Value);
                }
                return (Account)System.Web.HttpContext.Current.Session["SiteOwner"];
            }
            set { System.Web.HttpContext.Current.Session["SiteOwner"] = value; }
        }
    }
}
