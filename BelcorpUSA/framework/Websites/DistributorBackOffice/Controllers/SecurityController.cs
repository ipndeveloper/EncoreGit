using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NetSteps.Auth.UI.Common.Enumerations;
using System.Collections.Generic;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;

namespace DistributorBackOffice.Controllers
{
	[OutputCache(CacheProfile = "DontCache")]
	public class SecurityController : BaseController
	{
		[HttpGet]
		public virtual ActionResult Login(bool? autoLoggedOut, string returnUrl, string sso, string token = "")
		{

			if (!String.IsNullOrWhiteSpace(sso))
			{
				return DoSSOLogin(sso);
			}
			else if (!String.IsNullOrWhiteSpace(token))
			{
				return DoTokenLogin(token);
			}

			ViewData["SessionExpired"] = autoLoggedOut.HasValue ? autoLoggedOut.Value : false;

			if (String.IsNullOrWhiteSpace(returnUrl))
			{
				if (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultURL"))
				{
					string defaultUrl = ConfigurationManager.AppSettings["DefaultURL"];
					if (!String.IsNullOrWhiteSpace(defaultUrl))
						returnUrl = defaultUrl.ResolveUrl();
				}
			}
			TempData["ReturnUrl"] = String.IsNullOrWhiteSpace(returnUrl) || returnUrl.Contains("favicon.ico") ? "~/".ResolveUrl() : returnUrl;

			return View(GetLoginModel());
		}

		private ActionResult DoTokenLogin(string token)
		{
			try
			{
				Session.Clear();
				int accountID = Account.GetAccountIdFromSingleSignOnToken(token);
				if (accountID == 0)
				{
					ViewData["InvalidLogin"] = true;
					return View(GetLoginModel());
				}

				var account = Account.LoadForSession(accountID);// Account.LoadFull(accountID);

				if (account.AccountTypeID != (int)Constants.AccountType.Distributor)
					throw new NetSteps.Common.Exceptions.NetStepsException("Cannot login here")
					{
						PublicMessage = Translation.GetTerm("CannotLoginHere", "Cannot login here")
					};

				if (!SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).ReportAsActive)
				{
					throw new NetStepsBusinessException("Account is inactive.")
					{
						PublicMessage = Translation.GetTerm("AccountIsInactive", "Account is inactive."),
						IncludeErrorLogMessage = false
					};
				}

				CoreContext.SetCurrentAccountId(accountID);
                CoreContext.CurrentLanguageID = account.DefaultLanguageID;
				int? marketId = account.MarketID;
				if (marketId.HasValue)
					ApplicationContext.Instance.CurrentMarketID = marketId.Value;

				CreateMailAccountIfNull(account);

				if (account.User == null)
					throw new Exception("The specified Account does not have User credentials.");

				FormsAuthentication.SetAuthCookie(account.User.Username, false);

				Response.Cookies.Add(new HttpCookie("AccountID", "1")
				{
					Path = FormsAuthentication.FormsCookiePath
				});
				if (TempData["ReturnUrl"] == null)
				{
					Uri urlReferrer = Request.UrlReferrer;
					if (urlReferrer == null)
						return Redirect("~/".ResolveUrl());
					if (urlReferrer.AbsolutePath.ToLower().Contains("login"))
					{
						if (urlReferrer.Query.Contains("returnUrl"))
							return Redirect(Server.UrlDecode(urlReferrer.Query.Substring(urlReferrer.Query.IndexOf("returnUrl") + 10)));
						else
							return Redirect("~/".ResolveUrl());
					}
					return Redirect("~/".ResolveUrl());
				}
                
                var CurrentUser = GetAccountWithCredentails(accountID.ToString(), 1);
                CoreContext.CurrentUser = CurrentUser;

				return Redirect(TempData["ReturnUrl"].ToString());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				TempData["SSOError"] = exception.PublicMessage;
			}
			return Redirect("~/Home");
		}

