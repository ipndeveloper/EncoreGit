using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NetSteps.AccountLeadService.Common;
using NetSteps.Auth.UI.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Business.Helpers;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.SocialNetworking;
using nsDistributor.Extensions;
using nsDistributor.Models.Shared;

namespace nsDistributor.Controllers
{
	using NetSteps.Data.Entities.Generated;
	using NetSteps.Web.Mvc.Controls.Controllers;
	using NetSteps.Commissions.Common;
    using NetSteps.Data.Entities.Business.Logic;

	public abstract class BaseController : NetSteps.Web.Mvc.Business.Controllers.BaseController
    {
        private ICommissionsService _comSvc;
        protected ICommissionsService CommissionsService
        {
            get
            {
                if (_comSvc == null)
                {
                    _comSvc = Create.New<ICommissionsService>();
                }
                return _comSvc;
            }
        }

        #region
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

        #endregion

        #region Properties

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

		public static bool IsLoggedIn
		{
			get { return CoreContext.CurrentAccount != null && !CoreContext.CurrentAccount.IsTempAccount; }
		}

        public static bool TieneKitInicio
        {
            get 
            {
                if (CoreContext.CurrentAccount != null)
                {
                    Tuple<int, int, int> result = new OrderBusinessLogic().CheckOrdersByAccountID(CoreContext.CurrentAccount.AccountID);
                    return Convert.ToBoolean(result.Item3);
                }
                else return false;
                
            }
        }

		public static bool OwnerLoggedIn
		{
			get
			{
				if (IsLoggedIn)
				{
					//Check if the account id of the currently logged in person is the same as the account tied to the site
					return IsLoggedIn && SiteOwner != null && CoreContext.CurrentAccount.AccountID == SiteOwner.AccountID;
				}
				return false;
			}
		}

		public static CultureInfo CurrentCulture
		{
			get
			{
				try
				{
					//Try to create a C# culture from the language and country that the user has selected
					if (CurrentLanguageID > 0)
					{
						Language currentLanguage = SmallCollectionCache.Instance.Languages.GetById(CurrentLanguageID);
						if (!string.IsNullOrEmpty(currentLanguage.CultureInfo))
						{
							return CultureInfo.CreateSpecificCulture(currentLanguage.CultureInfo);
						}
					}
				}
				catch { }
				try
				{
					//Try to create a culture from the user's browser settings
					if (System.Web.HttpContext.Current.Request.UserLanguages.Length > 0)
						return CultureInfo.CreateSpecificCulture(System.Web.HttpContext.Current.Request.UserLanguages[0].ToLowerInvariant().Trim());
					return null;
				}
				catch (ArgumentException)
				{
					return null;
				}
			}
		}

		public static Country DefaultCountry
		{
			get
			{
				if (System.Web.HttpContext.Current.Session["DefaultCountry"] == null)
				{
					if (SiteOwner != null && SiteOwner.Addresses.Any(a => a.AddressTypeID == (int)Constants.AddressType.Main))
					{
						System.Web.HttpContext.Current.Session["DefaultCountry"] = Country.LoadFull(SiteOwner.Addresses.GetDefaultByTypeID(Constants.AddressType.Main).CountryID);
					}
					else
					{
						var country = Market.LoadFull(CoreContext.CurrentMarketId).Countries.FirstOrDefault() ?? Country.LoadFull((int)Constants.Country.UnitedStates);
						System.Web.HttpContext.Current.Session["DefaultCountry"] = country;
					}
				}
				return System.Web.HttpContext.Current.Session["DefaultCountry"] as Country;
			}
		}

		public static Constants.ViewingMode PageMode
		{
			get
			{
				//if (!OwnerLoggedIn)
				//    return Constants.ViewingMode.Production;
				//Check to make sure that we can only change the page mode on the base site
				if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["PageMode"]) && CurrentSite.IsBase && CoreContext.CurrentUser != null && CoreContext.CurrentUser is CorporateUser)
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

				if (System.Web.HttpContext.Current.Session.IsNotNull() && System.Web.HttpContext.Current.Session["PageMode"] != null)
				{
					return (Constants.ViewingMode)System.Web.HttpContext.Current.Session["PageMode"];
				}

