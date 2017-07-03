using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class ProspectCreatedCampaignEmailSender : CampaignEmailSender
    {
        public Account Consultant { get; set; }
        public Account Prospect { get; set; }

        public ProspectCreatedCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            InitializeMembers(domainEventQueueItem);
        }

        protected override string GetRecipientEmailAddress()
        {
            return EmailExists() ? Consultant.EmailAddress : string.Empty;
        }

        protected override string GetRecipientFullName()
        {
            return FullNameExists() ? Consultant.FullName : string.Empty;
        }

        protected override int GetRecipientLanguageID()
        {
            return LanguageExists() ? Consultant.DefaultLanguageID : (int)Constants.Language.English;
        }

        #region Helper Methods

        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
        {
            if (ValidValues(domainEventQueueItem))
            {
                Prospect = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);

                Consultant = GetAccount(Prospect.SponsorID.Value);
            }
        }

        public virtual bool ValidValues(DomainEventQueueItem domainEventQueueItem)
        {
            return (domainEventQueueItem.EventContext != null
                        && domainEventQueueItem.EventContext.AccountID.HasValue);
        }

        public virtual Account GetAccount(int accountID)
        {
            return Account.Load(accountID);
        }

        public bool EnrollerExists()
        {
            return Consultant != null;
        }

        public bool EmailExists()
        {
            return EnrollerExists() && !String.IsNullOrEmpty(Consultant.EmailAddress);
        }

        public bool FullNameExists()
        {
            return EnrollerExists() && !String.IsNullOrWhiteSpace(Consultant.FullName);
        }

        public bool LanguageExists()
        {
            return EnrollerExists() && Consultant.DefaultLanguageID != default(int);
        }
        #endregion


        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return new ProspectCreatedValueProvider(Prospect, Consultant);
        }
    }
}