		private ActionResult DoSSOLogin(string sso)
		{
			try
			{
				Session.Clear();
				var userId = CorporateUser.GetIdFromSingleSignOnToken(sso);
				if (userId > 0)
				{
					var user = CorporateUser.LoadFull(userId);
					CoreContext.CurrentUser = user;

					int corporateAccountID = NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting<int>(NetSteps.Common.Configuration.ConfigurationManager.VariableKey.CorporateAccountID);

					if (corporateAccountID == 0)
					{
						ViewData["InvalidLogin"] = true;
						return View(GetLoginModel());
					}

					var account = Account.LoadForSession(corporateAccountID);// Account.LoadFull(corporateAccountID);
					if (!SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).ReportAsActive)
					{
						throw new NetStepsBusinessException("Account is inactive.")
						{
							PublicMessage = Translation.GetTerm("AccountIsInactive", "Account is inactive."),
							IncludeErrorLogMessage = false
						};
					}
					CoreContext.SetCurrentAccountId(corporateAccountID);
                    CoreContext.CurrentLanguageID = account.DefaultLanguageID;
					int? marketId = account.MarketID;
					if (marketId.HasValue)
						ApplicationContext.Instance.CurrentMarketID = marketId.Value;

					CreateMailAccountIfNull(account);

					return Redirect("~?PageMode=Edit");
				}
				else
				{
					TempData["SSOError"] = Translation.GetTerm("SSOError", "There was an error with the single sign on.  Please try to login manually.");
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				TempData["SSOError"] = exception.PublicMessage;
				ViewData["InvalidLogin"] = true;
				return View(GetLoginModel());
			}

			return Redirect("~/Home");
		}

		[HttpPost]
		public virtual ActionResult Login(string username, string password)
		{

            //var CurrentLanguageID = CoreContext.CurrentLanguageID;
            //var Culture = CoreContext.CurrentCultureInfo;
			try
			{
				Session.Clear();

				var authResult = GetAuthUIService().Authenticate(username, password, CurrentSite.SiteID);
				if (!authResult.WasLoginSuccessful)
				{
					throw new NetStepsException(authResult.FailureMessage) { PublicMessage = authResult.FailureMessage };
				}

				var account = GetAccountWithCredentails(username, authResult.CredentialTypeID);

                if (!SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).ReportAsActive)
                {
                    throw new NetStepsBusinessException("Account is inactive.")
                    {
                        PublicMessage = Translation.GetTerm("AccountIsInactive", "Account is inactive."),
                        IncludeErrorLogMessage = false
                    };
                }

				if (account.AccountTypeID != (int)Constants.AccountType.Distributor)
					throw new NetSteps.Common.Exceptions.NetStepsException("Cannot login here")
					{
						PublicMessage = Translation.GetTerm("CannotLoginHere", "Cannot login here")
					};

                //begin-Validar Bloqueo de Consultora
                List<AccountBlocking> blockingSts = new List<AccountBlocking>();
                blockingSts = Account.AccountBlockingStatus(account.AccountID, "BLKSDWS");
                if (blockingSts.Count > 0)
                {
                    if (blockingSts[0].Status == true)
                    {
                        throw new NetStepsBusinessException(blockingSts[0].Description)
                        {
                            PublicMessage = blockingSts[0].Description,
                            IncludeErrorLogMessage = false
                        };

                    }
                }
                //end-Validar Bloqueo de Consultora

				CoreContext.CurrentUser = account;
				CoreContext.SetCurrentAccountId(account.AccountID);

                // comentado por HUMDRED 26042017 ==> Motivo hacer que el lenguaje se establesca en la configuración global del app global.asax
                CoreContext.CurrentLanguageID = account.DefaultLanguageID;
				int? marketId = account.MarketID;
				if (marketId.HasValue)
					ApplicationContext.Instance.CurrentMarketID = marketId.Value;
				FormsAuthentication.SetAuthCookie(username, false);


				Response.Cookies.Add(new HttpCookie("AccountID", "1")
				{
					Path = FormsAuthentication.FormsCookiePath
				});

				CreateMailAccountIfNull(account);

				if (TempData["ReturnUrl"] == null)
				{
					Uri urlReferrer = Request.UrlReferrer;
					if (urlReferrer == null)
						return Redirect("~/".ResolveUrl());
					if (urlReferrer.AbsolutePath.ToLower().Contains("login"))
					{
						if (urlReferrer.Query.Contains("returnUrl"))
							return Redirect(Server.UrlDecode(urlReferrer.Query.Substring(urlReferrer.Query.IndexOf("returnUrl") + 10)));
						else
							return Redirect("~/".ResolveUrl());
					}
					return Redirect(urlReferrer.PathAndQuery);
				}
				return Redirect(TempData["ReturnUrl"].ToString());
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				ViewData["InvalidLogin"] = true;

				if (WebContext.IsLocalHost)
					ViewData["ErrorMessage"] = ex.Message;
				return View(GetLoginModel());
			}
		}

