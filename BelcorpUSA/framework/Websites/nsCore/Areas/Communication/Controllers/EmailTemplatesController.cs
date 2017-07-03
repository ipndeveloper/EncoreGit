using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using nsCore.Areas.Communication.Models;
using nsCore.Controllers;

namespace nsCore.Areas.Communication.Controllers
{
    public class EmailTemplatesController : BaseController
    {
        #region Email Templates

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GetEmailTemplates(int page, int pageSize, bool? active, short? type, string orderBy, Constants.SortDirection orderByDirection)
        {
            StringBuilder builder = new StringBuilder();

            List<short> unsupportedEmailTemplateTypes = new List<short>() { (short)Constants.EmailTemplateType.Autoresponder, (short)Constants.EmailTemplateType.Campaign, (short)Constants.EmailTemplateType.Newsletter };
            var emailTemplates = EmailTemplate.Search(new EmailTemplateSearchParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                Active = active,
                EmailTemplateTypeIDs = type.HasValue ? new List<short>() { type.Value } : SmallCollectionCache.Instance.EmailTemplateTypes.Where(ett => !unsupportedEmailTemplateTypes.Contains(ett.EmailTemplateTypeID)).Select(ett => ett.EmailTemplateTypeID).ToList()
            });

            int count = 0;
            foreach (var emailTemplate in emailTemplates)
            {
                builder.Append("<tr>")
                    .AppendCheckBoxCell(value: emailTemplate.EmailTemplateID.ToString())
                    .AppendLinkCell("~/Communication/EmailTemplates/Edit/" + emailTemplate.EmailTemplateID, emailTemplate.Name)
                    .AppendCell(emailTemplate.Subject)
                    .AppendCell(SmallCollectionCache.Instance.EmailTemplateTypes.GetById(emailTemplate.EmailTemplateTypeID).GetTerm())
                    .AppendCell(emailTemplate.Active.ToBool() ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                    .Append("</tr>");
                ++count;
            }

            return Json(new { result = true, totalPages = emailTemplates.TotalPages, page = builder.ToString() });
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Edit(int? id, short? emailTemplateTypeID, int? campaignID, int? campaignActionID, short? campaignTypeID)
        {
            EmailTemplateModel emailTemplateViewModel = new EmailTemplateModel();

            if (id.HasValue && id.Value > 0)
            {
                EmailTemplate emailTemplate = EmailTemplate.LoadFull(id.Value);

                var availableLanguages = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), emailTemplate.EmailTemplateTranslations);

                var tokens = EmailTemplateType.LoadFull(emailTemplate.EmailTemplateTypeID).Tokens;

                emailTemplateViewModel = new EmailTemplateModel
                {
                    EmailTemplate = emailTemplate,
                    Tokens = tokens,
                    Languages = availableLanguages,
                    IsNewsLetterType = (emailTemplate.EmailTemplateTypeID == (short)Constants.EmailTemplateType.Newsletter),
                    CampaignActionID = campaignActionID,
                    CampaignID = campaignID,
                    CampaignTypeID = campaignTypeID,
                    IsNewEmailTemplate = false,
                    CancelURL = GetRedirectUrl(campaignID.HasValue ? campaignID.Value : 0, campaignActionID.HasValue ? campaignActionID.Value : 0, campaignTypeID.HasValue ? campaignTypeID.Value : (short)0)
                };
            }
            else
            {
                emailTemplateViewModel = new EmailTemplateModel
                {
                    EmailTemplate = new EmailTemplate(),
                    Languages = TermTranslation.GetTranslatedLanguages(),
                    IsNewsLetterType = false,
                    CampaignActionID = campaignActionID,
                    CampaignID = campaignID,
                    CampaignTypeID = campaignTypeID,
                    IsNewEmailTemplate = true
                };
            }



            return View(emailTemplateViewModel);
        }

