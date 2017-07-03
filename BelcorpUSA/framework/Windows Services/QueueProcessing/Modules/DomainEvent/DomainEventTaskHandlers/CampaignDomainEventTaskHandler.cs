using NetSteps.Data.Entities;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.DomainEventTaskHandlers
{
	using NetSteps.QueueProcessing.Modules.DomainEvent;
	using NetSteps.QueueProcessing.Modules.ModuleBase;

	public class CampaignDomainEventTaskHandler : DomainEventTaskHandlerBase
	{
		private DomainEventQueueProcessor.CampaignDomainEventTaskHandlerHelper _helper;

		internal CampaignDomainEventTaskHandler(DomainEventQueueProcessor.CampaignDomainEventTaskHandlerHelper helper)
		{
			_helper = helper;
		}

		protected override bool Run(DomainEventQueueItem domainEventQueueItem)
		{
			this.Logger.Debug("running CampaignDomainEventTaskHandler on domainEventQueueItem {0}", domainEventQueueItem.DomainEventQueueItemID);

			var campaign = GetCampignByDomainEvent(domainEventQueueItem);

			bool processedSuccessfully = false;

			if (campaign != null)
			{
				processedSuccessfully = ProcessCampaignActions(domainEventQueueItem, campaign);
			}
			else
			{
				Logger.Error("DomainEventQueueProcessor.ProcessQueueItem Campaign is null for DomainEventQueueItemID {0}.", domainEventQueueItem.DomainEventQueueItemID);
			}

			return processedSuccessfully;
		}

		protected virtual bool ProcessCampaignActions(DomainEventQueueItem domainEventQueueItem, Campaign campaign)
		{
			return _helper.ProcessCampaignActions(domainEventQueueItem, campaign);
		}

		protected virtual bool ProcessCampaignAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			return _helper.ProcessCampaignAction(domainEventQueueItem, action);
		}

		protected virtual Campaign GetCampignByDomainEvent(DomainEventQueueItem domainEventQueueItem)
		{
			return _helper.GetCampignByDomainEvent(domainEventQueueItem);
		}

		protected virtual bool ProcessEmailAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			return _helper.ProcessEmailAction(domainEventQueueItem, action);
		}

		protected virtual bool ProcessAlertAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			return _helper.ProcessAlertAction(domainEventQueueItem, action);
		}

		protected virtual CampaignAccountAlertGenerator GetCampaignAlertSender(DomainEventQueueItem domainEventQueueItem, AlertCampaignAction alertCampaignAction)
		{
			return _helper.GetCampaignAlertSender(domainEventQueueItem, alertCampaignAction);
		}

		protected virtual ICampaignEmailSender GetCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
		{
			return _helper.GetCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
		}
	}
}