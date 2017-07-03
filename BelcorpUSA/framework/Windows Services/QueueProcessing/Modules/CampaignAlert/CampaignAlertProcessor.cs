using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.CampaignAlert
{
    public class CampaignAlertProcessor : QueueProcessor<int>
    {
        public static readonly string CProcessorName = "CampaignAlertProcessor";

        public CampaignAlertProcessor()
        {
            Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToEnqueue)
        {
            //CampaignAlert CampaignAlert = new CampaignAlert();
            //CampaignAlert.FindAndCreateAccountTasks();
        }

        public override void ProcessQueueItem(int item)
        {
            // no items are ever queued for processing. This is queue processor
            // is being used as a timer to have a process run on a regular interval
        }
    }
}