				return Constants.ViewingMode.Production;
			}
		}

		public static NetSteps.Common.SSL.AwsSslPolicy SslPolicy = new NetSteps.Common.SSL.AwsSslPolicy();

		public bool AccountLocatorLocationSearchUsed
		{
			get
			{
				var accountLocatorLocationSearchUsed = System.Web.HttpContext.Current.Session["AccountLocatorLocationSearchUsed"];

				return accountLocatorLocationSearchUsed != null
					&& (bool)accountLocatorLocationSearchUsed;
			}
			set
			{
				System.Web.HttpContext.Current.Session["AccountLocatorLocationSearchUsed"] = value;
			}
		}

		//public static bool GenerateNewLead
		//{
		//    get
		//    {
		//        return System.Web.HttpContext.Current.Session["GenerateNewLead"] != null
		//               && (bool)System.Web.HttpContext.Current.Session["GenerateNewLead"];
		//    }
		//    set
		//    {
		//        System.Web.HttpContext.Current.Session["GenerateNewLead"] = value;
		//    }
		//}

		#endregion

    
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}

			if (WebHelpers.BaseControllerOverrideActionFilter != null)
			{
				WebHelpers.BaseControllerOverrideActionFilter.CurrentSite = CurrentSite;
				WebHelpers.BaseControllerOverrideActionFilter.BaseController = this;
				WebHelpers.BaseControllerOverrideActionFilter.OnActionExecuted(filterContext);
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

			if (PageMode == NetSteps.Common.Constants.ViewingMode.Preview)
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

			var siteUrl = CurrentSiteUrl.EldDecode();

			// replace "www.designer.domain.com" url with "designer.domain.com"
			if (!SiteUrl.Exists(siteUrl) && siteUrl.Contains("www"))
			{
				var subDomain = siteUrl.GetURLSubdomain().ToCleanString().ToLower();

				if (subDomain.Contains("www"))
				{
					siteUrl = siteUrl.Replace("www.", string.Empty);
					if (SiteUrl.Exists(siteUrl))
					{
						filterContext.Result = new RedirectResult(siteUrl);
						return;
					}
				}
			}

			var siteId = CurrentSite != null ? CurrentSite.SiteID : -1;
			var page = Page.Repository.Where(u =>
				u.SiteID == siteId &&
				u.Url == Request.Url.AbsolutePath);

			if (page != null && page.Count() == 1 && !page.Any(a => a.Active))
			{
				filterContext.Result = RedirectToAction("Http404", "Error");
			}

			if (ConfigurationManager.ForceSSL && SslPolicy.IsHttps(Request) == NetSteps.Common.Constants.IsHttpsReturnStatus.IsNotHttps)
			{
				string secureURL = Request.Url.AbsoluteUri.ToHttps();

				filterContext.Result = new RedirectResult(secureURL);
				return;
			}

			if (WebHelpers.BaseControllerOverrideActionFilter != null)
			{
				WebHelpers.BaseControllerOverrideActionFilter.CurrentSite = CurrentSite;
				WebHelpers.BaseControllerOverrideActionFilter.BaseController = this;
				WebHelpers.BaseControllerOverrideActionFilter.OnActionExecuting(filterContext);
			}

			//Check to make sure we're not trying to route a static file, because that means it's a 404
			if (Regex.IsMatch(Request.Url.AbsoluteUri, @"^[^\?]+\.\w+$"))
			{
				filterContext.Result = new HttpNotFoundResult();
				return;
			}

			// Don't service requests for domains we don't have - JHE
			if (!SiteUrl.Exists(siteUrl))
			{
				// check for CWS
				var cws = Site.FindCorporateSite(CoreContext.CurrentMarketId);
				if (cws != null && cws.PrimaryUrl != null)
				{
					filterContext.Result = new RedirectResult(cws.PrimaryUrl.Url.EldEncode());
					return;
				}

				Session.RemoveAll();
				if (filterContext.ActionDescriptor.ActionName != "SiteNotFound")
				{
					filterContext.Result = RedirectToAction("SiteNotFound", "Error");
				}
				return;
			}

			bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
			var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
			var distributor = !isSubdomain && !string.IsNullOrEmpty(localPath) && Regex.IsMatch(localPath, @"^/\w+") ? localPath.Substring(1, localPath.IndexOf('/', 1) > 0 ? localPath.IndexOf('/', 1) : localPath.Length - 1) : "";

			//Redirect to the base site if no vanity name was supplied - DES
			if (!isSubdomain && string.IsNullOrEmpty(distributor))
			{
				var url = CurrentSite.SiteUrls.FirstOrDefault(su => Regex.IsMatch(su.Url, @"/\w+$") && su.IsPrimaryUrl);
				if (url != null)
				{
					distributor = url.Url.Substring(url.Url.LastIndexOf("/"));
				}

				if (string.IsNullOrEmpty(distributor))
				{
					filterContext.Result = new HttpNotFoundResult();
					return;
				}
			}

			if (Request.QueryString.AllKeys.Contains("comingFromAccountLocator") && Request.QueryString["comingFromAccountLocator"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				var consultantsAccountId = CurrentSite.AccountID;
				this.SetAccountLocatorResultsUsedCookie(consultantsAccountId.HasValue ? consultantsAccountId.Value : 0);
			}

			if (Request.Url.AbsolutePath == "/" + (isSubdomain ? "" : distributor))
			{
				filterContext.Result = new RedirectResult("~" + (string.IsNullOrEmpty(distributor) ? "" : "/" + distributor) + "/Home");//Redirect("~/Home");
				return;
			}
			string attemptedUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + distributor;
			//If there is no site at that specific url
			if (!SiteUrl.Exists(attemptedUrl.EldDecode()))
			{
				if (filterContext.ActionDescriptor.ActionName != "SiteNotFound")
				{
					Session.RemoveAll();
					filterContext.Result = RedirectToAction("SiteNotFound", "Error");
				}
				return;
			}
			if (CurrentSite.SiteStatusID != (int)Constants.SiteStatus.Active && filterContext.ActionDescriptor.ActionName != "SiteNotActive")
			{
				Session.RemoveAll();
				filterContext.Result = RedirectToAction("SiteNotActive", "Error");
				return;
			}

			if (SiteOwner != null && !SmallCollectionCache.Instance.AccountStatuses.GetById(SiteOwner.AccountStatusID).ReportAsActive && filterContext.ActionDescriptor.ActionName != "SiteNotActive")
			{
				Session.RemoveAll();
				filterContext.Result = RedirectToAction("SiteNotActive", "Error");
				return;
			}

			if (IsLoggedIn)
			{
				ViewBag.HasParties = Party.HasHostedParties(CoreContext.CurrentAccount.AccountID);
			}

			ViewBag.IsPartyOrderClient = OrdersSection.Instance.IsPartyOrderClient;

			base.OnActionExecuting(filterContext);
		}

		protected override void SetViewData()
		{
			try
			{
				var currentSite = CurrentSite;

				if (ControllerContext.Controller is ErrorController)
				{
					return;
				}
				if (CurrentSite.Settings["Skin"] == null || CurrentSite.Settings["Skin"].Value == null)
				{
					ViewBag.Skin = "";
				}
				else
				{
					ViewBag.Skin = currentSite.Settings["Skin"] == null || currentSite.Settings["Skin"].Value == null ? "" : ("~/Content/Styles/Skin-" + currentSite.Settings["Skin"].Value.ToString() + ".css").ResolveUrl();
				}

				ViewBag.Logo = CurrentSite.GetHtmlSectionByName("Logo");
				ViewBag.Header = CurrentSite.GetHtmlSectionByName("Header");
				ViewBag.Footer = CurrentSite.GetHtmlSectionByName("Footer");
				ViewBag.GoogleAnalyticTrackerID = CurrentSite.Settings["GoogleAnalyticsTrackerID"] != null ? CurrentSite.Settings["GoogleAnalyticsTrackerID"].Value : string.Empty;
				ViewBag.MyPhoto = CurrentSite.GetHtmlSectionByName("MyPhoto");
				ViewBag.SiteDesignContent = CurrentSite.GetHtmlSectionByName("SiteDesignContent");
				ViewBag.LoadLastPendingOrderOnLogin = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.LoadLastPendingOrderOnLogin, false);

				FacebookAppInfo facebookAppInfo = new FacebookAppInfo()
				{
					AppId = CurrentSite.Settings["FacebookAppId"] != null ? CurrentSite.Settings["FacebookAppId"].Value : string.Empty,
					AppSecret = CurrentSite.Settings["FacebookAppSecret"] != null ? CurrentSite.Settings["FacebookAppSecret"].Value : string.Empty,
					ChannelUrl = CurrentSite.Settings["FacebookChannelUrl"] != null ? CurrentSite.Settings["FacebookChannelUrl"].Value : string.Empty
				};
				ViewBag.FacebookAppInfo = facebookAppInfo;

				if (SiteOwner != null)
				{
					var contactInfo = SiteOwner.AccountPublicContactInfo ?? new AccountPublicContactInfo() { HideName = false, HideEmailAddress = true, HideAddress = true, HidePhoneNumber = true };
					ViewBag.AccountName = contactInfo.HideName ? string.Empty : string.IsNullOrWhiteSpace(contactInfo.Name) ? SiteOwner.FullName : contactInfo.Name;
					ViewBag.AccountEmail = contactInfo.HideEmailAddress ? string.Empty : string.IsNullOrWhiteSpace(contactInfo.EmailAddress) ? SiteOwner.EmailAddress : contactInfo.EmailAddress;
					ViewBag.AccountPhone = contactInfo.HidePhoneNumber ? string.Empty : string.IsNullOrWhiteSpace(contactInfo.PhoneNumber) ? SiteOwner.MainPhone : contactInfo.PhoneNumber;
					ViewBag.AccountTitle = string.Empty;

					bool usesComissions = ApplicationContext.Instance.UsesEncoreCommissions;

					ViewBag.AccountTitle = String.Empty;

					if (usesComissions && !contactInfo.HideTitle)
					{

						var commissionsService = Create.New<ICommissionsService>();
						var curPeriod = commissionsService.GetCurrentPeriod();
						if (curPeriod != null)
						{
							var accountTitle = commissionsService.GetAccountTitle(SiteOwner.AccountID, 2, curPeriod.PeriodId);
							if (accountTitle != null)
							{
								var title = commissionsService.GetTitle(accountTitle.TitleId);
								if (title != null)
								{
									ViewBag.AccountTitle = Translation.GetTerm(title.TermName);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Session["LastException"] = ex;
			}
		}

		public virtual ActionResult CheckOwnerAuthentication()
		{
			if (OwnerLoggedIn)
				return null;
			Session["ReturnUrl"] = Request.Url == null ? "~/Home".ResolveUrl() : Request.Url.AbsolutePath;
			return RedirectToAction("Login", "Static");
		}

		/// <summary>
		/// Adds an error message to the TempData dictionary to be displayed on the next request.
		/// </summary>
		protected void AddErrorToTempData(string message) { this.AddMessageToTempData(new BasicResponse { Success = false, Message = message }); }

		/// <summary>
		/// Adds an error message to the ViewData dictionary to be displayed on the current view.
		/// </summary>
		protected void AddErrorToViewData(string message) { this.AddMessageToViewData(new BasicResponse { Success = false, Message = message }); }

		/// <summary>
		/// Adds all the error messages in the ModelStateDictionary seperated by newlines to the ViewData dictionary to be displayed on the current view.
		/// </summary>
		protected void AddModelStateErrorsToViewData()
		{
			AddErrorToViewData(ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).Aggregate((x, y) => x + Environment.NewLine + y));
		}

		/// <summary>
		/// Renders all of the current page's HtmlSections into ViewData.
		/// </summary>
		protected virtual void LoadPageHtmlContent()
		{
			var page = GetCurrentPage();
			if (page == null)
			{
				return;
			}
			foreach (var htmlSection in page.HtmlSections)
			{
				ViewData[htmlSection.SectionName] = htmlSection.ToDisplay();
			}
		}

		protected virtual Page GetCurrentPage()
		{
			return CurrentSite.GetPageByUrl(Request.Path);
		}

		protected virtual IAuthenticationUIService GetAuthUIService()
		{
			return Create.New<IAuthenticationUIService>();
		}

		protected virtual void SetAccountLocatorLocationSearchUsedFlag(AccountLocatorModel model)
		{
			AccountLocatorLocationSearchUsed = model.SearchType == AccountLocatorModel.AccountLocatorSearchType.Location;
		}

		/// <summary>
		/// Updates the lead count for the consultant by the amount specified
		/// </summary>
		/// <param name="consultantId">The consultant Id to update the lead count for</param>
		/// <param name="amount">Amount to add to the consultant's lead count</param>
		protected virtual void IncrementConsultantsLeadCount(int consultantId, int amount = 1)
		{
			if (consultantId <= 0 || !this.CanGenerateNewLeadForConsultant(consultantId))
			{
				return;
			}

			var accountLeadService = Create.New<IAccountLeadService>();
			accountLeadService.IncrementLeadCount(consultantId, amount);

			this.ExpireAccountLocatorResultsUsedCookie();
		}

		protected virtual void ExpireAccountLocatorResultsUsedCookie()
		{
			var cookies = HttpContext.Request.Cookies;
			var accountLocatorSearchResultsUsedCookie = cookies["NetSteps_AccountLocatorSearchResultsUsed"];

			if (accountLocatorSearchResultsUsedCookie != null)
			{
				accountLocatorSearchResultsUsedCookie.Expires = DateTime.UtcNow.AddDays(-2);
				Response.Cookies.Add(accountLocatorSearchResultsUsedCookie);
			}
		}

		protected virtual void SetAccountLocatorResultsUsedCookie(int locatedAccountId)
		{
			var cookies = HttpContext.Request.Cookies;
			var accountLocatorSearchResultsUsedCookie = cookies["NetSteps_AccountLocatorSearchResultsUsed"]
						 ?? new HttpCookie("NetSteps_AccountLocatorSearchResultsUsed");

			accountLocatorSearchResultsUsedCookie.Value = locatedAccountId.ToString(CultureInfo.InvariantCulture);
			accountLocatorSearchResultsUsedCookie.Expires = DateTime.UtcNow.AddYears(5);

			HttpContext.Response.Cookies.Add(accountLocatorSearchResultsUsedCookie);
		}

		protected virtual bool CanGenerateNewLeadForConsultant(int accountId)
		{
			var cookies = HttpContext.Request.Cookies;
			var cookie = cookies["NetSteps_AccountLocatorSearchResultsUsed"];

			return cookie != null
				&& cookie.Expires > DateTime.UtcNow
				&& cookie.Value == accountId.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Returns the CurrentSite property, but throws an error if CurrentSite is null
		/// </summary>
		/// <returns>CurrentSite property</returns>
		public virtual Site GetCurrentSiteErrorOnNull()
		{
			var currentSite = CurrentSite;

			if (currentSite == null)
			{
				throw new Exception("Could not find the current PWS.");
			}

			return currentSite;
		}

		public Account Account
		{
			get
			{
				return IsLoggedIn ? CoreContext.CurrentAccount : new Account()
				{
					AccountTypeID = (int)ConstantsGenerated.AccountType.RetailCustomer,
					SponsorID = 0
				};
			}
		}

		#region Strings
		protected virtual string _errorSessionTimedOut { get { return Translation.GetTerm("SessionTimedOut", "Your session has timed out."); } }
		#endregion
		#region Private Methods
		private static int? GetBrowserLanguage(TrackableCollection<Language> languages)
		{
			int? languageID = null;
			if (System.Web.HttpContext.Current.Request.UserLanguages != null && System.Web.HttpContext.Current.Request.UserLanguages.Length > 0)
			{
				foreach (var userLanguage in System.Web.HttpContext.Current.Request.UserLanguages)
				{
					try
					{
						var cultureInfo = userLanguage.IndexOf(';') < 0 ? userLanguage : userLanguage.Substring(0, userLanguage.IndexOf(';'));
						foreach (var language in languages.Where(language => language.CultureInfo.Equals(cultureInfo, StringComparison.InvariantCultureIgnoreCase)))
						{
							languageID = language.LanguageID;
							break;
						}
					}
					catch { }
				}
			}
			return languageID;
		}
		#endregion
	}
}
