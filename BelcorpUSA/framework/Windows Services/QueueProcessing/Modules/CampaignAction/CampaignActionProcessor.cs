using System;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Modules.CampaignAction.Logic;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.CampaignAction
{
    public class CampaignActionProcessor : CampaignActionBaseProcessor<Data.Entities.CampaignAction>
    {
        public static readonly string CProcessorName = "CampaignActionProcessor";

        public CampaignActionProcessor()
        {
            Name = CProcessorName;
        }

		/// <summary>
		/// Creates the queue items.
		/// </summary>
		/// <param name="maxNumberToPoll">The max number to poll.</param>
		public override void CreateQueueItems(int maxNumberToPoll)
		{
			//Get Newsletters that are past due, load their subscribers, and queue up a CampaignActionQueueItem to send each email
			try
			{
				Logger.Info("CampaignActionProcessor - Start");

				//We need to be careful about caching campaignActionItems this becasue we need to have a realtime look into
				//the campaignAction QueueStatus to make sure we dont do work on Campaign Actions being run by other processes. SOK

				var campaigns = Campaign.LoadAll();

				foreach (Campaign campaign in campaigns.Where(c => c.Active))
				{
					var individualCampaignProcessor = Create.New<IIndividualCampaignProcessor>();
					individualCampaignProcessor.Process(campaign);
				}

				Logger.Info("CampaignActionProcessor - End");
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Processes the queue item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void ProcessQueueItem(NetSteps.Data.Entities.CampaignAction item)
		{
			throw new System.NotImplementedException();
		}

    }
}
