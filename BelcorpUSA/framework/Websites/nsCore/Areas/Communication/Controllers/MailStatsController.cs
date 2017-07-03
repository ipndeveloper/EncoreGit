using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Mail;
using nsCore.Areas.Communication.Models.MailStats;
using nsCore.Controllers;

namespace nsCore.Areas.Communication.Controllers
{
    public class MailStatsController : BaseController
    {
        public virtual PartialViewResult Header(
            int campaignActionID)
        {
            var campaignAction = CampaignAction.Load(campaignActionID);

            var model = new HeaderModel
            {
                MailingDate = campaignAction.LastRunDate,
                Totals = MailMessageGroupAddress.GetSearchTotals(new MailMessageGroupAddressSearchParameters
                {
                    CampaignActionID = campaignActionID
                }),
            };

            return PartialView(model);
        }
    }
}