		protected virtual Account GetAccountWithCredentails(string identifier, int credentialTypeID)
		{
			switch ((LoginCredentialTypes)credentialTypeID)
			{
				case LoginCredentialTypes.Username:
				case LoginCredentialTypes.CorporateUsername:
					var userID = Create.New<IUserRepository>().GetByUsername(identifier).UserID;
					return Account.LoadForSessionByUserID(userID);// Create.New<IAccountRepository>().LoadByUserIdFull(userID);
				case LoginCredentialTypes.Email:
					return Account.LoadForSession(identifier, false);//.LoadNonProspectByEmailFull(identifier);
				case LoginCredentialTypes.AccountId:
					return Account.LoadForSession(int.Parse(identifier));// Account.LoadFull(int.Parse(identifier));
				default:
					throw new NetStepsException(Translation.GetTerm("Login_UnableToLocateAcount", "Unable to locate account"));
			}
		}

		protected virtual ILoginModel GetLoginModel()
		{
			var model = Create.New<ILoginModel>();
			var authService = GetAuthUIService();
			var config = authService.GetConfiguration();
			model.EnableForgotPassword = config.EnableForgotPassword;
			model.LoginUrl = Url.Action("Login");

			string credentialType = ((LoginCredentialTypes)config.PrimaryCredentialType).ToString("G");
			model.UsernameErrorText = Translation.GetTerm("Login_UsernameIsRequired_" + credentialType, "{0} is required.", credentialType);
			model.UsernameLabelText = Translation.GetTerm("Login_UsernameLabelText_" + credentialType, credentialType);

			return model;
		}

        public virtual ActionResult ExternalToken(string section)
        {
            try
            {
               var account = CoreContext.CurrentAccount;
               string urlConectados = "http://conectados.";
               string sectionStr = "";
               string uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}?chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                // Get account proxy links.
                var proxyLinks = ProxyLink.GetAccountProxyLinks(account.AccountID, account.AccountTypeID);
                //Get AccountToken
               string token = proxyLinks[0].Url.Substring(proxyLinks[0].Url.IndexOf("token") + 6);
               string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
               Response.Cookies.Add(new HttpCookie("token", token));
               Response.Cookies.Add(new HttpCookie("return", url));

                if (section == "Home")
                {
                    Response.Redirect(uri);
                }
                else if (section =="TreinamentoProductos")
                {
                    sectionStr = "Treinamento?TipoTreinamento=produtos";
                //"http://conectados.belcorpbra.qas.draftbrasil.com/&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login=62351"
                    uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                    Response.Redirect(uri);
                }
                else if (section =="TreinamentoNegocio")
                {
                    sectionStr = "Treinamento?TipoTreinamento=Neg%C3%B3cios";
               //http://conectados.belcorpbra.qas.draftbrasil.com/&login=62351
                    uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                    Response.Redirect(uri);
                }
                else if (section == "FaleConozco")
                {
                    sectionStr = "Contato/FaleConosco";
                    //uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                    uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}", urlConectados, sectionStr, "62351");
                    Response.Redirect(uri);
                }
                else if (section == "FaleConozco")
                {
                    sectionStr = "Contato/FaleConosco";
                    uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                    Response.Redirect(uri);
                }
                else if (section == "PerguntaFrequente")
                {
                    sectionStr = "PerguntaFrequente";
                    uri = String.Format("{0}belcorpbra.qas.draftbrasil.com/{1}&chave=5a6e84dd-d56e-4e27-8581-2cf1bc9117af&login={2}", urlConectados, sectionStr, "62351");
                    Response.Redirect(uri);
                }

                
               

                return Json(new { result = true });
            }
            catch (Exception  ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
       
        }

