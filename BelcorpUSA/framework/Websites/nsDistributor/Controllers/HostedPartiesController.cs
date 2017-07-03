using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using Constants = NetSteps.Data.Entities.Constants;

namespace nsDistributor.Controllers
{
    public class HostedPartiesController : BaseController
    {
        protected static MailAccount _corporateMailAccount = null;
        private static object _lock = new object();

        protected Dictionary<string, string> _anonymousActionNames = new Dictionary<string, string>();

        public HostedPartiesController()
        {
            _anonymousActionNames.Clear();
            _anonymousActionNames.Add("Login", "Login");
        }

        #region Action Executing
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.HasGuestInviteContent = false;
            if (!IsLoggedIn && !_anonymousActionNames.ContainsKey(filterContext.ActionDescriptor.ActionName))
            {
                if (Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
                }
                else
                {
                    bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                    var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                    var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
                    filterContext.Result = Redirect("~" + distributor + "/Home");
                }
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        protected void CallParentOnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region Login
        public virtual ActionResult Login(string sso)
        {
            try
            {
                int partyId = Party.DecryptPartyID(sso);
                var party = Party.LoadFull(partyId);

                //TODO: If party has been closed for x amount of time, display an error message.

                var firstOrDefault = party.Order.OrderCustomers.FirstOrDefault(oc => oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess);
                if (firstOrDefault != null)
                {
                    int hostAccountId = firstOrDefault.AccountID;

                    var account = Account.LoadForSession(hostAccountId);
                    CoreContext.CurrentAccount = account;

                    if (account.User != null && !string.IsNullOrEmpty(account.User.Username))
                        return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

        public virtual ActionResult Index()
        {
            List<Party> parties = Party.GetHostedParties(CoreContext.CurrentAccount.AccountID);
            //if (parties.Count == 1)
            //    return RedirectToAction("GuestList", new { partyId = parties[0].PartyID });
            ViewBag.Parties = parties;
			ViewBag.CurrentCultureInfo = CoreContext.CurrentCultureInfo;
            return View(CurrentSite.GetPageByUrl("/HostedParties"));
        }

        public virtual ActionResult GuestList(int partyId)
        {
            return View(Party.LoadWithGuests(partyId));
        }

        #region Invite Guests
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult InviteGuests(int partyId)
        {
            var guestInvitation = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
            {
                Active = true,
                PageIndex = 0,
                PageSize = 1,
                EmailTemplateTypeIDs = new List<short> { (int)Constants.EmailTemplateType.EvitesCustomerInvite }
            }).FirstOrDefault();

            if (guestInvitation != null)
            {
                var guestInviteEmailTranslation = guestInvitation.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
                ViewBag.HasGuestInviteContent = guestInviteEmailTranslation != null && guestInviteEmailTranslation.Body.Contains("{{DistributorContent}}");

                var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(guestInvitation.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                ViewBag.PersonalizedContent = customTokens.Any(t => t.PartyID == partyId && t.Token == "DistributorContent") ? customTokens.First(t => t.PartyID == partyId && t.Token == "DistributorContent").Value : "";
                ViewBag.InvitationThumbnail = guestInvitation.AttachmentPath.ReplaceFileUploadPathToken();
            }
			else
				ViewBag.HasGuestInviteContent = false;

            ViewBag.Party = Party.Load(partyId);
            return View();
        }

        [ValidateInput(false)]
        public virtual ActionResult SetupPreview(string content)
        {
            try
            {
                TempData["content"] = content;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult PreviewEmail(short emailTemplateTypeId)
        {
            var emailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
            {
                Active = true,
                PageIndex = 0,
                PageSize = 1,
                EmailTemplateTypeIDs = new List<short>() { emailTemplateTypeId }
            }).FirstOrDefault();

            if (emailTemplate != null)
            {
                string content = TempData["content"].ToString();

                var translation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);

                if (translation != null)
                {
                    return Content(translation.GetTokenReplacedMailMessage(
                        new CompositeTokenValueProvider(
                            new FakePartyTokenValueProvider(),
                            new FakePartyGuestTokenValueProvider(),
                            new PersonalizedContentTokenValueProvider(content, null))).HTMLBody, "text/html");
                }
            }

            return Content(Translation.GetTerm("NoPreviewAvailable", "No preview available."));
        }

        public virtual ActionResult SaveMessage(int partyId, short emailTemplateTypeId, string message)
        {
            try
            {
                var emailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
                {
                    Active = true,
                    PageIndex = 0,
                    PageSize = 1,
                    EmailTemplateTypeIDs = new List<short>() { emailTemplateTypeId }
                }).FirstOrDefault();
                if (emailTemplate != null)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplate.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                        var token = customTokens.Any(t => t.Token == "DistributorContent" && t.PartyID == partyId) ? customTokens.First(t => t.Token == "DistributorContent" && t.PartyID == partyId) : new EmailTemplateToken();
                        //token.EmailTemplateID = emailTemplate.EmailTemplateID;
                        token.AccountID = CoreContext.CurrentAccount.AccountID;
                        token.PartyID = partyId;
                        token.Token = "DistributorContent";
                        token.Value = message;

                        token.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual void PersistGuests(List<PartyGuest> guests)
        {
            TempData["Guests"] = guests;
        }

        public virtual ActionResult ImportGuests(int partyId, HttpPostedFileBase guestImportFile)
        {
            var guests = TempData["Guests"] as List<PartyGuest> ?? new List<PartyGuest>();
            try
            {
                using (CsvReader reader = new CsvReader(new StreamReader(guestImportFile.InputStream), false))
                {
                    reader.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                    while (reader.ReadNextRecord())
                    {
                        var firstName = reader[0];
                        var lastName = reader[1];
                        var email = reader[2];

                        guests.Add(new PartyGuest()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            EmailAddress = email
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var e = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                TempData["Error"] = e.PublicMessage;
            }
            finally
            {
                TempData["Guests"] = guests;
            }
            return RedirectToAction("InviteGuests", new { partyId = partyId });
        }

        public virtual ActionResult SendInvites(int partyId, List<PartyGuest> guests)
        {
            try
            {
                var party = Party.LoadWithGuests(partyId);

                var guestInviteEmailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
                {
                    Active = true,
                    PageIndex = 0,
                    PageSize = 1,
                    EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesCustomerInvite }
                }).FirstOrDefault();
                var translation = guestInviteEmailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);

                foreach (var guest in guests)
                {
                    //Try to get the account id of this person so we can link them all together - DES
                    guest.AccountID = Account.GetNonProspectAccountIDByEmail(guest.EmailAddress);
                    if (!party.PartyGuests.Any(pg => pg.EmailAddress == guest.EmailAddress))
                    {
                        party.PartyGuests.Add(guest);
                    }
                }

                party.Save();

                foreach (var guest in guests.Where(g => g.PartyGuestID > 0))
                {
                    if (translation != null)
                    {
                        var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(guestInviteEmailTemplate.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                        var token = customTokens.Any(t => t.Token == "DistributorContent" && t.PartyID == partyId) ? customTokens.First(t => t.Token == "DistributorContent" && t.PartyID == partyId) : new EmailTemplateToken();
                    	var partyTokenValueProvider = Create.NewWithParams<PartyTokenValueProvider>(LifespanTracking.External, Param.Value(party));
                    	var partyGuestTokenValueProvider = Create.NewWithParams<PartyGuestTokenValueProvider>(LifespanTracking.External, Param.Value(guest));
                        var message = translation.GetTokenReplacedMailMessage(new CompositeTokenValueProvider(partyTokenValueProvider, partyGuestTokenValueProvider, new PersonalizedContentTokenValueProvider(token.Value, null)));
                        if (_corporateMailAccount == null)
                        {
                            lock (_lock)
                            {
                                if (_corporateMailAccount == null)
                                    _corporateMailAccount = MailAccount.GetCorporateMailAccount();
                            }
                        }
                        message.To.Add(new MailMessageRecipient(guest.EmailAddress));
                        var host = party.Order.GetHostess();
                        if (MailMessage.SetReplyToEmailAddress())
                        {
                            message.ReplyToAddress = host.AccountInfo.EmailAddress;
                        }
                        message.Send(_corporateMailAccount, CurrentSite.SiteID);
                    }
                }

                TempData["SavedGuests"] = true;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Re-invite Guests
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ReinviteGuests(int partyId)
        {
            var guestInvitation = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
            {
                Active = true,
                PageIndex = 0,
                PageSize = 1,
                EmailTemplateTypeIDs = new System.Collections.Generic.List<short>() { (int)Constants.EmailTemplateType.EvitesCustomerInvite }
            }).FirstOrDefault();
            if (guestInvitation != null)
            {
                var guestInviteEmailTranslation = guestInvitation == null ? null : guestInvitation.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
                ViewBag.HasGuestInviteContent = guestInviteEmailTranslation != null && guestInviteEmailTranslation.Body.Contains("{{DistributorContent}}");

                var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(guestInvitation.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                ViewBag.PersonalizedContent = customTokens.Any(t => t.PartyID == partyId && t.Token == "DistributorContent") ? customTokens.First(t => t.PartyID == partyId && t.Token == "DistributorContent").Value : "";
                ViewBag.InvitationThumbnail = guestInvitation.AttachmentPath.ReplaceFileUploadPathToken();
            }
			else
				ViewBag.HasGuestInviteContent = false;

            return View(Party.LoadWithGuests(partyId));
        }

        public virtual ActionResult ResendInvites(int partyId, List<PartyGuest> guests)
        {
            try
            {
                var party = Party.LoadWithGuests(partyId);

                var guestInviteEmailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
                {
                    Active = true,
                    PageIndex = 0,
                    PageSize = 1,
                    EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesCustomerInvite }
                }).FirstOrDefault();
                var translation = guestInviteEmailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);

                foreach (var guest in guests)
                {
                    var oldGuest = party.PartyGuests.FirstOrDefault(pg => pg.PartyGuestID == guest.PartyGuestID);
                    //Try to get the account id of this person so we can link them all together - DES
                    guest.AccountID = Account.GetNonProspectAccountIDByEmail(guest.EmailAddress);

                    oldGuest.AccountID = guest.AccountID;
                    oldGuest.FirstName = guest.FirstName;
                    oldGuest.LastName = guest.LastName;
                    oldGuest.EmailAddress = guest.EmailAddress;

                	if (translation == null)
                	{
                		continue;
                	}

                	var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(guestInviteEmailTemplate.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                	var token = customTokens.Any(t => t.Token == "DistributorContent" && t.PartyID == partyId) ? customTokens.First(t => t.Token == "DistributorContent" && t.PartyID == partyId) : new EmailTemplateToken();

                	var partyTokenValueProvider = Create.NewWithParams<PartyTokenValueProvider>(LifespanTracking.External, Param.Value(party));
                	var message = translation.GetTokenReplacedMailMessage(new CompositeTokenValueProvider(partyTokenValueProvider, new PartyGuestTokenValueProvider(guest), new PersonalizedContentTokenValueProvider(token.Value, null)));
                	if (_corporateMailAccount == null)
                	{
                		lock (_lock)
                		{
                			if (_corporateMailAccount == null)
                				_corporateMailAccount = MailAccount.GetCorporateMailAccount();
                		}
                	}
                	message.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(guest.EmailAddress));
                	var host = party.Order.GetHostess();
                	if (MailMessage.SetReplyToEmailAddress())
                	{
                		message.ReplyToAddress = host.AccountInfo.EmailAddress;
                	}
                	message.Send(_corporateMailAccount, CurrentSite.SiteID);
                }

                party.Save();

                TempData["SavedGuests"] = true;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        public virtual ActionResult SendThankYou(int partyId)
        {
            var thankYou = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
            {
                Active = true,
                PageIndex = 0,
                PageSize = 1,
                EmailTemplateTypeIDs = new System.Collections.Generic.List<short>() { (int)Constants.EmailTemplateType.EvitesThankYou }
            });
            if (thankYou.Count > 0)
            {
                var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(thankYou.First().EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                ViewBag.PersonalizedContent = customTokens.Any(t => t.PartyID == partyId && t.Token == "DistributorContent") ? customTokens.First(t => t.PartyID == partyId && t.Token == "DistributorContent").Value : "";
                ViewBag.ThankYouThumbnail = thankYou.First().AttachmentPath.ReplaceFileUploadPathToken();
            }
            return View(Party.LoadWithGuests(partyId));
        }

        public virtual ActionResult SendOutThankYous(int partyId, List<int> guests)
        {
            try
            {
                if (guests != null && guests.Count > 0)
                {
                    var party = Party.LoadWithGuests(partyId);

                    var thankYouEmailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
                    {
                        Active = true,
                        PageIndex = 0,
                        PageSize = 1,
                        EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesThankYou }
                    }).FirstOrDefault();
                    var translation = thankYouEmailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);

                    var partyGuests = PartyGuest.LoadBatch(guests);
                    foreach (var guest in partyGuests)
                    {
                    	if (translation == null)
                    	{
                    		continue;
                    	}

                    	var partyTokenValueProvider = Create.NewWithParams<PartyTokenValueProvider>(LifespanTracking.External, Param.Value(party));
                    	var message = translation.GetTokenReplacedMailMessage(new CompositeTokenValueProvider(partyTokenValueProvider, new PartyGuestTokenValueProvider(guest)));
                    	if (_corporateMailAccount == null)
                    	{
                    		lock (_lock)
                    		{
                    			if (_corporateMailAccount == null)
                    			{
                    				_corporateMailAccount = MailAccount.GetCorporateMailAccount();
                    			}
                    		}
                    	}
                    	message.To.Add(new MailMessageRecipient(guest.EmailAddress));
                    	var host = party.Order.GetHostess();
                    	if (MailMessage.SetReplyToEmailAddress())
                    	{
                    		message.ReplyToAddress = host.AccountInfo.EmailAddress;
                    	}
                    	message.Send(_corporateMailAccount, CurrentSite.SiteID);
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SaveThankYouMessage(int partyId, string message)
        {
            try
            {
                var emailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
                {
                    Active = true,
                    PageIndex = 0,
                    PageSize = 1,
                    EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesThankYou }
                }).FirstOrDefault();
                if (emailTemplate != null)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        var customTokens = EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplate.EmailTemplateID, CoreContext.CurrentAccount.AccountID);
                        var token = customTokens.Any(t => t.Token == "DistributorContent" && t.PartyID == partyId) ? customTokens.First(t => t.Token == "DistributorContent" && t.PartyID == partyId) : new EmailTemplateToken();
                        //token.EmailTemplateID = emailTemplate.EmailTemplateID;
                        token.AccountID = CoreContext.CurrentAccount.AccountID;
                        token.PartyID = partyId;
                        token.Token = "DistributorContent";
                        token.Value = message;

                        token.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
