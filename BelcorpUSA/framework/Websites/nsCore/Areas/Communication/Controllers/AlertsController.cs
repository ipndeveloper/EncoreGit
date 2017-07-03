using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Areas.Communication.Models;
using nsCore.Controllers;

namespace nsCore.Areas.Communication.Controllers
{
    public class AlertsController : BaseController
    {
        //private List<AlertTriggerType> AlertTriggerTypes
        //{
        //    get
        //    {
        //        if (Session["Alerts"] == null)
        //            Session["Alerts"] = AlertTriggerType.LoadAll();
        //        return Session["Alerts"] as List<AlertTriggerType>;
        //        throw new NotImplementedException();
        //    }
        //}

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Edit(int id = 0)
        {
            AlertTemplateViewModel alertModel;

            if (id != 0)
            {
                AlertTemplate alertTemplate = AlertTemplate.LoadFull(id);
                alertModel = new AlertTemplateViewModel
                {
                    AlertTemplate = alertTemplate,
                    Languages = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), alertTemplate.AlertTemplateTranslations)
                };
            }
            else
            {
                alertModel = new AlertTemplateViewModel
                {
                    AlertTemplate = new AlertTemplate(),
                    Languages = TermTranslation.GetTranslatedLanguages()
                };

            }

            // If creating a new alert template
            return View(alertModel);
        }

        /// <summary>
        /// Returns all the tokens for the AlertTemplate selected.
        /// </summary>
        public virtual JsonResult GetTokens(int id)
        {
            var tokens = AlertTemplate.LoadFull(id).Tokens;
            var slimTokens = from t in tokens
                             select new
                             {
                                 ID = TokenReplacer.GetDelimitedTokenizedString(t.Placeholder),
                                 Name = t.Name
                             };
            return Json(slimTokens, JsonRequestBehavior.AllowGet);
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult SaveAlertTemplate(AlertTemplate alertTemplate)
        {
            try
            {
                /* If it's a new alert template - simply save it */
                if (alertTemplate.AlertTemplateID == 0)
                {
                    alertTemplate.StoredProcedureName = "usp_alerts_test_alert";    // TODO: StoredProcedureName ???
                    alertTemplate.Save();
                }
                else
                { /* Update the current alert template */
                    var oldAlertTemplate = AlertTemplate.Load(alertTemplate.AlertTemplateID);
                    oldAlertTemplate.Name = alertTemplate.Name;
                    oldAlertTemplate.Active = alertTemplate.Active;

                    oldAlertTemplate.AlertPriorityID = alertTemplate.AlertPriorityID;
                    oldAlertTemplate.Save();
                }

                return Json(new { result = true, alertTemplateID = alertTemplate.AlertTemplateID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult SaveAlertTemplateTranslation(AlertTemplateTranslation translation)
        {
            try
            {
                AlertTemplate alertTemplate = AlertTemplate.Load(translation.AlertTemplateID);

                // New alert template translation
                if (translation.AlertTemplateTranslationID == 0)
                {
                    alertTemplate.AlertTemplateTranslations.Add(translation);
                    alertTemplate.Save();
                }
                else
                {// Update the old one
                    var template = AlertTemplateTranslation.Load(translation.AlertTemplateTranslationID);
                    template.LanguageID = translation.LanguageID;
                    template.Message = translation.Message;
                    template.Save();
                }


                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetAlertTemplateTranslation(int alertTemplateID, int alertTemplateTranslationID = 0)
        {
            AlertTemplateViewModel viewModel = new AlertTemplateViewModel();

            AlertTemplate alertTemplate = AlertTemplate.LoadFull(alertTemplateID);

            viewModel.AlertTemplate = alertTemplate;

            /** If editing an existing alert template translation **/
            if (alertTemplateTranslationID != 0)
            {
                var alertTemplateTranslations = AlertTemplateTranslation.Load(alertTemplateTranslationID);
                viewModel.CurrentTemplateTranslation = alertTemplateTranslations;
                viewModel.Languages = AvailableLanguages(alertTemplateTranslations, alertTemplate);
            }
            else
            { /** If adding a new alert template translation **/
                viewModel.Languages = AllLanguages(alertTemplate);
            }

            return PartialView("_AlertTemplateTranslation", viewModel);

        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult DeleteAlertTemplateTranslation(int alertTemplateTranslationID, int alertTemplateID)
        {
            try
            {
                var translation = AlertTemplateTranslation.Load(alertTemplateTranslationID);
                translation.Delete();

                return Json(new { result = true, alertTemplateID = alertTemplateID });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [NonAction]
        protected Dictionary<int, string> AvailableLanguages(AlertTemplateTranslation alertTemplateTranslation, AlertTemplate alertTemplate)
        {
            Language currentLanguage = SmallCollectionCache.Instance.Languages.GetById(alertTemplateTranslation.LanguageID);

            var availableLanguages = AllLanguages(alertTemplate, currentLanguage.LanguageID);

            return availableLanguages;
        }

        [NonAction]
        protected Dictionary<int, string> AllLanguages(AlertTemplate alertTemplate, int? currentLanguageID = null)
        {
            if (alertTemplate != null)
            {
                return LanguagesLeft(TermTranslation.GetTranslatedLanguages(), alertTemplate.AlertTemplateTranslations, currentLanguageID);
            }
            else
            {
                return TermTranslation.GetTranslatedLanguages();
            }
        }


        [NonAction]
        protected Dictionary<int, string> LanguagesLeft(Dictionary<int, string> allLanguages, IList<AlertTemplateTranslation> templates, int? currentLanguageID = null)
        {
            var stillAvailable = new Dictionary<int, string>();

            // Add all the languages in the list - later we'll remove the one's that already exist
            foreach (var eachLanguages in allLanguages)
            {
                stillAvailable.Add(eachLanguages.Key, eachLanguages.Value);
            }

            // Loop through each emailTemplates and remove languages from the list that already exists
            foreach (var eachLanguages in allLanguages)
            {
                foreach (var template in templates)
                {
                    if (eachLanguages.Key == template.LanguageID && (currentLanguageID == null || (eachLanguages.Key != currentLanguageID)))
                    { 
                        stillAvailable.Remove(eachLanguages.Key);
                    }
                }
            }
            return stillAvailable;
        }

        public virtual ActionResult GetAlerts(int page, int pageSize, bool? active, string orderBy, Constants.SortDirection orderByDirection, short? alertPriorityID)
        {
            StringBuilder builder = new StringBuilder();

            var alertTemplates = AlertTemplate.Search(new AlertTemplateSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Active = active,
                    OrderByDirection = orderByDirection,
                    AlertPriorityID = alertPriorityID
                });

            int count = 0;
            foreach (var alertTemplate in alertTemplates)
            {
                builder.Append("<tr>")
                    .AppendLinkCell("~/Communication/Alerts/Edit/" + alertTemplate.AlertTemplateID, alertTemplate.Name)
                    .AppendCell(alertTemplate.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                    .Append("</tr>");
                ++count;
            }

            return Json(new { result = true, totalPages = alertTemplates.TotalPages, page = builder.ToString() });


            #region Old code that's already there from before

            //var builder = new StringBuilder();

            //var alerts = active.HasValue ? AlertTriggerTypes.Where(a => a.Active == active.Value && a.Market == marketId && a.Language == languageId) :
            //                               AlertTriggerTypes.Where(a => a.Market == marketId && a.Language == languageId);

            //alerts = orderByDirection == Constants.SortDirection.Ascending ? alerts.OrderBy(orderBy) : alerts.OrderByDescending(orderBy);

            //int count = 0;
            //var markets = new Dictionary<int, Market>();

            //foreach (AlertTriggerType alertTrigger in alerts.Skip(page * pageSize).Take(pageSize))
            //{
            //List<AlertRecipient> alertRecipients = AlertRecipient.LoadCollection(alertTrigger.AlertTriggerTypeID);
            //string recipientList = alertRecipients.Select(r => r.ReceiverType).Distinct().Join(", ");

            //    var market = new Market();
            //    if (alertTrigger.Market > 0)
            //    {
            //        market = markets.ContainsKey(alertTrigger.Market)
            //                            ? markets[alertTrigger.Market]
            //                            : (from m in CoreContext.Markets where m.Id == alertTrigger.Market select m).First();

            //        if (!markets.ContainsKey(alertTrigger.Market))
            //            markets.Add(alertTrigger.Market, market);
            //    }
            //    else
            //    {
            //        market.Title = "";
            //    }

            //    builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append("><td><input type=\"checkbox\" class=\"alertSelector\" value=\"").Append(alertTrigger.AlertTriggerTypeID)
            //            .Append("\" /></td><td><a href=\"").Append("~/Communication/EditAlert/".ResolveUrl()).Append(alertTrigger.AlertTriggerTypeID).Append("\">").Append(alertTrigger.AlertTriggerName)
            //            .Append("</a></td><td>").Append(((Constants.AlertScheduleType)alertTrigger.ScheduleID).ToStringSafe()).Append("</td><td>").Append(market.Title)
            //            .Append("</td><td>").Append(((Constants.Language)alertTrigger.Language).ToStringSafe()).Append("</td><td>").Append(recipientList).Append("</td><td>")
            //            .Append(alertTrigger.Active ? "Active " : "Inactive").Append("</td></tr>");

            //    count++;
            //}

            //return Json(new { result = true, resultCount = AlertTriggerTypes.Count(t => t.Language == languageId && t.Market == marketId), alerts = builder.ToString() });
            //try
            //{
            //    var alertTriggerTypes = AlertTriggerType.Search(new NetSteps.Common.Base.FilterPaginatedListParameters<AlertTriggerType>()
            //    {
            //        PageIndex = page,
            //        PageSize = pageSize,
            //        OrderBy = orderBy,
            //        OrderByDirection = orderByDirection,
            //        WhereClause = active.HasValue ? (Expression<Func<AlertTriggerType, bool>>)(att => att.Active == active.Value) : null
            //    });
            //    var builder = new StringBuilder();

            //    var count = 0;
            //    foreach (var alertTriggerType in alertTriggerTypes)
            //    {
            //        builder.Append("<tr>")
            //            .AppendLinkCell("~/Communication/Alerts/Edit/" + alertTriggerType.AlertTriggerTypeID, alertTriggerType.Name)
            //            .AppendCell(alertTriggerType.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
            //            .Append("</tr>");
            //        ++count;
            //    }

            //    return Json(new { result = true, totalPages = alertTriggerTypes.TotalPages, page = builder.ToString() });
            //}
            //catch (Exception ex)
            //{
            //    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            //    return Json(new { result = false, message = exception.PublicMessage });
            //}

            #endregion


            //return View();
        }

        //public virtual ActionResult EditAlert(int? id)
        //{
        //    AlertModel alertModel = new AlertModel();

        //    if (id.HasValue)
        //    {
        //        AlertTriggerType alertTriggerType = AlertTriggerType.Load(id.Value);
        //        alertModel.AlertTriggerType = alertTriggerType;

        //        var markets = new Dictionary<int, Market>();
        //        var market = new Market();
        //        if (alertModel.AlertTriggerType.Market > 0)
        //        {
        //            market = markets.ContainsKey(alertModel.AlertTriggerType.Market)
        //                                ? markets[alertModel.AlertTriggerType.Market]
        //                                : (from m in CoreContext.Markets where m.Id == alertModel.AlertTriggerType.Market select m).First();

        //            if (!markets.ContainsKey(alertModel.AlertTriggerType.Market))
        //                markets.Add(alertModel.AlertTriggerType.Market, market);
        //        }
        //        else
        //        {
        //            market.Title = "";
        //        }

        //        ViewData["MarketName"] = market.Title;
        //        ViewData["LanguageName"] = ((Constants.Language)alertModel.AlertTriggerType.Language).ToStringSafe();

        //        List<AlertRecipient> alertRecipients = AlertRecipient.LoadCollection(alertTriggerType.AlertTriggerTypeID);

        //        alertModel.AlertRecipients = GetListOfUniqueEmailTemplates(alertRecipients);

        //        return View(alertModel);
        //    }
        //    return Json(alertModel, JsonRequestBehavior.AllowGet);
        //}

        //public virtual ActionResult ChangeAlertStatus(bool active, List<int> alerts)
        //{
        //    foreach (int alertId in alerts)
        //    {
        //        AlertTriggerType alertTriggerType = AlertTriggerTypes.First(a => a.AlertTriggerTypeID == alertId);
        //        if (alertTriggerType.Active != active)
        //        {
        //            alertTriggerType.Active = active;
        //            alertTriggerType.Save();
        //        }
        //    }

        //    return Json(new { result = true });
        //}

        //protected List<AlertRecipient> GetListOfUniqueEmailTemplates(List<AlertRecipient> alertRecipients)
        //{
        //    List<AlertRecipient> distinctEmailTemplates = new List<AlertRecipient>();

        //    foreach (AlertRecipient alertRecipient in from alertRecipient in alertRecipients
        //                                              let isfound = distinctEmailTemplates.Any(distinctEmailTemplate => distinctEmailTemplate.EmailTemplate.Id == alertRecipient.EmailTemplate.Id)
        //                                              where !isfound
        //                                              select alertRecipient)
        //    {
        //        distinctEmailTemplates.Add(alertRecipient);
        //    }

        //    return distinctEmailTemplates;
        //}

        //[HttpGet]
        //public virtual ActionResult EditAlertTemplate(int? id, int alertId)
        //{
        //    EmailTemplate template = null;

        //    List<AlertRecipient> recipients = AlertRecipient.LoadCollection(alertId);
        //    ViewData["AlertId"] = alertId;
        //    ViewData["ReceiverIds"] = (from r in
        //                                   (from receiver in recipients
        //                                    where receiver.EmailTemplate.Id == id && receiver.Active
        //                                    select receiver).ToList()
        //                               select r.ReceiverType).ToList();

        //    if (id.HasValue)
        //        template = EmailTemplate.Load(id.Value);

        //    if (template == null)
        //        template = new EmailTemplate { Active = true, Id = 0 };

        //    return View(template);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public virtual ActionResult EditAlertTemplate(int emailTemplateId, int alertId, string name, bool active, List<int> receivers, string from, string subject, string body, bool? istest)
        //{
        //    EmailTemplate emailTemplate = emailTemplateId > 0 ? EmailTemplate.Load(emailTemplateId) : new EmailTemplate();
        //    emailTemplate.TemplateName = name;
        //    emailTemplate.FromAddress = from;
        //    emailTemplate.Subject = subject;
        //    emailTemplate.Body = body;
        //    emailTemplate.Active = active;

        //    string uploadDirectory = WebContext.WebConfig.FileUploadAbsolutePath;
        //    string webPath = WebContext.WebConfig.FileUploadAbsolutePath;
        //    HttpFileCollectionBase fileCollection = Request.Files;

        //    HttpPostedFileBase file = fileCollection[0];

        //    // TODO: Attachments
        //    if (file.ContentLength > 0)
        //    {
        //        file.SaveAs(string.Format(@"{0}{1}", uploadDirectory, file.FileName));
        //        emailTemplate.AttachmentPath = webPath + file.FileName;
        //    }

        //    emailTemplate.Save();

        //    List<AlertRecipient> alertRecipients = AlertRecipient.LoadCollection(alertId).Where(ar => ar.EmailTemplate.Id == emailTemplateId).ToList();
        //    foreach (var alertRecipient in alertRecipients)
        //    {
        //        int recieverTypeId = 0;
        //        switch (alertRecipient.ReceiverType)
        //        {
        //            case "Consultant":
        //                recieverTypeId = (int)Constants.AlertReciever.Consultant;
        //                break;
        //            case "Sponsor":
        //                recieverTypeId = (int)Constants.AlertReciever.Sponsor;
        //                break;
        //            case "Director":
        //                recieverTypeId = (int)Constants.AlertReciever.Director;
        //                break;
        //        }

        //        if (recieverTypeId > 0 && receivers.Contains(recieverTypeId))
        //        {
        //            alertRecipient.Active = true;
        //            receivers.Remove(recieverTypeId);
        //        }
        //        else if (recieverTypeId > 0)
        //        {
        //            alertRecipient.Active = false;
        //            receivers.Remove(recieverTypeId);
        //        }

        //        alertRecipient.Save(alertId);
        //    }

        //    foreach (int receiver in receivers)
        //    {
        //        AlertRecipient alertRecipient = new AlertRecipient()
        //        {
        //            EmailTemplate = emailTemplate,
        //            ReceiverType = ((Constants.AlertReciever)receiver).ToStringSafe()
        //        };

        //        alertRecipient.EmailTemplate.TemplateName = name;
        //        alertRecipient.Save(alertId);
        //    }

        //    if (istest.HasValue && istest.Value)
        //    {

        //        string accountNumber = System.Configuration.ConfigurationManager.AppSettings["AlertTestSendAccountNumber"];
        //        AlertTrigger alertTrigger = new AlertTrigger();
        //        alertTrigger.SaveTestTrigger(accountNumber, alertId);

        //    }

        //    ViewData["AlertId"] = alertId;
        //    ViewData["ReceiverIds"] = (from r in
        //                                   (from receiver in AlertRecipient.LoadCollection(alertId)
        //                                    where receiver.Active && receiver.EmailTemplate.Id == emailTemplateId
        //                                    select receiver).ToList()
        //                               select r.ReceiverType).ToList();

        //    ViewData["SavedEmailTemplate"] = true;
        //    ViewData["TestAlertCreated"] = istest;

        //    EmailTemplate template = EmailTemplate.Load(emailTemplate.Id) ?? new EmailTemplate();

        //    return View(template);
        //}

        //[HttpGet]
        //public virtual ActionResult GetAlertEmailTemplates(int alertId)
        //{
        //    List<AlertRecipient> alertRecipients = AlertRecipient.LoadCollection(alertId);
        //    List<AlertRecipient> templates = GetListOfUniqueEmailTemplates(alertRecipients);

        //    return Json(templates, JsonRequestBehavior.AllowGet);
        //}

        //public virtual ActionResult SaveAlert(int alertId, bool active, string description)
        //{
        //    AlertTriggerType alertTriggerType = AlertTriggerType.Load(alertId);
        //    alertTriggerType.Description = description;
        //    alertTriggerType.Active = active;
        //    alertTriggerType.Save();

        //    return Json(new { result = true });
        //}

        //[HttpGet]
        //public virtual ActionResult GetScheduleOptions()
        //{
        //    List<object> scheduleOptions = new List<object>()
        //                                       {
        //                                           new{ Id = 0,Name = Constants.AlertScheduleType.Always.ToStringSafe()},
        //                                           new {Id = 1, Name = Constants.AlertScheduleType.Daily.ToStringSafe()},
        //                                           new{Id = 2,Name = Constants.AlertScheduleType.Monthly.ToStringSafe()},
        //                                           new{Id = 3,Name = Constants.AlertScheduleType.Yearly.ToStringSafe()}
        //                                       };

        //    return Json(scheduleOptions, JsonRequestBehavior.AllowGet);
        //}

        //public virtual ActionResult Log(int id = -1)
        //{
        //    if (id > 0)
        //    {
        //        CoreContext.CurrentAccount = Account.FindByAccountNumber(id.ToString());
        //    }
        //    else
        //        return RedirectToAction("SearchAccounts", "Accounts", new { q = "", returnUrl = "~/Communication/AlertsLog/" });

        //    return View();
        //}

        //public virtual Dictionary<int, List<CommunicationAlertTriggerEntry>> AlertTriggerEntries
        //{
        //    get
        //    {
        //        if (Session["AlertTriggerEntries"] == null)
        //            Session["AlertTriggerEntries"] = new Dictionary<int, List<CommunicationAlertTriggerEntry>>();
        //        return Session["AlertTriggerEntries"] as Dictionary<int, List<CommunicationAlertTriggerEntry>>;
        //    }
        //    set { Session["AlertTriggerEntries"] = value; }
        //}

        //[HttpGet]
        //public virtual ActionResult GetAlertEntries(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection, DateTime StartDateFilter_, DateTime EndDateFilter_)
        //{
        //    if (EndDateFilter_ != null && EndDateFilter_ == EndDateFilter_.Midnight())
        //        EndDateFilter_ = EndDateFilter_.EndOfDay();

        //    List<CommunicationAlertTriggerEntry> alertTriggerEntries = new List<CommunicationAlertTriggerEntry>();
        //    if (!AlertTriggerEntries.ContainsKey(CoreContext.CurrentAccount.AccountId))
        //    {
        //        List<AlertTriggerType> alertTriggerTypes = AlertTriggerType.LoadAll();

        //        AlertTriggerCollection handledAlertTriggers =
        //            AlertTriggerCollection.LoadHandledAlertTriggers(CoreContext.CurrentAccount.AccountId);
        //        AccountCollection accountsBySponsorId =
        //            AccountCollection.GetAccountsBySponsorId(CoreContext.CurrentAccount.AccountId);
        //        foreach (Account account in accountsBySponsorId)
        //            handledAlertTriggers.AddRange(AlertTriggerCollection.LoadHandledAlertTriggers(account.AccountId));

        //        alertTriggerEntries.AddRange(handledAlertTriggers.Select(handledAlertTrigger => new CommunicationAlertTriggerEntry(handledAlertTrigger, alertTriggerTypes)));
        //        AlertTriggerEntries.Add(CoreContext.CurrentAccount.AccountId, alertTriggerEntries);
        //    }
        //    else
        //    {
        //        alertTriggerEntries = AlertTriggerEntries[CoreContext.CurrentAccount.AccountId];
        //    }

        //    alertTriggerEntries = (from alerttriggerentry in alertTriggerEntries
        //                           where
        //                               DateTime.Parse(alerttriggerentry.Created).CompareTo(StartDateFilter_) >= 0 &&
        //                               DateTime.Parse(alerttriggerentry.Processed).CompareTo(EndDateFilter_) <= 0
        //                           select alerttriggerentry).ToList();

        //    if (alertTriggerEntries.Count() > 0)
        //    {
        //        StringBuilder builder = new StringBuilder();

        //        int count = 0;
        //        foreach (CommunicationAlertTriggerEntry entry in alertTriggerEntries.Skip(page * pageSize).Take(pageSize))
        //        {
        //            builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append("><td>").Append(entry.Created).Append("</td><td>")
        //                .Append(entry.Processed).Append("</td><td>").Append(entry.Name).Append("</td><td>").Append(entry.AlertTriggerName)
        //                .Append("</td></tr>");
        //            ++count;
        //        }

        //        return Json(new { totalPages = Math.Ceiling(alertTriggerEntries.Count() / (double)pageSize), page = builder.ToString() });
        //    }
        //    return Json(new { totalPages = Math.Ceiling(alertTriggerEntries.Count() / (double)pageSize), page = "<tr><td colspan=\"7\">No ledger entries.</td></tr>" });
        //}
    }
}