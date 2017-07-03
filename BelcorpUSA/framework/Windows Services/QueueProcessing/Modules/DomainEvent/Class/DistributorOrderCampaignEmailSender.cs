using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    class DistributorOrderCampaignEmailSender : CampaignEmailSender
    {
        private Order CurrentOrder { get; set; }

        public DistributorOrderCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            int orderID = domainEventQueueItem.EventContext.OrderID.HasValue ? domainEventQueueItem.EventContext.OrderID.Value : 0;

            CurrentOrder = Order.LoadFull(orderID);

            if (CurrentOrder == null || CurrentOrder.OrderID == 0)
            {
                //Throw Exception
            }
            else
            {
                CurrentOrder.Consultant = Account.Load(CurrentOrder.ConsultantID);
            }
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return new OrderTokenValueProvider(CurrentOrder);
        }

        protected override string GetRecipientEmailAddress()
        {
            return CurrentOrder.Consultant.EmailAddress;
        }

        protected override int GetRecipientLanguageID()
        {
            return CurrentOrder.Consultant.DefaultLanguageID;
        }

        protected override string GetRecipientFullName()
        {
            return CurrentOrder.Consultant.FullName;
        }
    }
}
