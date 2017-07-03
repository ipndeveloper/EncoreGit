using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    public class AccountCampaignEmailSender : CampaignEmailSender
    {
        public Account Account { get; private set; }

        public AccountCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            InitializeMembers(domainEventQueueItem);
        }

        protected override string GetRecipientEmailAddress()
        {
            return EmailExists() ? Account.EmailAddress : string.Empty;
        }

        protected override string GetRecipientFullName()
        {
            return FullNameExists() ? Account.FullName : string.Empty;
        }

        protected override int GetRecipientLanguageID()
        {
            return LanguageExists() ? Account.DefaultLanguageID : (int)Constants.Language.English;
        }

        #region Helper Methods

        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
        {
            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.AccountID != null)
                Account = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);
        }

        public virtual Account GetAccount(int accountID)
        {
            return Account.Load(accountID);
        }

        public virtual Order GetOrder(int orderID)
        {
            return Order.Load(orderID);
        }

        public bool EnrollerExists()
        {
            return Account != null;
        }

        public bool EmailExists()
        {
            return EnrollerExists() && !String.IsNullOrEmpty(Account.EmailAddress);
        }

        public bool FullNameExists()
        {
            return EnrollerExists() && !String.IsNullOrEmpty(Account.FullName);
        }

        public bool LanguageExists()
        {
            return EnrollerExists() && Account.DefaultLanguageID != default(int);
        }

        #endregion

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return new AccountTokenValueProvider(Account);
        }
    }
}
