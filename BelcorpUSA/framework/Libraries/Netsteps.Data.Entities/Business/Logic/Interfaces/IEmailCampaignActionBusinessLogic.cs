using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;
namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IEmailCampaignActionBusinessLogic
    {
        List<EmailCampaignAction> LoadAllDistributorVisible(IEmailCampaignActionRepository repository, Constants.CampaignType? campaignType = null, int? campaignID = null);
    }

    public partial interface IEmailCampaignActionBusinessLogic
    {
        List<EmailCampaignAction> LoadFullAllDistributorVisible(IEmailCampaignActionRepository repository, Constants.CampaignType? campaignType = null, int? campaignID = null);
    }
}
