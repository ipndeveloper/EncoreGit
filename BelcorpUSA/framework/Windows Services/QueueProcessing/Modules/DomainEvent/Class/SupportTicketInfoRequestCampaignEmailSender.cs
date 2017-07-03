using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    class SupportTicketInfoRequestCampaignEmailSender : CampaignEmailSender
    {
        private SupportTicket _ticket;

        public SupportTicketInfoRequestCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            //TODO
            _ticket = SupportTicket.LoadFull((int)domainEventQueueItem.EventContext.SupportTicketID);
        }

        protected override string GetRecipientEmailAddress()
        {
            return _ticket.Account.EmailAddress;
        }

        protected override string GetRecipientFullName()
        {
            return _ticket.Account.FullName;
        }

        protected override int GetRecipientLanguageID()
        {
            return _ticket.Account.DefaultLanguageID;
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            var distributorWorkstationUrl = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.DistributorWorkstationUrl, "");
            return new SupportTicketTokenValueProvider(_ticket, distributorWorkstationUrl);
        }
    }
}
