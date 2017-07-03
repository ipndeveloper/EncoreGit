using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Auth.UI.Common;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using DistributorBackOffice.Models;

namespace DistributorBackOffice.Controllers
{
	/// <summary>
	/// The base controller.
	/// </summary>
	public abstract class BaseController : NetSteps.Web.Mvc.Business.Controllers.BaseController
	{
		/// <summary>
		/// The ssl policy.
		/// </summary>
		public static NetSteps.Common.SSL.AwsSslPolicy SslPolicy = new NetSteps.Common.SSL.AwsSslPolicy();

		private int _currentAccountId;
		public int CurrentAccountId
		{
			get
			{
				if (_currentAccountId == 0)
				{
					_currentAccountId = CoreContext.GetCurrentAccountId();
				}
				return _currentAccountId;
			}
		}

		private Account _currentAccount;
		public Account CurrentAccount
		{
			get
			{
				if (_currentAccount == null
					|| _currentAccount.AccountID != CurrentAccountId)
				{
					_currentAccount = GetCurrentAccount();
				}
				return _currentAccount;
			}
		}
		private Account GetCurrentAccount()
		{
			var accountId = CurrentAccountId;
			if (accountId > 0)
			{
				return Account.LoadFull(CurrentAccountId);
			}
			return null;
		}

		/// <summary>
		/// Clears the CurrentAccount variable which will trigger a reload
		/// from the database the next time CurrentAccount is accessed.
		/// </summary>
		public void ReloadCurrentAccount()
		{
			_currentAccount = null;
		}

		private Site _site;
		public Site CurrentSite
		{
			get
			{
				if (_site == null)
				{
					_site = GetCurrentSite();
				}
				return _site;
			}
		}

		private List<Archive> _archives;
		public List<Archive> Archives
		{
			get
			{
				if (_archives == null)
				{
					_archives = GetArchives();
				}
				return _archives;
			}
		}

		public static List<Archive> GetArchives()
		{
			var currentSite = GetCurrentSite();
			if (currentSite.IsBase)
			{
				return currentSite.Archives.ToList();
			}
			return SiteCache.GetSiteByID(currentSite.BaseSiteID.Value).Archives.ToList();
		}

		private Page _currentPage = null;
		public Page CurrentPage
		{
			get
			{
				if (_currentPage == null)
				{
					_currentPage = GetCurrentPage();
				}
				return _currentPage;
			}
		}

		public static Page GetCurrentPage()
		{
			Site currentSite = GetCurrentSite();
			if (currentSite != null)
			{
				string path = WebContext.PageUrl;
				if (path == "/")
					path = "/Home";
				return currentSite.GetPageByUrl(path);
			}
			return null;
		}

		public static Constants.ViewingMode GetPageMode()
		{
			Site currentSite = GetCurrentSite();
			HttpContext currentHttpContext = System.Web.HttpContext.Current;

			if (currentSite != null && currentHttpContext != null && currentHttpContext.Session != null)
			{
				//if (!OwnerLoggedIn)
				//    return Constants.ViewingMode.Production;
				//Check to make sure that we can only change the page mode on the base site
				//if (!OwnerLoggedIn)
				//    return Constants.ViewingMode.Production;
				//Check to make sure that we can only change the page mode on the base site
				if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["PageMode"]) && currentSite.IsBase)
				{
					NetSteps.Common.Constants.ViewingMode pageMode = (Constants.ViewingMode)Enum.Parse(typeof(Constants.ViewingMode), System.Web.HttpContext.Current.Request.Params["PageMode"].ToPascalCase());
					if (System.Web.HttpContext.Current.Session["PageMode"] == null || pageMode != (Constants.ViewingMode)System.Web.HttpContext.Current.Session["PageMode"])
					{
						System.Web.HttpContext.Current.Session["PreviousPageMode"] = System.Web.HttpContext.Current.Session["PageMode"];
						System.Web.HttpContext.Current.Session["PageMode"] = pageMode;
						if (pageMode == Constants.ViewingMode.Production)
							NetSteps.Data.Entities.ApplicationContext.Instance.SetDateTimeNow(null);
					}
					return pageMode;
				}
				else if (System.Web.HttpContext.Current.Session["PageMode"] != null)
				{
					return (Constants.ViewingMode)System.Web.HttpContext.Current.Session["PageMode"];
				}
			}

