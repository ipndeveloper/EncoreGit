using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Auth.UI.Common;
using NetSteps.Auth.UI.Common.Enumerations;
using NetSteps.Auth.UI.Service.Services;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common;
using NetSteps.Enrollment.Common.Configuration;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Business.Helpers;
using NetSteps.Web.Mvc.Controls.Models;
using nsDistributor.Models.Shared;
using NetSteps.Data.Entities.Business.Logic;

namespace nsDistributor.Controllers
{
    public class SecurityController : BaseOrderContextController
    {
        private readonly Lazy<IAuthenticationUIService> _authenticationUIServiceFactory = new Lazy<IAuthenticationUIService>(Create.New<IAuthenticationUIService>);
        public IAuthenticationUIService AuthenticationUIService { get { return _authenticationUIServiceFactory.Value; } }

        private readonly Lazy<IEnrollmentService> enrollmentService = new Lazy<IEnrollmentService>(Create.New<IEnrollmentService>);
        protected IEnrollmentService EnrollmentService { get { return enrollmentService.Value; } }


        /// <summary>
        /// Template names to be passed to the client-side JS model.
        /// </summary>
        public static class SignUpTemplates
        {
            public static string Default { get { return "SignUp-DefaultTemplate"; } }
        }

        [HttpGet]
        [ActionName("Login")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetLogin(string returnUrl, string token, string sso, bool kitInicio = false)
        {
            CoreContext.HasModalBeenShown = false;
            CoreContext.CurrentAccount = null;
            CoreContext.CurrentUser = null;
            OrderContext.Clear();
            CoreContext.CurrentOrder = null;
            Session.Clear();

            bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
            var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
            var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
            CoreContext.HasModalBeenShown = false;
            if (!string.IsNullOrEmpty(sso))
            {
                try
                {
                    var userId = CorporateUser.GetIdFromSingleSignOnToken(sso);
                    if (userId > 0)
                    {
                        var user = CorporateUser.LoadFull(userId);
                        CoreContext.CurrentUser = user;

                        return Redirect("~/Home?PageMode=Edit");
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
                }
                return Redirect("~" + distributor + "/Home");
            }
            else if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var accountId = Account.GetAccountIdFromSingleSignOnToken(token);
                    if (accountId > 0)
                    {
                        var account = Account.LoadForSession(accountId);
                        if (!SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).ReportAsActive)
                        {
                            throw new NetStepsBusinessException("Account is inactive.")
                            {
                                PublicMessage = Translation.GetTerm("AccountIsInactive", "Account is inactive."),
                                IncludeErrorLogMessage = false
                            };
                        }

                        CoreContext.CurrentAccount = account;
                        if (account.UserID.HasValue)
                            CoreContext.CurrentUser = account;

                        //Make sure we update the current order with the account that just logged in so they get the right prices and everything - DES
                        if (OrderContext.Order != null)
                        {
                            OrderContext.Order.AsOrder().UpdateCustomer(account);
                            var service = Create.New<IOrderService>();
                            service.UpdateOrder(OrderContext);
                        }

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return Redirect(Server.UrlDecode(returnUrl));
                        }

                        /*CS.09JUL2016.Inicio.Retoma de Primer Desde a DWS.Se adiciono parametro de entrada kitInicio*/
                        if (kitInicio)
                        {
                            return RedirectToAction("EnrollmentVariantKits", "Products", "Enroll");
                        }
                        /*CS.09JUL2016.Fin.Retoma de Primer Desde a DWS*/
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
                }
                return Redirect("~" + distributor + "/Home");
            }
            Session["ReturnUrl"] = returnUrl;

            LoadPageHtmlContent();

            return View(GetLoginModel());
        }

        public virtual ILoginModel GetLoginModel()
        {
            var model = Create.New<ILoginModel>();
            model.LoginUrl = Url.Action("Login");

            var loginConfig = GetAuthUIService().GetConfiguration();
            model.DisplayUsernameField = loginConfig.ShowUsernameFormFields;
            model.EnableAnonymousLogin = true; //todo: ya
            model.EnableForgotPassword = loginConfig.EnableForgotPassword;

            string credentialType = ((LoginCredentialTypes)loginConfig.PrimaryCredentialType).ToString("G");
            model.UsernameLabelText = Translation.GetTerm("Login_UsernameLabelText_" + credentialType, credentialType);
            model.UsernameErrorText = Translation.GetTerm("Login_UsernameErrorText_" + credentialType, "{0} is required.", credentialType);

            model.Data.SignUp = new
            {


                Data = new SignUpModelData
                {
                    SelectedAccountTypeID = Convert.ToInt16(HttpContext.Request.QueryString["accountType"])
                },
                Options = new
                {
                    SignUpTypes = GetSignUpTypeModels().ToArray(),
                    SignUpUrl = Url.Action("SignUp")
                }
            };

            var username = Request.Params["username"];
            var password = Request.Params["password"];

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var descrPassword = DesEncriptar(password.ToString());

                model.UserName = username.ToString();
                model.Password = descrPassword;
            }

