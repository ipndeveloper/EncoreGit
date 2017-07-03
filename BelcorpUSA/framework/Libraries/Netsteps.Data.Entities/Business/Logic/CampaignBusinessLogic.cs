using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class CampaignBusinessLogic
    {
        public override List<string> ValidatedChildPropertiesSetByParent(Repositories.ICampaignRepository repository)
        {
            List<string> list = new List<string>() { "CampaignID", "CampaignActionID", "EmailTemplateID", "AlertCampaignActionID", "AlertTemplateID" };
            return list;
        }
    }
}

