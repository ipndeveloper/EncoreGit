using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class CampaignActionQueueItemBusinessLogic
    {
        public override List<string> ValidatedChildPropertiesSetByParent(Repositories.ICampaignActionQueueItemRepository repository)
        {
            List<string> list = new List<string>() { "CampaignActionQueueItemID", "EventContextID" };
            return list;
        }
    }
}

