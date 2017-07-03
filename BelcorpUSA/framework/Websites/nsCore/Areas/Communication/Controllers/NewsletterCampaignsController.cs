using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Communication.Controllers
{
    public class NewsletterCampaignsController : BaseController
    {
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Index(int marketID = -1)
        {
            return View();
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Create()
        {
            var newsletterCampaign = new Campaign();
            newsletterCampaign.CampaignTypeID = (short)Constants.CampaignType.Newsletters;
            return View("Edit", newsletterCampaign);
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Edit(int id = 0)
        {
            return View(Campaign.LoadFull(id));
        }

        public virtual ActionResult GetNewsletters(int page, int pageSize, bool? active, short? marketID, string orderBy, Constants.SortDirection orderByDirection)
        {
            StringBuilder builder = new StringBuilder();

            var newsletterCampaigns = Campaign.Search(new CampaignSearchParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                Active = active,
                MarketID = marketID
            });

            int count = 0;
            foreach (var newsletterCampaign in newsletterCampaigns)
            {
                builder.Append("<tr>")
                    .AppendLinkCell("~/Communication/NewsletterCampaigns/Edit/" + newsletterCampaign.CampaignID, newsletterCampaign.Name)
                    .AppendCell(newsletterCampaign.NextCampaignActionScheduledDate.HasValue ? newsletterCampaign.NextCampaignActionScheduledDate.Value.ToString(CoreContext.CurrentCultureInfo) : "")
                    .AppendCell(SmallCollectionCache.Instance.Markets.GetById(newsletterCampaign.MarketID).GetTerm())
                    .AppendCell(newsletterCampaign.Active.ToString())
                    .Append("</tr>");
                ++count;
            }

            return Json(new { result = true, totalPages = newsletterCampaigns.TotalPages, page = builder.ToString() });
        }

        public virtual ActionResult GetNewsletterActions(int page, int pageSize, bool? active, short? campaignID, string orderBy, Constants.SortDirection orderByDirection, Constants.CampaignActionType campaignActionType)
        {
            StringBuilder builder = new StringBuilder();

            var newsletterCampaigns = CampaignAction.Search(new CampaignActionSearchParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                Active = active,
                CampaignID = campaignID,
                CampaignActionType = campaignActionType
            });

            int count = 0;
            foreach (var newsletterCampaign in newsletterCampaigns)
            {
                //EmailCampaignAction emailtemplate = newsletterCampaign.EmailCampaignActionObject;

                string editTemplateUrl = "";

                if (newsletterCampaign.EmailTemplateID.HasValue)
                {
                    editTemplateUrl = Url.Action("Edit", new
                    {
                        area = "Communication",
                        controller = "EmailTemplates",
                        id = newsletterCampaign.EmailTemplateID,
                        emailTemplateTypeID = (short)Constants.EmailTemplateType.Newsletter,
                        campaignID = campaignID,
                        campaignActionID = newsletterCampaign.CampaignActionID,
                        campaignTypeID = (short)Constants.CampaignType.Newsletters
                    });
                }

                builder
                    .Append(string.Format("<tr data-id=\"{0}\" data-type=\"{1}\">", newsletterCampaign.CampaignActionID, campaignActionType))
                    .AppendLinkCell("javascript:void(0);", newsletterCampaign.Name, linkCssClass: "btnEditCampaignAction")
                    .AppendCell(newsletterCampaign.NextRunDateUTC.HasValue ? newsletterCampaign.NextRunDateUTC.Value.UTCToLocal().ToString(CoreContext.CurrentCultureInfo) : "")
                    .AppendCell(newsletterCampaign.Active.ToString());
                if (campaignActionType != NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.Alert)
                    builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("ViewStats", "View Stats"), linkCssClass: "btnViewStats");
                builder.Append("</tr>");

                ++count;
            }

            return Json(new { result = true, totalPages = newsletterCampaigns.TotalPages, page = builder.ToString() });
        }

        [HttpPost]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Save(int campaignID = 0, string name = "", int marketID = 0, bool active = false)
        {
            try
            {
                Campaign newsletterCampaign;
                if (campaignID > 0)
                    newsletterCampaign = Campaign.LoadFull(campaignID);
                else
                    newsletterCampaign = new Campaign();

                newsletterCampaign.CampaignTypeID = (short)Constants.CampaignType.Newsletters;
                newsletterCampaign.MarketID = marketID;
                newsletterCampaign.Name = name;
                newsletterCampaign.Active = active;

                newsletterCampaign.Save();

                return Json(new { success = true, message = Translation.GetTerm("Newslettersuccessfullysaved", "Newsletter successfully saved.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Newsletter Campaign Actions

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult EditCampaignActionModal(int newsletterCampaignID = 0, int campaignActionID = 0, Constants.CampaignActionType campaignActionType = Constants.CampaignActionType.Email)
        {
            CampaignAction newsletterCampaignAction;
            if (campaignActionID > 0)
                newsletterCampaignAction = CampaignAction.LoadFull(campaignActionID);
            else
                newsletterCampaignAction = new CampaignAction()
                {
                    CampaignID = newsletterCampaignID,
                    CampaignActionTypeID = (short)campaignActionType
                };

            return View(newsletterCampaignAction);
        }

        [HttpPost]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult SaveCampaignAction(int newsletterCampaignID, /*int emailTemplateID,*/ DateTime? scheduledRunDate, DateTime? distributorEditableDate, int newsletterCampaignActionID = 0, string name = "", bool active = false)
        {
            //dont allow edit date after run date. if it slipped past the javascript, adjust it here.  
            if (scheduledRunDate.HasValue && distributorEditableDate.HasValue && distributorEditableDate.Value > scheduledRunDate.Value)
            {
                distributorEditableDate = scheduledRunDate;
            }

            CampaignAction newsletterCampaignAction;
            if (newsletterCampaignActionID > 0)
            {
                //load existing campaign action
                newsletterCampaignAction = CampaignAction.LoadFull(newsletterCampaignActionID);
            }
            else
            {
                //create new campaign action
                newsletterCampaignAction = new CampaignAction()
                {
                    CampaignID = newsletterCampaignID,
                    CampaignActionTypeID = (short)Constants.CampaignActionType.Email,
                };
            }
            //update and save campaign action
            newsletterCampaignAction.Name = name;
            newsletterCampaignAction.Active = active;
            newsletterCampaignAction.NextRunDate = scheduledRunDate;
            newsletterCampaignAction.Save();

           SaveCampaignActionResult result;
           if (newsletterCampaignAction.CampaignActionTypeID == (short)Constants.CampaignActionType.Email)
               result = SaveEmailCampaignAction(newsletterCampaignID, distributorEditableDate, newsletterCampaignAction);
           else
               result = SaveAlertCampaignAction(newsletterCampaignID, distributorEditableDate, newsletterCampaignAction);

            return Json(new { result = true, message = "", emailTemplateID = result.TemplateID, editTemplateUrl = result.EditTemplateUrl });
        }
        private struct SaveCampaignActionResult
        {
            public int TemplateID;
            public string EditTemplateUrl;
            public SaveCampaignActionResult(int templateID, string editTemplateUrl)
            {
                TemplateID = templateID;
                EditTemplateUrl = editTemplateUrl;
            }
        }


        private SaveCampaignActionResult SaveAlertCampaignAction(int newsletterCampaignID, DateTime? distributorEditableDate, CampaignAction newsletterCampaignAction)
        {
            int alertTemplateID;
            string editTemplateUrl;
            AlertCampaignAction alertCampaignAction;
            if (newsletterCampaignAction.AlertCampaignActions.Count() == 0)
            {
                //create email template
                var alertTemplate = new AlertTemplate();
                alertTemplate.Active = true;
                alertTemplate.Name = newsletterCampaignAction.Name;
                alertTemplate.Save();
                alertCampaignAction = new AlertCampaignAction();
                alertCampaignAction.AlertTemplateID = alertTemplate.AlertTemplateID;

                //add email campaign action to campaign action
                newsletterCampaignAction.AlertCampaignActions.Add(alertCampaignAction);
            }
            else
                alertCampaignAction = newsletterCampaignAction.AlertCampaignActions.FirstOrDefault();

            alertCampaignAction.DistributorEditableDate = distributorEditableDate;

            //save email campaign action
            alertCampaignAction.Save();

            alertTemplateID = alertCampaignAction.AlertTemplateID;
            //url used by client to redirect to edit email template
            editTemplateUrl = Url.Action("Edit", new
            {
                area = "Communication",
                controller = "Alerts",
                id = alertTemplateID
            });
            return new SaveCampaignActionResult(alertTemplateID, editTemplateUrl);
        }

        private SaveCampaignActionResult SaveEmailCampaignAction(int newsletterCampaignID, DateTime? distributorEditableDate, CampaignAction newsletterCampaignAction)
        {
            int emailTemplateID;
            string editTemplateUrl;
            EmailCampaignAction emailCampaignAction;
            if (newsletterCampaignAction.EmailCampaignActions.Count() == 0)
            {
                //create email template
                EmailTemplate emailTemplate = new EmailTemplate();
                emailTemplate.Active = true;
                emailTemplate.EmailTemplateTypeID = (short)Constants.EmailTemplateType.Newsletter;
                emailTemplate.Name = newsletterCampaignAction.Name;
                emailTemplate.Save();

                emailCampaignAction = new EmailCampaignAction();
                emailCampaignAction.EmailTemplate = emailTemplate;

                //add email campaign action to campaign action
                newsletterCampaignAction.EmailCampaignActions.Add(emailCampaignAction);
            }
            else
            {
                emailCampaignAction = newsletterCampaignAction.EmailCampaignActions.FirstOrDefault();
            }

            emailCampaignAction.DistributorEditableDate = distributorEditableDate;

            //save email campaign action
            emailCampaignAction.Save();

            emailTemplateID = emailCampaignAction.EmailTemplateID;
            //url used by client to redirect to edit email template
            editTemplateUrl = Url.Action("Edit", new
            {
                area = "Communication",
                controller = "EmailTemplates",
                id = emailTemplateID,
                emailTemplateTypeID = (short)Constants.EmailTemplateType.Newsletter,
                campaignID = newsletterCampaignID,
                campaignActionID = newsletterCampaignAction.CampaignActionID,
                campaignTypeID = (short)Constants.CampaignType.Newsletters
            });
            return new SaveCampaignActionResult(emailTemplateID, editTemplateUrl);
        }
        #endregion
    }
}
