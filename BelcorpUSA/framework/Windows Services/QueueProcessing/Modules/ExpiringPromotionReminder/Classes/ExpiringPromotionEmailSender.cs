// -----------------------------------------------------------------------
// <copyright file="ExpiringPromotionEmailSender.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Classes
{
    using System;
    using NetSteps.Common.Interfaces;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Promotions.Common;
    using NetSteps.Promotions.Common.Model;
    using NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces;
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    [ContainerRegister(typeof(IExpiringPromotionEmailSender), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ExpiringPromotionEmailSender : CampaignEmailSender, IExpiringPromotionEmailSender
    {
        Account Account { get; set; }
        IPromotion Promotion { get; set; }

        public ExpiringPromotionEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
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


        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
        {
            if (!domainEventQueueItem.EventContext.PromotionID.HasValue)
            {
                var domainEventType = SmallCollectionCache.Instance.DomainEventTypes.GetById(domainEventQueueItem.DomainEventTypeID);
                throw new Exception(string.Format("PromotionID not set on EventContext for DomainEventType: {0} ({1}); DomainEventQueueItemID: {2}", domainEventQueueItem.DomainEventTypeID, domainEventQueueItem.DomainEventTypeID, domainEventType != null ? domainEventType.Name : string.Empty, domainEventQueueItem.DomainEventQueueItemID));
            }

            if (!domainEventQueueItem.EventContext.AccountID.HasValue)
            {
                var domainEventType = SmallCollectionCache.Instance.DomainEventTypes.GetById(domainEventQueueItem.DomainEventTypeID);
                throw new Exception(string.Format("AccountID not set on EventContext for DomainEventType: {0} ({1}); DomainEventQueueItemID: {2}", domainEventQueueItem.DomainEventTypeID, domainEventQueueItem.DomainEventTypeID, domainEventType != null ? domainEventType.Name : string.Empty, domainEventQueueItem.DomainEventQueueItemID));
            }

            if (domainEventQueueItem.EventContext.AccountID.HasValue)
                this.Account = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);
            if (domainEventQueueItem.EventContext.PromotionID.HasValue)
                this.Promotion = GetPromotion(domainEventQueueItem.EventContext.PromotionID.Value);

            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.AccountID != null)
            {
                this.Account = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);
            }
        }

        public virtual IPromotion GetPromotion(int promotionID)
        {
            var service = Create.New<IPromotionService>();

            return service.GetPromotion(promotionID);
        }

        public virtual Account GetAccount(int accountID)
        {
            return Account.Load(accountID);
        }

        public bool AccountExists()
        {
            return this.Account != null;
        }

        public bool EmailExists()
        {
            return AccountExists() && !string.IsNullOrEmpty(Account.EmailAddress);
        }

        public bool FullNameExists()
        {
            return AccountExists() && !string.IsNullOrEmpty(Account.FullName);
        }

        public bool LanguageExists()
        {
            return AccountExists() && Account.DefaultLanguageID != default(int);
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return Create.NewWithParams<IExpiringPromotionTokenProvider>(LifespanTracking.External, Param.Value(this.Account), Param.Value(this.Promotion));
        }
    }
}
