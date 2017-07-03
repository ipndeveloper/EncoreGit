using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.QueueProcessing.Modules.DomainEvent
{
    public abstract class CampaignAccountAlertGenerator
    {
        ILogger _logger;

        protected DomainEventQueueItem _domainEventQueueItem { get; private set; }
        protected AlertCampaignAction _alertCampaignAction { get; private set; }

        public CampaignAccountAlertGenerator(DomainEventQueueItem domainEventQueueItem, AlertCampaignAction alertCampaignAction)
        {
            _logger = Create.New<ILogger>();

            _domainEventQueueItem = domainEventQueueItem;
            _alertCampaignAction = alertCampaignAction;
        }

        protected virtual bool CanBeDismissed
        {
            get
            {
                return _alertCampaignAction.CanBeDismissed;
            }
        }

        public bool CreateAlert()
        {
            bool result = false;
            string logMessage = string.Empty;

            if (_alertCampaignAction != null && _alertCampaignAction.AlertTemplateID > 0)
            {
                var accountAlert = new AccountAlert
                {
                    EventContextID = _domainEventQueueItem.EventContextID,
                    AlertTemplateID = _alertCampaignAction.AlertTemplateID,
                    CanBeDismissed = CanBeDismissed
                };
                accountAlert.Save();
                logMessage = string.Format("CreateAlert - DomainEventQueueItemID: {0} - Alert Successfully Saved!", _domainEventQueueItem.DomainEventQueueItemID);
            }
            else
            {
                if (_alertCampaignAction != null && _alertCampaignAction.AlertTemplateID > 0)
                {
                    logMessage = string.Format("CreateAlert - DomainEventQueueItemID: {0} - Missing AlertTemplateID: {1}", _domainEventQueueItem.DomainEventQueueItemID, _alertCampaignAction.AlertTemplateID);
                }
                else
                {
                    logMessage = "CreateAlert - DomainEventQueueItemID: Null AlertCampaignAction";
                }
            }

            _logger.Info(logMessage);

            if (!result)
                EntityExceptionHelper.GetAndLogNetStepsException(logMessage, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);

            return result;
        }
    }
}