using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Areas.Communication.Models.Newsletters
{
    public static class NewslettersModelExtensions
    {
        #region IndexModel
        public static void Load(
            this IndexModel model,
            IEnumerable<EmailCampaignAction> emailCampaignActions)
        {
            // Safety check
            if (model == null
                || emailCampaignActions == null
                || !emailCampaignActions.Any())
            {
                model.ShowNewsletters = false;
                return;
            }

            // Get the campaigns from the actions
            var campaigns = (from x in emailCampaignActions
                             select x.CampaignAction.Campaign).Distinct();

            // TODO: Add logic to decide default campaign
            var selectedCampaign = campaigns.First();

            // Just the actions for the selected campaign
            var filteredEmailCampaignActions = from x in emailCampaignActions
                                               where x.CampaignAction.Campaign == selectedCampaign
                                               select x;

            // TODO: Add logic to decide default action
            var selectedEmailCampaignAction = filteredEmailCampaignActions.First();

            model.ShowNewsletters = true;
            model.SelectedCampaignID = selectedCampaign.CampaignID;
            model.CampaignSelectList = (from x in campaigns
                                        select new SelectListItem
                                        {
                                            Text = x.Name,
                                            Value = x.CampaignID.ToString()
                                        }).ToList();
            model.SelectedCampaignActionID = selectedEmailCampaignAction.CampaignActionID;
            model.CampaignActionSelectList = (from x in filteredEmailCampaignActions
                                              select new SelectListItem
                                              {
                                                  Text = x.CampaignAction.Name,
                                                  Value = x.CampaignActionID.ToString()
                                              }).ToList();

            model.NewsletterInfoModel.Load(selectedEmailCampaignAction);
        }
        #endregion

        #region NewsletterInfoModel
        public static void Load(
            this NewsletterInfoModel model,
            EmailCampaignAction emailCampaignAction)
        {
            // Safety check
            if (model == null
                || emailCampaignAction == null)
            {
                model.ShowContent = model.ShowStats = false;
                return;
            }

            model.CampaignActionID = emailCampaignAction.CampaignActionID;
            model.MailingDate = emailCampaignAction.CampaignAction.LastRunDate;
            model.Name = emailCampaignAction.CampaignAction.Name;

            model.TimeRemaining = null;
            if (emailCampaignAction.CampaignAction.NextRunDate.HasValue)
            {
                var timeRemaining = emailCampaignAction.CampaignAction.NextRunDate.Value - DateTime.Now;
                if (timeRemaining.Ticks > 0)
                {
                    model.TimeRemaining = timeRemaining;
                }
            }

            model.ShowStats = emailCampaignAction.CampaignAction.IsCompleted;

            IEnumerable<int> languageIDs;
            try
            {
                var emailTemplate = EmailTemplate.LoadFull(emailCampaignAction.EmailTemplateID);
                if (emailTemplate == null)
                    throw new Exception("EmailCampaignAction does not have an EmailTemplate.");

                languageIDs = from x in emailTemplate.EmailTemplateTranslations
                              select x.LanguageID;
            }
            catch
            {
                model.ShowContent = false;
                return;
            }            

            // TODO: Add logic to decide default language
            int? selectedLanguageID = null;
            if (languageIDs.Any())
                selectedLanguageID = languageIDs.First();

            if (emailCampaignAction.CampaignAction.IsCompleted == false
                && selectedLanguageID.HasValue
                && languageIDs != null
                && languageIDs.Count() > 0)
            {
                model.ShowContent = true;
                model.SelectedLanguageID = selectedLanguageID;
                model.LanguageSelectList = (from x in languageIDs
                                            select new SelectListItem
                                            {
                                                Text = Translation.GetTerm(SmallCollectionCache.Instance.Languages.GetById(x).TermName),
                                                Value = x.ToString()
                                            }).ToList();

                model.NewsletterContentModel.Load(emailCampaignAction.CampaignActionID, selectedLanguageID.Value);
            }
            else
            {
                model.ShowContent = false;
            }
        }
        #endregion

        #region NewsletterContentModel
        public static void Load(
            this NewsletterContentModel model,
            int campaignActionID,
            int languageID)
        {
            var tokenValues = CampaignActionTokenValue.LoadAll(new CampaignActionTokenValueSearchParameters
            {
                AccountID = CoreContext.CurrentAccount.AccountID,
                CampaignActionID = campaignActionID,
                IncludeDefaults = false,
                LanguageID = languageID
            });
            
            var distributorContentValue = tokenValues.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorContent);
            var distributorImageValue = tokenValues.FirstOrDefault(x => x.TokenID == (int)Constants.Token.DistributorImage);

            model.CampaignActionID = campaignActionID;
            model.LanguageID = languageID;
            model.DistributorContent = distributorContentValue != null ? distributorContentValue.PlaceholderValue : string.Empty;
            model.DistributorImageUrl = distributorImageValue != null ? distributorImageValue.PlaceholderValue.AddWebUploadPath(Constants.Token.DistributorImage.ToString(), CoreContext.CurrentAccount.AccountID.ToString()) : string.Empty;
        }
        #endregion
    }
}