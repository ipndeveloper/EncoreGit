using NetSteps.Data.Entities;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    using NetSteps.QueueProcessing.Modules.DomainEvent;

    public class GenericCampaignAccountAlertGenerator : CampaignAccountAlertGenerator
    {
        public GenericCampaignAccountAlertGenerator(DomainEventQueueItem domainEventQueueItem, AlertCampaignAction alertCampaignAction)
            : base(domainEventQueueItem, alertCampaignAction)
        {
   
        }
    }
}