        /// <summary>
        /// Saves a new email template - now allowing users to add email template translations for each
        /// </summary>
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Save(EmailTemplate emailTemplate, int? campaignID, int? campaignActionID, short? campaignTypeID)
        {
            try
            {
                /* If it's a new email template - simply save it */
                if (emailTemplate.EmailTemplateID == 0)
                {
                    emailTemplate.Save();
                }
                else
                {   /* Update the current email template */
                    var oldEmailTemplate = EmailTemplate.Load(emailTemplate.EmailTemplateID);
                    oldEmailTemplate.Name = emailTemplate.Name;
                    oldEmailTemplate.EmailTemplateTypeID = emailTemplate.EmailTemplateTypeID;
                    oldEmailTemplate.Active = emailTemplate.Active;
                    oldEmailTemplate.Save();
                }

                string redirectUrl = "";
                if (campaignID.HasValue && campaignActionID.HasValue && campaignTypeID.HasValue)
                    redirectUrl = GetRedirectUrl(campaignID.Value, campaignActionID.Value, campaignTypeID.Value);

                return Json(new { result = true, emailTemplateID = emailTemplate.EmailTemplateID, emailTemplateTypeID = emailTemplate.EmailTemplateTypeID, redirectUrl = redirectUrl });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual string GetRedirectUrl(int campaignID, int campaignActionID, short campaignTypeID)
        {
            string redirectUrl = "";

            switch ((Constants.CampaignType)campaignTypeID)
            {
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.NotSet:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.MassEmails:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.Campaigns:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.EventBasedEmails:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.MassAlerts:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType.Newsletters:
                    redirectUrl = Url.Action("Edit", new
                                    {
                                        area = "Communication",
                                        controller = "NewsletterCampaigns",
                                        id = campaignID,
                                        campaignActionId = campaignActionID
                                    });
                    break;
                default:
                    break;
            }

            return redirectUrl;
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult ChangeStatus(bool active, List<int> items)
        {
            if (items == null)
                return Json(new { result = false, message = Translation.GetTerm("PleaseSelectAtLeastOneItem", "Please select at least one item") });
            foreach (var emailTemplate in EmailTemplate.LoadBatch(items))
            {
                if (emailTemplate.Active != active)
                {
                    emailTemplate.Active = active;
                    emailTemplate.Save();
                }
            }
            return Json(new { result = true });
        }

        /// <summary>
        /// Returns all the tokens for the EmailTemplateType selected.
        /// </summary>
        public virtual JsonResult GetTokens(short id)
        {

            var tokens = EmailTemplateType.LoadFull(id).Tokens;
            var slimTokens = from t in tokens
                             select new
                             {
                                 ID = TokenReplacer.GetDelimitedTokenizedString(t.Placeholder),
                                 Name = t.Name
                             };

            return Json(slimTokens, JsonRequestBehavior.AllowGet);
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult UploadThumbnail()
        {
            try
            {
                string cleanedFileName = string.Empty;
                string fileName = string.Empty;

                bool nonHtml5Browser = Request.Files.Count > 0;

                if (nonHtml5Browser)
                    fileName = Request.Files[0].FileName;
                else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
                    fileName = Request.Params["qqfile"];
                else
                    return Json(new { success = false, error = "No files uploaded." });

                //Save the file on the server
                cleanedFileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^a-zA-Z0-9\s_\.]", "");
                string fullPath = ConfigurationManager.GetAbsoluteFolder("EmailTemplateThumbnails") + Path.GetFileName(cleanedFileName);

                if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                if (nonHtml5Browser)
                    Request.Files[0].SaveAs(fullPath);
                else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
                    Request.SaveAs(fullPath, false);
                var json = new { success = true, filePath = ConfigurationManager.GetWebFolder("EmailTemplateThumbnails") + Path.GetFileName(cleanedFileName) };

                if (nonHtml5Browser)
                    return Content(json.ToJSON(), "text/html");
                else
                    return Json(json);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { success = false, error = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult UploadCampaignImage()
        {
            try
            {
                string cleanedFileName = string.Empty;
                string fileName = string.Empty;

                bool nonHtml5Browser = Request.Files.Count > 0;

                if (nonHtml5Browser)
                    fileName = Request.Files[0].FileName;
                else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
                    fileName = Request.Params["qqfile"];
                else
                    return Json(new { success = false, error = "No files uploaded." });

                //Save the file on the server
                cleanedFileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^a-zA-Z0-9\s_\.]", "");
                string fullPath = Path.GetFileName(cleanedFileName).AddAbsoluteUploadPath(Constants.Token.DistributorImage.ToString(), "Default");

                if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                if (nonHtml5Browser)
                    Request.Files[0].SaveAs(fullPath);
                else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
                    Request.SaveAs(fullPath, false);
                var json = new { success = true, filePath = Path.GetFileName(cleanedFileName).AddWebUploadPath(Constants.Token.DistributorImage.ToString(), "Default") };

                if (nonHtml5Browser)
                    return Content(json.ToJSON(), "text/html");
                else
                    return Json(json);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { success = false, error = exception.PublicMessage });
            }
        }
        #endregion

        #region Email Template Translations

        [ValidateInput(false)]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual JsonResult SaveEmailTemplateTranslation(EmailTemplateTranslation template, string campaignActionPlaceholder,
                            string imagePath, int? campaignActionID)
        {
            try
            {
                // int Count = 0;
                if (template.EmailTemplateTranslationID == 0)
                {
                    if (!string.IsNullOrWhiteSpace(template.AttachmentPath))
                    {
                        template.AttachmentPath = template.AttachmentPath.RemoveWebUploadPath();
                    }

                    template.Save();
                }
                else
                {
                    var oldTemplate = EmailTemplateTranslation.Load(template.EmailTemplateTranslationID);
                    oldTemplate.LanguageID = template.LanguageID;
                    oldTemplate.Subject = template.Subject;
                    oldTemplate.Body = template.Body;
                    oldTemplate.FromAddress = template.FromAddress;

                    if (!string.IsNullOrWhiteSpace(template.AttachmentPath))
                    {
                        oldTemplate.AttachmentPath = template.AttachmentPath.RemoveWebUploadPath();     // Remove upload path
                    }

                    oldTemplate.Save();
                }

                EmailTemplate currentEmailTemplate = EmailTemplate.Load(template.EmailTemplateID);

                /* 6/2/2011: Tenzin - If the email template is of type Newsletter then save new record in CampaignActionTokenValue */
                if (currentEmailTemplate.EmailTemplateTypeID == (short)Constants.EmailTemplateType.Newsletter)
                {
                    if (campaignActionID.HasValue && campaignActionID.Value > 0)
                    {
                        List<CampaignActionTokenValue> campaignActionTokens = CampaignAction.LoadFull(campaignActionID.Value)
                                                                                            .CampaignActionTokenValues
                                                                                            .Where(x => x.LanguageID == template.LanguageID)
                                                                                            .ToList();

                        // If no Campaign Action Token values - Save new ones
                        if (campaignActionTokens == null || campaignActionTokens.Count == 0)
                        {
                            // Save new Distributor content value in CampaignActionTokenValue
                            CampaignActionTokenValue distributorContentValue = new CampaignActionTokenValue
                            {
                                TokenID = (short)Constants.Token.DistributorContent,
                                AccountID = null,
                                LanguageID = template.LanguageID,
                                PlaceholderValue = campaignActionPlaceholder,
                                CampaignActionID = campaignActionID.Value
                            };
                            distributorContentValue.Save();

                            // Save new Image value in CampaignActionTokenValue
                            CampaignActionTokenValue distributorImageValue = new CampaignActionTokenValue
                            {
                                TokenID = (short)Constants.Token.DistributorImage,
                                AccountID = null,
                                LanguageID = template.LanguageID,
                                PlaceholderValue = imagePath.RemoveWebUploadPath(Constants.Token.DistributorImage.ToString(), "Default"),     // Remove web upload path
                                CampaignActionID = campaignActionID.Value
                            };
                            distributorImageValue.Save();
                        }
                        else
                        {   /* Update the values */
                            CampaignActionTokenValue contentValue = campaignActionTokens.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorContent);
                            contentValue.AccountID = null;
                            contentValue.LanguageID = template.LanguageID;
                            contentValue.PlaceholderValue = campaignActionPlaceholder;
                            contentValue.CampaignActionID = campaignActionID.Value;
                            contentValue.Save();

                            CampaignActionTokenValue imageValue = campaignActionTokens.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorImage);
                            imageValue.AccountID = null;
                            imageValue.LanguageID = template.LanguageID;
                            imageValue.PlaceholderValue = imagePath.RemoveWebUploadPath(Constants.Token.DistributorImage.ToString(), "Default");  // Remove web upload path
                            imageValue.CampaignActionID = campaignActionID.Value;
                            imageValue.Save();
                        }
                    }
                }

                return Json(new { result = true, message = Translation.GetTerm("SavedSuccessfully", "Saved successfully!"), emailTemplateID = template.EmailTemplateID, campaignActionID = campaignActionID, templateOnly = !campaignActionID.HasValue });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { success = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult DeleteTemplateTranslation(int id, int? campaignActionID, int? campaignID, int? campaignTypeID)
        {
            try
            {
                var translation = EmailTemplateTranslation.Load(id);
                translation.Delete();

                /** Also delete the Campaign Action Token Values associated with this email template translation **/
                if (campaignActionID.HasValue && campaignActionID.Value > 0)
                {
                    IList<CampaignActionTokenValue> campaignActionTokens = CampaignAction.LoadFull(campaignActionID.Value)
                                                                                         .CampaignActionTokenValues
                                                                                         .Where(x => x.LanguageID == translation.LanguageID)
                                                                                         .ToList();
                    for (int i = 0; i < campaignActionTokens.Count; i++)
                    {
                        campaignActionTokens[i].Delete();
                    }
                }

                return RedirectToAction("Edit", new { id = translation.EmailTemplateID, campaignActionID = campaignActionID, campaignID = campaignID, campaignTypeID = campaignTypeID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetEmailTemplateTranslation(int id, int? campaignActionID)
        {
            var translation = EmailTemplateTranslation.Load(id);

            string subject;
            string body;
            string from;
            string attachPath;
            bool containAttachPath;
            string distributorContent;
            string distributorImage;
            bool containImage;

            if (campaignActionID.HasValue && campaignActionID.Value > 0)
            {
                List<CampaignActionTokenValue> campaignActionTokens = CampaignAction.LoadFull(campaignActionID.Value)
                                                                                .CampaignActionTokenValues
                                                                                .Where(x => x.LanguageID == translation.LanguageID)
                                                                                .ToList();
                CampaignActionTokenValue imageValue = campaignActionTokens.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorImage);

                subject = translation.Subject;
                body = translation.Body;
                from = translation.FromAddress;
                attachPath = translation.AttachmentPath != null ? translation.AttachmentPath.AddWebUploadPath() : string.Empty; // Add web upload path
                containAttachPath = !String.IsNullOrEmpty(translation.AttachmentPath);
                distributorContent = campaignActionTokens.Count > 0 ? campaignActionTokens.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorContent).PlaceholderValue : string.Empty;
                distributorImage = imageValue != null ? imageValue.PlaceholderValue.AddWebUploadPath(Constants.Token.DistributorImage.ToString(), "Default") : string.Empty; // Add web upload path
                containImage = (imageValue != null && !String.IsNullOrEmpty(imageValue.PlaceholderValue));

                return Json(new
                {
                    subject = subject,
                    body = body,
                    from = from,
                    attachPath = attachPath,
                    containAttachPath = containAttachPath,
                    distributorContent = distributorContent,
                    distributorImage = distributorImage,
                    containImage = containImage
                });
            }
            else
            {
                subject = translation.Subject;
                body = translation.Body;
                from = translation.FromAddress;
                attachPath = translation.AttachmentPath != null ? translation.AttachmentPath.AddWebUploadPath() : string.Empty; // Add web upload path
                containAttachPath = !String.IsNullOrEmpty(translation.AttachmentPath);

                return Json(new
                {
                    subject = subject,
                    body = body,
                    from = from,
                    attachPath = attachPath,
                    containAttachPath = containAttachPath
                });
            }

        }

        public virtual ActionResult AvailableLanguages(int emailTemplateTranslationID, int id)
        {
            // Load the current emailTemplateTranslation details
            EmailTemplateTranslation currentTemplate = EmailTemplateTranslation.Load(emailTemplateTranslationID);
            Language currentLanguage = Language.Load(currentTemplate.LanguageID);
            var emailTemplate = EmailTemplate.LoadFull(id);

            var available = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), emailTemplate.EmailTemplateTranslations, currentLanguage.LanguageID);

            var availableLanguages = from lang in available
                                     select new
                                     {
                                         ID = lang.Key,
                                         Name = lang.Value
                                     };

            return Json(new { languages = availableLanguages, selected = currentLanguage.LanguageID }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult AllLanguages(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                EmailTemplate emailTemplate = EmailTemplate.LoadFull(id.Value);
                var languageTaken = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), emailTemplate.EmailTemplateTranslations);

                var languages = from lang in languageTaken
                                select new
                                {
                                    ID = lang.Key,
                                    Name = lang.Value
                                };

                return Json(languages, JsonRequestBehavior.AllowGet);
            }

            var allLang = from lang in TermTranslation.GetTranslatedLanguages()
                          select new
                          {
                              ID = lang.Key,
                              Name = lang.Value
                          };

            return Json(allLang, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        protected virtual Dictionary<int, string> LanguagesLeft(Dictionary<int, string> allLanguages, IList<EmailTemplateTranslation> templates, int? currentLanguageID = null)
        {
            Dictionary<int, string> stillAvailable = new Dictionary<int, string>();

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

        #endregion

        #region Preview Template

        [HttpPost]
        [ValidateInput(false)]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult SetupPreview(EmailTemplateContentModel model)
        {
            try
            {
                Session["EmailTemplateContentModel"] = model;

                return Json(new { success = true, result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { success = false, error = exception.PublicMessage });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult SendTestEmail(EmailTemplateContentModel model)
        {
            try
            {
                Session["EmailTemplateContentModel"] = model;

                bool sentSuccessfully = SendEmail(model);

                return Json(new { success = sentSuccessfully, message = Translation.GetTerm("EmailSentSuccessfully", "Email Sent Successfully!") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { success = false, error = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual bool SendEmail(EmailTemplateContentModel model)
        {
            bool result = false;

            EmailTemplateTranslation tempTranslation = new EmailTemplateTranslation()
            {
                Subject = model.Subject,
                Body = model.Body,
            };

            var mailMessage = EmailTemplateContentModel.GetPreviewMailMessage(model, tempTranslation);

            mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(model.To));
            var corporateMailAccountId = ConfigurationManager.GetAppSetting<int?>(ConfigurationManager.VariableKey.CorporateAccountID);
            int mailMessageID = mailMessage.Send(MailAccount.LoadByAccountID(corporateMailAccountId ?? 1), 420);

            if (mailMessageID > 0)
            {
                result = true;
            }

            return result;
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Preview()
        {
            EmailTemplateContentModel model = Session["EmailTemplateContentModel"] as EmailTemplateContentModel;
            if (model == null)
                return Content("<html><body>No preview data</body></html>", "text/html");

            EmailTemplateTranslation tempTranslation = new EmailTemplateTranslation()
            {
                Subject = model.Subject,
                Body = model.Body,
            };

            var mailMessage = EmailTemplateContentModel.GetPreviewMailMessage(model, tempTranslation);

            // 6-30-2011: Insert Opt-out link
            mailMessage.AppendOptOutFooter("optoutTest@email.com", Constants.Language.English.ToShort());

            // Once HtmlContent for all languages are set up use this one instead
            //mailMessage.AppendOptOutFooter("optoutTest@email.com", model.LanguageID != null ? model.LanguageID.Value : Constants.Language.English.ToShort());

            return Content(mailMessage.HTMLBody ?? mailMessage.Body, "text/html");
        }
        #endregion //Preview Template
    }
}
