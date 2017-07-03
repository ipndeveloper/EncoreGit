using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.QueueProcessing.Modules.CampaignAction.Logic
{
	[ContainerRegister(typeof(IIndividualCampaignProcessor), RegistrationBehaviors.Default)]
	public class IndividualCampaignProcessor : IIndividualCampaignProcessor
	{
		/// <summary>
		/// Processes the campaign.
		/// </summary>
		/// <param name="campaign">The campaign.</param>
		public void Process(Campaign campaign)
		{
			var campaignActions = NetSteps.Data.Entities.CampaignAction.LoadAllActiveAndNotCompletedForCampaign(campaign);
			var subscribers = CampaignSubscriber.LoadAllForCampaign(campaign);

			foreach (NetSteps.Data.Entities.CampaignAction action in campaignActions)
			{
				var actionProcessor = Create.New<IIndividualCampaignActionProcessor>();
				actionProcessor.Process(action, (Constants.CampaignType)campaign.CampaignTypeID, subscribers);
			}
		}
	}
}