			return Constants.ViewingMode.Production;
		}

		public MailAccount CurrentMailAccount
		{
			get
			{
				using (var currentMailAccountTrace = this.TraceActivity("CurrentMailAccount"))
				{
					MailAccount mailAccount = (MailAccount)System.Web.HttpContext.Current.Session["CurrentMailAccount"];
					if (mailAccount == null)
					{
						this.TraceInformation("mailAccount was null, checking accounts");
						Account acct = CurrentAccount;
						if (acct != null)
						{
							this.TraceInformation(string.Format("getting MailAccounts for {0}", acct.AccountID));
							if (acct.MailAccounts.Count == 0)
							{
								string errorMessage = Translation.GetTerm("ThereArentAnyMailAccountsForThisAccount", "There aren't any MailAccounts for this Account");
								this.TraceError(errorMessage);
								throw new Exception(errorMessage);
							}

							System.Web.HttpContext.Current.Session["CurrentMailAccount"] = mailAccount = acct.MailAccounts.Where(a => a.Active).FirstOrDefault();
							this.TraceInformation(string.Format("got MailAccount {0}", mailAccount.MailAccountID));
						}
						else
						{
							this.TraceInformation("CurrentAccount was null");
						}

						if (mailAccount == null)
						{
							this.TraceError("CurrentMail account is null");
							throw new NullReferenceException("CurrentMailAccount is null");
						}
					}
					return mailAccount;
				}
			}
			set { System.Web.HttpContext.Current.Session["CurrentMailAccount"] = value; }
		}

		public static string GetCurrentSiteUrl()
		{
			HttpRequest request = System.Web.HttpContext.Current.Request;
			string currentSiteUrl = "http://" +
									request.Url.Authority.Replace(":81", "") +
									request.ApplicationPath;
			return currentSiteUrl;
		}

		public static Site GetCurrentSite()
		{
			string currentSiteUrl = GetCurrentSiteUrl();
			return SiteCache.GetSiteByUrl(currentSiteUrl.EldDecode());
		}
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}

			if (filterContext.Result is ViewResult)
			{
				//If the result is a ViewResult, we need to set the view data that is used by the master page
				SetViewData();
			}
			base.OnActionExecuted(filterContext);
		}

		protected override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}

			if (GetPageMode() == NetSteps.Common.Constants.ViewingMode.Preview)
			{
				Session["PageMode"] = Session["PreviousPageMode"];
				Session["PreviewContent"] = null;
			}
			base.OnResultExecuted(filterContext);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}

			if (ConfigurationManager.ForceSSL && SslPolicy.IsHttps(Request) == NetSteps.Common.Constants.IsHttpsReturnStatus.IsNotHttps)
			{
				string secureURL = Request.Url.AbsoluteUri.ToHttps();

				filterContext.Result = new RedirectResult(secureURL);
				return;
			}

			string allowedRoutes = ConfigurationManager.AppSettings["AllowedRoutes"];
			bool logAttemptsAtLockedRoutes;
			bool lockOutAttemptsAtLockedRoutes;
			bool.TryParse(ConfigurationManager.AppSettings["LogAttemptsAtLockedRoutes"], out logAttemptsAtLockedRoutes);
			bool.TryParse(ConfigurationManager.AppSettings["LockOutAttemptsAtLockedRoutes"], out lockOutAttemptsAtLockedRoutes);

			var currentAction = filterContext.RouteData.Values["action"].ToString();

			if (!string.IsNullOrEmpty(allowedRoutes) && !this.AllowRoute(filterContext.RouteData.Values, allowedRoutes.Split(';')))
			{
				if (logAttemptsAtLockedRoutes)
				{
					var requestedUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
					var referrer = filterContext.HttpContext.Request.UrlReferrer;
					var referringUrl = referrer == null ? null : referrer.AbsolutePath;
					this.LogAttemptAtLockedPage(requestedUrl, referringUrl);
				}

				if (lockOutAttemptsAtLockedRoutes)
				{
					string controllerToRedirectTo = ConfigurationManager.AppSettings["ControllerForFailedRoutes"];
					string actionToRedirectTo = ConfigurationManager.AppSettings["ActionForFailedRoutes"];
					string areaToRedirectTo = ConfigurationManager.AppSettings["AreaForFailedRoutes"];

					filterContext.Result = RedirectToRoute(new { area = areaToRedirectTo, controller = controllerToRedirectTo, action = actionToRedirectTo, returnUrl = string.Empty });
					return;
				}
			}

			bool customNonAuthenticatedAction = filterContext.RouteData.Values["CustomNonAuthenticatedAction"] != null
				 && bool.Parse(filterContext.RouteData.Values["CustomNonAuthenticatedAction"].ToString());

			if (CurrentAccount == null && !customNonAuthenticatedAction
				&& currentAction != "Login"
				&& currentAction != "Optout"
				&& currentAction != "ReloadInventory"
				&& filterContext.HttpContext.Request.Url.LocalPath != "/favicon.ico"
				&& currentAction != "ForgotPassword"
					 && currentAction != "ResetPassword")
			{
				if (Request.IsAjaxRequest())
				{
					filterContext.Result =
						 this.Json(
							  new
									{
										result = false,
										message =
											 Translation.GetTerm(
												  "YourSessionHasTimedOutPleaseRefreshthePage",
												  "Your session has timed out.  Please refresh the page.")
									});
				}
				else
				{

					filterContext.Result = RedirectToRoute(new { controller = "Security", action = "Login", area = "", returnUrl = TempData["AttemptedPage"] ?? filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery });
				}

				return;
			}

			var ownedSites = Site.LoadByAccountID(CurrentAccountId);
			if (ownedSites != null && ownedSites.Count > 0)
			{
				var defaultSite = ownedSites.FirstOrDefault(s => s.SiteStatusID == (int)Constants.SiteStatus.Active && s.SiteUrls.Count > 0);
				if (defaultSite != null)
				{
					//Get the localhost url if we are on dev boxes - DES
					var defaultUrl =
						 Request.Url.Authority.Contains("localhost")
						 && defaultSite.SiteUrls.Any(su => su.Url.Contains("localhost"))
												? defaultSite.SiteUrls.FirstOrDefault(su => su.Url.Contains("localhost"))
												: defaultSite.SiteUrls.FirstOrDefault(su => su.IsPrimaryUrl);
					if (defaultUrl == null && defaultSite.SiteUrls.Count > 0)
					{
						defaultUrl = defaultSite.SiteUrls.FirstOrDefault();
					}
					if (defaultUrl != null && CurrentAccount.IsNotNull())
					{
						ViewBag.SiteUrl = string.Format("{0}Login?token={1}&returnUrl=%2FAdmin",
																		defaultUrl.Url.AppendForwardSlash().ConvertToSecureUrl(ConfigurationManager.ForceSSL),
													Account.GetSingleSignOnToken(CurrentAccount.AccountID)).EldEncode();
					}
				}
			}

			base.OnActionExecuting(filterContext);
		}

		protected override void SetViewData()
		{
			try
			{
				ViewData["CurrentSite"] = CurrentSite;
				ViewData["CurrentPage"] = CurrentPage;

				ViewData["Logo"] = CurrentSite.GetHtmlSectionByName("Logo");
				//ViewData["Header"] = CurrentSite.GetHtmlSectionByName("Header");
				//ViewData["Footer"] = CurrentSite.GetHtmlSectionByName("Footer");
				ViewBag.GoogleAnalyticTrackerID = CurrentSite.Settings["GoogleAnalyticsTrackerID"] != null ? CurrentSite.Settings["GoogleAnalyticsTrackerID"].Value : string.Empty;
				//ViewData["SiteDesignContent"] = CurrentSite.GetHtmlSectionByName("SiteDesignContent");
				ViewBag.SiteDesignContent = CurrentSite.GetHtmlSectionByName("SiteDesignContent");
			}
			catch (Exception)
			{
				//Session["LastException"] = ex;
			}
		}

		protected virtual void LogAttemptAtLockedPage(string url, string referringUrl)
		{
			Create.New<NetSteps.Common.Interfaces.ILogger>().AttemptAtBlockedPage(url, referringUrl);
		}

		private bool AllowRoute(System.Web.Routing.RouteValueDictionary routeValues, string[] routes)
		{
			foreach (string route in routes)
			{
				string controller = route.Split('/')[0];
				string action = route.Split('/')[1];

				if (routeValues["controller"].ToString().ToLower() == controller.ToLower()
					 && routeValues["action"].ToString().ToLower() == action.ToLower())
				{
					return true;
				}
			}

			return false;
		}

		protected virtual IAuthenticationUIService GetAuthUIService()
		{
			return Create.New<IAuthenticationUIService>();
		}
	}
}
