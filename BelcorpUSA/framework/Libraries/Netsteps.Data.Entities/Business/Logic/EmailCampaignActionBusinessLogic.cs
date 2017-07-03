using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class EmailCampaignActionBusinessLogic
    {
        public virtual List<EmailCampaignAction> LoadAllDistributorVisible(IEmailCampaignActionRepository repository, Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return repository.LoadAllDistributorVisible(campaignType, campaignID);
        }

        public virtual List<EmailCampaignAction> LoadFullAllDistributorVisible(IEmailCampaignActionRepository repository, Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return repository.LoadFullAllDistributorVisible(campaignType, campaignID);
        }
    }
}