		public virtual ActionResult Logout()
		{
			try
			{
				Session.Clear();
				Session.Abandon();
			}
			catch
			{
				ClearContextData();
			}
			FormsAuthentication.SignOut();
			return RedirectToAction("Login");
			//return Redirect("~/Login.aspx");
		}

		public virtual ActionResult SetMarket(int marketId)
		{
			try
			{
				CoreContext.CurrentMarketId = marketId;
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult SetLanguage(int languageId)
		{
			try
			{
				CoreContext.CurrentLanguageID = languageId;
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected internal void ClearContextData()
		{
			try
			{
				CoreContext.CurrentUser = null;
				CoreContext.SetCurrentAccountId(0);
				CoreContext.CurrentAutoship = null;
				CoreContext.UserSiteWidgets = null;

				if (Session != null)
				{
					Session["CurrentMailAccount"] = null;
					OrderContextSessionProvider.Get(Session).Clear();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public virtual void ExpireSmallCollectionCache()
		{
			SmallCollectionCache.Instance.ExpireAllCache();
		}

		public virtual void ExpireTranslationCache()
		{
			CachedData.Translation.ExpireCache();
		}

		protected virtual bool CreateMailAccount()
		{
			return false;
		}

		protected virtual void CreateMailAccountIfNull(Account account)
		{
			if (CreateMailAccount())
			{
				MailAccount mailAccount = MailAccount.LoadByAccountID(account.AccountID);

				if (mailAccount.IsNull())
				{
					var sites = Site.LoadByAccountID(account.AccountID);
					if (sites != null || sites.Count > 0)
					{
						var siteUrl = sites.SelectMany(s => s.SiteUrls).FirstOrDefault();
						if (siteUrl != null && !siteUrl.Url.IsNullOrEmpty())
						{
							MailDomain mailDomain = MailDomain.LoadDefaultForInternal();
							string emailAddress = string.Format("{0}@{1}", siteUrl.Url.GetURLSubdomain(), mailDomain.DomainName);

							// Default to their account.AccountNumber if their email is no longer available - JHE
							if (!MailAccount.IsAvailable(emailAddress, account.AccountID))
								emailAddress = string.Format("{0}@{1}", account.AccountNumber, mailDomain.DomainName);

							mailAccount = new MailAccount
							{
								AccountID = account.AccountID,
								EmailAddress = emailAddress,
								Active = true
							};
							mailAccount.Save();

							CoreContext.ReloadCurrentAccount();
						}
					}
				}
			}
		}

		#region ForgotPassword
		protected virtual IForgotPasswordModel GetForgotPasswordModel()
		{
			var model = Create.New<IForgotPasswordModel>();

			model.ResetPasswordUrl = Url.Action("ForgotPassword");

			string credentialType = ((LoginCredentialTypes)GetAuthUIService().GetConfiguration().PrimaryCredentialType).ToString("G");
			model.HeaderText = Translation.GetTerm("ForgotPassword_HeaderText", "Enter your {0} and we'll email you a link to reset your password.", credentialType);
			model.UsernameLabelText = Translation.GetTerm("ForgotPassword_UsernameField", credentialType);
            model.UserCFPLabelText = Translation.GetTerm("ForgotPassword_UserCFPField", credentialType);
            model.UserbirthdayLabelText = Translation.GetTerm("ForgotPassword_UserbirthdayField", credentialType);
			model.UsernameErrorText = Translation.GetTerm("ForgotPassword_UsernameRequiredError", "{0} is required.", credentialType);

			return model;
		}

		public virtual ActionResult ForgotPassword()
		{
			if (Request.IsAjaxRequest())
				return (ActionResult)PartialView("_ForgotPassword");
			else
				return View("ForgotPassword", GetForgotPasswordModel());
		}

		[HttpPost]
		public virtual ActionResult ForgotPassword(string username, string CFP, string BirthDay_)
		{
			try
			{
                DateTime BirthDay;
                if (!(DateTime.TryParse(BirthDay_, out BirthDay)))
                    BirthDay = DateTime.MinValue;
                var service = GetAuthUIService();
                var response = service.ForgotPassword_(username, CFP, BirthDay, CurrentSite.SiteID);
				return Json(new { result = response.WasSuccessful, message = response.Message });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region ResetPassword
		public virtual ActionResult ResetPassword(string accountStr)
		{
			var accountId = Account.GetAccountIdFromSingleSignOnToken(accountStr);
			if (accountId == 0)
			{
				ViewBag.LinkExpired = true;
			}
			ViewBag.AccountStr = accountStr;
			if (Request.IsAjaxRequest())
				return (ActionResult)PartialView("_ResetPassword");
			else
				return View("ResetPassword");
		}

		[HttpPost]
		public virtual ActionResult ResetPassword(string accountStr, string newPassword, string confirmPassword)
		{
			try
			{
				var accountId = Account.GetAccountIdFromSingleSignOnToken(accountStr);
				if (accountId == 0)
				{
					return Json(new { result = false, message = Translation.GetTerm("ExpiredLink", "The forgot password link has expired. Please re-submit.") });
				}
				var account = Account.LoadForSession(accountId);// Account.LoadFull(accountId);
				if (account != null)
				{
					var response = NetSteps.Data.Entities.User.NewPasswordIsValid(newPassword, confirmPassword);
					if (response.Success)
					{
						User loadedUser = NetSteps.Data.Entities.User.Load(account.UserID ?? 0);
						loadedUser.Password = newPassword;
						loadedUser.Save();
					}
					else
					{
						return Json(new { result = false, message = response.Message });
					}

					return Json(new { result = true });
				}
				return Json(new { result = false, message = Translation.GetTerm("AccountNotFound", "Account not found") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}


        #region Helper Methods
        protected virtual StringBuilder DisplayProxyLinksIfAccountActive(Account account)
        {
            StringBuilder builder = new StringBuilder();

            if (account.User.IsNotNull() && account.User.UserStatusID != Constants.UserStatus.Active.ToShort())
                return builder.Append(Translation.GetTerm("InActiveUserStatus", "User status is inactive."));

            IList<ProxyLink> accountProxyLinks = GetAccountProxyLinks(account);

            // Process Template data in links
            ProxyLink.GetAccountProxyLinks(account.AccountID, account.AccountTypeID);

            foreach (ProxyLink proxyLink in accountProxyLinks.Where(pl => pl.Active))
            {
                builder.Append(string.Format(
                     "<li><a href=\"{0}\" target=\"_blank\" rel=\"external\">{1}</a></li>", proxyLink.URL,
                     proxyLink.GetTerm()));
            }

            return builder;
        }

        protected virtual IList<ProxyLink> GetAccountProxyLinks(Account account)
        {
            var accountProxyLinks = new List<ProxyLink>();

            if (account.AccountTypeID != (int)ConstantsGenerated.AccountType.PreferredCustomer
                 && account.AccountTypeID != (int)ConstantsGenerated.AccountType.RetailCustomer)
            {
                //var proxyLinks = SmallCollectionCache.Instance.ProxyLinks;
                var proxyLinks = ProxyLink.LoadAllFull();
                foreach (var proxyLink in proxyLinks)
                {
                    var proxyLinkClone = proxyLink.Clone();

                    var eldResolver = Create.New<IEldResolver>();

                    proxyLinkClone.URL = eldResolver.EldDecode(new Uri(proxyLinkClone.URL)).AbsoluteUri;

                    accountProxyLinks.Add(proxyLinkClone);
                }
            }

            return accountProxyLinks;
        }
        #endregion
	}
}
        #endregion