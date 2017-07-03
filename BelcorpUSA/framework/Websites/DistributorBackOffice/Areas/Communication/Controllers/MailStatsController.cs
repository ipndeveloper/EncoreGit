using System;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Communication.Models.MailStats;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using Constants = NetSteps.Data.Entities.Constants;

namespace DistributorBackOffice.Areas.Communication.Controllers
{
    public class MailStatsController : BaseCommunicationController
    {
        [FunctionFilter("Communications-Email Stats", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual PartialViewResult Index(
            Constants.MailMessageGroupAddressSearchType searchType,
            int? campaignActionID,
            int? campaignSubscriptionAddedByAccountID,
            DateTime? mailingDate)
        {
            var model = new IndexModel
            {
                SearchType = searchType,
                CampaignActionID = campaignActionID,
                MailingDate = mailingDate,
                Totals = MailMessageGroupAddress.GetSearchTotals(new MailMessageGroupAddressSearchParameters
                {
                    CampaignActionID = campaignActionID,
                    CampaignSubscriptionAddedByAccountID = campaignSubscriptionAddedByAccountID
                }),
                RecipientListModel = new RecipientListModel
                {
                    SearchType = searchType,
                    CampaignActionID = campaignActionID,
                    CampaignSubscriptionAddedByAccountID = campaignSubscriptionAddedByAccountID
                }
            };

            return PartialView(model);
        }

        [HttpPost]
        [FunctionFilter("Communications-Email Stats", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual PartialViewResult RecipientList(
            Constants.MailMessageGroupAddressSearchType searchType,
            int? campaignActionID,
            int? campaignSubscriptionAddedByAccountID)
        {
            var model = new RecipientListModel
            {
                SearchType = searchType,
                CampaignActionID = campaignActionID,
                CampaignSubscriptionAddedByAccountID = campaignSubscriptionAddedByAccountID
            };
            return PartialView(model);
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Communications-Email Stats", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult RecipientListData(
            Constants.MailMessageGroupAddressSearchType searchType,
            int? campaignActionID,
            int? campaignSubscriptionAddedByAccountID,
            int page,
            int pageSize,
            string orderBy,
            Constants.SortDirection orderByDirection)
        {
            try
            {
                var recipients = MailMessageGroupAddress.Search(new MailMessageGroupAddressSearchParameters
                {
                    SearchType = searchType,
                    CampaignActionID = campaignActionID,
                    CampaignSubscriptionAddedByAccountID = campaignSubscriptionAddedByAccountID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                if (recipients.Count > 0)
                {
                    var builder = new StringBuilder();
                    foreach (var recipient in recipients)
                    {
                        builder
                            .Append("<tr>")
                            .AppendCell(recipient.FirstName)
                            .AppendCell(recipient.LastName)
                            .AppendCell(recipient.EmailAddress)
                            .AppendCell(recipient.TotalActions.ToString("N0", CoreContext.CurrentCultureInfo))
                            .AppendCell(recipient.LastActionDate.HasValue ? recipient.LastActionDate.Value.ToString("g", CoreContext.CurrentCultureInfo) : "")
                            .AppendLinkCell(recipient.LastClickUrl, recipient.LastClickUrl, target: "_blank")
                            .Append("</tr>");
                    }
                    return Json(new { result = true, totalPages = recipients.TotalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