            return model;
        }

        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        protected virtual IEnumerable<ISignUpTypeModel> GetSignUpTypeModels()
        {
            if (EnrollmentService.IsAccountTypeSignUpEnabled((short)Constants.AccountType.RetailCustomer))
            {
                yield return GetRetailCustomerSignUpModel();
            }

            if (EnrollmentService.IsAccountTypeSignUpEnabled((short)Constants.AccountType.PreferredCustomer))
            {
                yield return GetPreferredCustomerSignUpModel();
            }

            if (EnrollmentService.IsAccountTypeSignUpEnabled((short)Constants.AccountType.Distributor))
            {
                yield return GetDistributorSignUpModel();
            }
        }

        protected virtual bool IsUsernameCredentialType()
        {
            return AuthenticationUIService.GetConfiguration().PrimaryCredentialType == (int)LoginCredentialTypes.Username;
        }

        protected virtual ISignUpTypeModel GetRetailCustomerSignUpModel()
        {
            var model = Create.New<ISignUpTypeModel>();
            model.AccountTypeID = (short)Constants.AccountType.RetailCustomer;
            model.Template = SignUpTemplates.Default;
            model.HeadingText = Translation.GetTerm("PWS_SignUp_RetailCustomer_Heading", "Retail Customer");
            model.ToolTipText = Translation.GetTerm("AccountToolTip_RetailCustomer", "Sign up for a retail account to save time on upcoming orders.");
            model.ShowUsername = IsUsernameCredentialType();
            model.ShowPassword = true;
            model.ShowOptOut = EnrollmentService.IsAccountTypeOptOutable(model.AccountTypeID);
            return model;
        }

        protected virtual ISignUpTypeModel GetPreferredCustomerSignUpModel()
        {
            var model = Create.New<ISignUpTypeModel>();
            model.AccountTypeID = (short)Constants.AccountType.PreferredCustomer;
            model.Template = SignUpTemplates.Default;
            model.HeadingText = Translation.GetTerm("PWS_SignUp_PreferredCustomer_Heading", "Preferred Customer");
            model.ToolTipText = Translation.GetTerm("AccountToolTip_PreferredCustomer", "Receive discounts on select items as a preferred customer.");
            model.ShowUsername = IsUsernameCredentialType();
            model.ShowPassword = true;
            model.ShowOptOut = EnrollmentService.IsAccountTypeOptOutable(model.AccountTypeID);
            return model;
        }

        protected virtual ISignUpTypeModel GetDistributorSignUpModel()
        {
            var model = Create.New<ISignUpTypeModel>();
            model.AccountTypeID = (short)Constants.AccountType.Distributor;
            model.Template = SignUpTemplates.Default;
            model.HeadingText = Translation.GetTerm("PWS_SignUp_Consultant_Heading", "Consultant");
            model.ToolTipText = Translation.GetTerm("AccountToolTip_Consultant", "Make some extra cash by becoming a consultant today.");
            model.ShowUsername = IsUsernameCredentialType();
            model.ShowPassword = true;
            model.ShowOptOut = EnrollmentService.IsAccountTypeOptOutable(model.AccountTypeID);
            return model;
        }


        [HttpPost]
        [ActionName("Login")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult PostLogin(string username, string password)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { result = false, message = Translation.GetTerm("InvalidLoginPWS", "Invalid login.") });

                var authResult = GetAuthUIService().Authenticate(username, password, CurrentSite.SiteID);
                if (!authResult.WasLoginSuccessful)
                {
                    throw new NetStepsException(authResult.FailureMessage) { PublicMessage = authResult.FailureMessage };
                }

                var account = GetAccountWithCredentails(username, authResult.CredentialTypeID);
                if (account != null)
                {
                    if (!SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).ReportAsActive
                        && !(account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment && account.User.UserStatusID == (short)Constants.UserStatus.Active))
                    {
                        throw new NetStepsBusinessException("Account is inactive.")
                        {
                            PublicMessage = Translation.GetTerm("AccountIsInactive", "Account is inactive."),
                            IncludeErrorLogMessage = false
                        };
                    }

                    CoreContext.CurrentAccount = account;

                    //Make sure we update the current order with the account that just logged in so they get the right prices and everything - DES
                    if (OrderContext.Order != null)
                    {
                        ((Order)OrderContext.Order).UpdateCustomer(account);

                        foreach (var step in OrderContext.InjectedOrderSteps)
                        {
                            step.CustomerAccountID = account.AccountID;
                        }
                        var svc = Create.New<IOrderService>();
                        svc.UpdateOrder(OrderContext);
                    }

                    var returnUrl = GetSuccessfulLogonReturnUrl(account);
                    return Json(new
                    {
                        result = true,
                        returnUrl = returnUrl,
                        openNewWindow = OpenNewWindowOrTab(),
                        name = CoreContext.CurrentAccount.FullName,
                        reload = GetSuccessfulLoginReload()
                    });
                }
                return Json(new { result = false, message = Translation.GetTerm("InvalidLoginPWS", "Invalid login.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual Account GetAccountWithCredentails(string identifier, int credentialTypeID)
        {
            switch ((LoginCredentialTypes)credentialTypeID)
            {
                case LoginCredentialTypes.Username:
                case LoginCredentialTypes.CorporateUsername:
                    var userID = Create.New<IUserRepository>().GetByUsername(identifier).UserID;
                    return Create.New<IAccountRepository>().LoadByUserIdFull(userID);
                case LoginCredentialTypes.Email:
                    return Account.LoadForSession(identifier, false);
                case LoginCredentialTypes.AccountId:
                    return Account.LoadForSession(int.Parse(identifier));
                default:
                    throw new NetStepsException(Translation.GetTerm("Login_UnableToLocateAcount", "Unable to locate account"));
            }
        }

        public virtual bool OpenNewWindowOrTab()
        {
            return false;
        }

        /// <summary>
        /// Checks if the successful logon response should redirect the user to another URL.
        /// </summary>
        public virtual string GetSuccessfulLogonReturnUrl(Account account)
        {
            // BR-CD-003
            if (account != null && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment)
            {
                Tuple<int, int, int> result = new OrderBusinessLogic().CheckOrdersByAccountID(account.AccountID);   
                var haveOrderEnroll = result.Item2;
                var haveOrderEnrollPending= result.Item3;

                if (haveOrderEnroll == 0)
                {
                    return Url.Content(ProductKitsEnrollmentUrl);
                }
                else
                {
                    if (haveOrderEnrollPending == 1)
                    {
                        return Url.Content(ProductItemsEnrollmentUrl);
                    }                   
                }
            }

            //if (account != null && account.AccountStatusID == (short)Constants.AccountStatus.Active )
            //{
            //    var site = Site.LoadByAccountID(account.AccountID).FirstOrDefault();
            //    if (site == null)
            //    {
            //        return Url.Content(URLwpEnrollmentUrl);
            //    }                
            //}
            //End  BR-CD-003

            // Check for resume enrollment
            //if (EnableResumeEnrollment
            //    && account != null
            //    && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment)
            //{
            //    return Url.Content(ResumeEnrollmentUrl);
            //}

            // Check for imported enrollment
            //if (EnableImportedEnrollment
            //    && account != null
            //    && account.AccountStatusID == (short)Constants.AccountStatus.Imported)
            //{
            //    return Url.Content(ImportedEnrollmentUrl);
            //}

            string returnUrl = "";
            if (OrderContext.Order == null)
            {
                returnUrl = OnGetSuccessfulLogonReturnUrl(account);
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return returnUrl;
                }
            }

            // Check for return url
            returnUrl = Session["ReturnUrl"] as string;
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                Session["ReturnUrl"] = null;
                return returnUrl;
            }

            // Default == no redirect
            return null;
        }

        public virtual string OnGetSuccessfulLogonReturnUrl(Account account)
        {
            return null;
        }

        /// <summary>
        /// Indicates if accounts with status "Begun Enrollment" should be redirected to enrollment.
        /// Base defaults to True.
        /// </summary>
        protected virtual bool EnableResumeEnrollment { get { return true; } }

        /// <summary>
        /// The URL to which a partially enrolled account will be redirected.
        /// </summary>
        protected virtual string ResumeEnrollmentUrl { get { return Url.Action("Index", "Landing", new { area = "Enroll", resume = true }); } }

        /// <summary>
        /// Indicates if accounts with status "Imported" should be redirected to enrollment.
        /// Base defaults to True.
        /// </summary>
        protected virtual bool EnableImportedEnrollment { get { return true; } }

        /// <summary>
        /// The URL to which an imported account will be redirected.
        /// </summary>
        protected virtual string ImportedEnrollmentUrl { get { return Url.Action("Index", "Landing", new { area = "Enroll", resume = true }); } }

        /// <summary>
        /// The URL to which an imported account will be redirected. ¿¿¿¡¡¡¡
        /// </summary>
        protected virtual string ProductKitsEnrollmentUrl { get { return Url.Action("EnrollmentVariantKits", "Products", new { area = "Enroll", resume = true }); } }

        /// <summary>
        /// The URL to which an imported account will be redirected. ¿¿¿¡¡¡¡
        /// </summary>
        protected virtual string ProductItemsEnrollmentUrl { get { return Url.Action("EnrollmentItems", "Products", new { area = "Enroll", resume = true }); } }

        /// <summary>
        /// The URL to which an imported account will be redirected. ¿¿¿¡¡¡¡
        /// </summary>
        protected virtual string URLwpEnrollmentUrl { get { return Url.Action("WebSite", "Products",new { area = "Enroll", resume = true }); } }

        /// <summary>
        /// Determines if the current page should be reloaded (refreshed) on successful login.
        /// Because there are so many cases where we need the page to reload on login,
        /// that is now the default behavior. If you find a case where the page SHOULDN'T reload
        /// on login, enter that logic here.
        /// </summary>
        /// <returns>A <see cref="bool"/> indicating whether the page should be reloaded.</returns>
        protected virtual bool GetSuccessfulLoginReload()
        {
            return true;
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Logout()
        {
            try
            {
                CoreContext.HasModalBeenShown = false;
                CoreContext.CurrentAccount = null;
                CoreContext.CurrentUser = null;
                OrderContext.Clear();

                CoreContext.CurrentOrder = null;
                bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
                return Redirect("~" + distributor + "/Home");
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual ActionResult SignUp(string firstName, string lastName, string email, string username, string password, bool? OptOut, short selectedAccountTypeID, IEnrollmentContext enrollmentContext)
        {
            try
            {
                if (AuthenticationUIService.GetConfiguration().ShowUsernameFormFields && !NetSteps.Data.Entities.User.IsUsernameAvailable(0, username))
                {
                    return Json(new { result = false, message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.") });
                }

                var enrollmentConfigProvider = Create.New<IEnrollmentConfigurationProvider>();
                var account = CreateAccount(firstName, lastName, email, username, password, selectedAccountTypeID);
                OnSignUpComplete(account);
                Account.AssignRolesByAccountType(account);
                account.Save();
                var siteAccount = SiteOwner;

                if (!enrollmentConfigProvider.EnrollableAccountTypeIDs.Contains(selectedAccountTypeID) ||
                    !enrollmentConfigProvider.AccountTypeEnabled(selectedAccountTypeID))
                {
                    enrollmentContext.EnrollingAccount = account;

                    if (OptOut.HasValue && OptOut.Value && EnrollmentService.IsAccountTypeOptOutable(selectedAccountTypeID))
                    {
                        EnrollmentService.OptOut(enrollmentContext);
                    }

                    var isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                    var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                    var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
                    string returnUrl = Session["ReturnUrl"] != null ? Session["ReturnUrl"].ToString() : ("~" + distributor + "/Home").ResolveUrl();
                    Session["ReturnUrl"] = null;

                    return Json(new { result = true, returnUrl = returnUrl });
                }

                enrollmentContext.Initialize(enrollmentConfigProvider.GetEnrollmentConfig(selectedAccountTypeID, ApplicationContext.Instance.SiteTypeID),
                    AddressHelper.GetCountryID(account, siteAccount),
                    siteAccount != null ? siteAccount.DefaultLanguageID : ApplicationContext.Instance.CurrentLanguageID,
                    CurrentSite.SiteID);

                enrollmentContext.EnrollingAccount = account;
                if (OrderContext != null && OrderContext.Order != null)
                {
                    enrollmentContext.InitialOrder = OrderContext.Order;
                }

                if (OptOut.HasValue && OptOut.Value && EnrollmentService.IsAccountTypeOptOutable(selectedAccountTypeID))
                {
                    EnrollmentService.OptOut(enrollmentContext);
                }

                Session["ReturnUrl"] = null;
                var enrollmentSteps = enrollmentContext.EnrollmentConfig.Steps;
                var nextStepUrl = enrollmentSteps.Any() ? GetStepUrl(enrollmentSteps[0]) : "Enroll/Sponsor";

                if (account.IsOptedOut)
                    nextStepUrl = enrollmentSteps.Count > 1 ? GetStepUrl(enrollmentSteps[1]) : "Enroll/Products";

                return Json(new { result = true, returnUrl = nextStepUrl });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual string GetStepUrl(IEnrollmentStepConfig stepConfig)
        {
            return String.Concat("Enroll", Url.Action(string.Empty, stepConfig.Controller));
        }

        protected virtual Account CreateAccount(string firstName, string lastName, string email, string username, string password, short accountType)
        {
            Account newAccount =
                    Account.EnrollRetailCustomer(
                        SiteOwner == null ? EnrollmentService.GetCorporateSponsorID() : SiteOwner.AccountID,
                        CurrentLanguageID,
                        firstName,
                        lastName,
                        email,
                        username,
                        password,
                        accountType);
            CoreContext.CurrentAccount = newAccount;
            //Make sure we update the current order with the account that just logged in so they get the right prices and everything - DES
            if (OrderContext.Order != null)
            {
                OrderContext.Order.AsOrder().UpdateCustomer(newAccount);
                OrderService.UpdateOrder(OrderContext);
            }
            return newAccount;
        }
        protected virtual void OnSignUpComplete(Account newAccount)
        {

        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual void SetMarket(int? marketId)
        {
            if (marketId.HasValue)
                CoreContext.CurrentMarketId = marketId.Value;
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual void SetLanguage(int? languageId)
        {
            if (languageId.HasValue)
                CurrentLanguageID = languageId.Value;
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual void ExpireSmallCollectionCache()
        {
            SmallCollectionCache.Instance.ExpireAllCache();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual void ExpireTranslationCache()
        {
            CachedData.Translation.ExpireCache();
        }

        #region ForgotPassword
        protected virtual IForgotPasswordModel GetForgotPasswordModel()
        {
            var model = Create.New<IForgotPasswordModel>();
            model.ResetPasswordUrl = Url.Action("ForgotPassword");

            string credentialType = ((LoginCredentialTypes)GetAuthUIService().GetConfiguration().PrimaryCredentialType).ToString("G");
            model.HeaderText = Translation.GetTerm("ForgotPassword_HeaderText", "Enter your {0} and we'll email you a link to reset your password.", credentialType);
            model.FormHeaderText = Translation.GetTerm("ForgotPassword_FormHeader", "Enter your {0}", credentialType);
            model.UsernameLabelText = Translation.GetTerm("ForgotPassword_UsernameField", credentialType);
            model.UsernameErrorText = Translation.GetTerm("ForgotPassword_UsernameRequiredError", "{0} is required.", credentialType);

            return model;
        }

        [ActionName("ForgotPassword")]
        public virtual ActionResult ForgotPassword()
        {
            return View("ForgotPassword", GetForgotPasswordModel());
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public virtual ActionResult PostForgotPassword(string username)
        {
            try
            {
                var service = GetAuthUIService();
                int siteID = CurrentSite.IsBase ? CurrentSite.SiteID : CurrentSite.BaseSiteID.Value;
                var response = service.ForgotPassword(username, siteID);
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
        [ActionName("ResetPassword")]
        public virtual ActionResult ResetPassword(string accountStr)
        {
            var accountId = Account.GetAccountIdFromSingleSignOnToken(accountStr);
            if (accountId == 0)
            {
                ViewBag.LinkExpired = true;
            }
            ViewBag.AccountStr = accountStr;
            return View("ResetPassword");
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public virtual ActionResult PostResetPassword(string accountStr, string newPassword, string confirmPassword)
        {
            try
            {
                var accountId = Account.GetAccountIdFromSingleSignOnToken(accountStr);
                if (accountId == 0)
                {
                    return Json(new { result = false, message = Translation.GetTerm("ExpiredLink", "The forgot password link has expired. Please re-submit.") });
                }
                var account = Account.LoadFull(accountId);
                if (account != null)
                {
                    var response = NetSteps.Data.Entities.User.NewPasswordIsValid(newPassword, confirmPassword);
                    if (response.Success)
                    {
                        account.User.Password = newPassword;
                        account.Save();
                        if (CoreContext.CurrentAccount != null && CoreContext.CurrentAccount.AccountID != account.AccountID)
                        {
                            this.Logout();
                        }
                        this.PostLogin(account.User.Username, newPassword);
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
        #endregion

        #region PasswordUpdated
        public ActionResult PasswordUpdated()
        {
            if (IsLoggedIn)
                return View(CoreContext.CurrentAccount);
            else
                return RedirectToAction("ForgotPassword");
        }
        #endregion
    }
}
