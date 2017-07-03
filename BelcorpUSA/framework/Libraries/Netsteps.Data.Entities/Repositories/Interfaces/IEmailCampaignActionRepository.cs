using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IEmailCampaignActionRepository
    {
        List<EmailCampaignAction> LoadAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null);
        List<EmailCampaignAction> LoadFullAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null);
    }
}
