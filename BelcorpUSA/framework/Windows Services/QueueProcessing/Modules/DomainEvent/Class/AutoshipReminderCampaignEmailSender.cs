using System;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class AutoshipReminderCampaignEmailSender : CampaignEmailSender
    {
        public AutoshipOrder AutoshipOrder { get; set; }

        public Account Account
        {
            get { return AutoshipOrder.Account; }
        }

        public AutoshipReminderCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            InitializeMembers(domainEventQueueItem);
        }

        protected override string GetRecipientEmailAddress()
        {
            return EmailAddressExists() ? Account.EmailAddress : string.Empty;
        }

        protected override string GetRecipientFullName()
        {
            return FullNameExists() ? Account.FullName : string.Empty;
        }

        protected override int GetRecipientLanguageID()
        {
            return LanguageIDExists() ? Account.DefaultLanguageID : (int)Constants.Language.English;
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            string url;

            if (Account.AccountTypeID == (short)Constants.AccountType.Distributor)
            {
                url = DistributorWorkstationUrl();
            }
            else
            {
                url = GetPwsUrl();
            }

            return new AutoshipReminderTokenValueProvider(AutoshipOrder, url);
        }



        #region Helper Methods

        private string DistributorWorkstationUrl()
        {
            string workstationUrl = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.DistributorWorkstationUrl, "");
            string url = string.Format("{0}/Account/Autoships/Edit/{1}", workstationUrl, AutoshipOrder.AutoshipOrderID);
            return url;
        }

        private string GetPwsUrl()
        {
            string domain = ConfigurationManager.AppSettings["Domains"];
            string url = string.Format("http://loyalcustomers.{0}/Home", domain);

            return url;
        }

        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
        {
            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.OrderID != null)
            {
                AutoshipOrder = GetAutoshipOrder(domainEventQueueItem.EventContext.OrderID.Value);
            }
        }

        public virtual bool LanguageIDExists()
        {
            return ValidAutoshipOrder() && Account.DefaultLanguageID != default(int);
        }

        public virtual bool FullNameExists()
        {
            return ValidAutoshipOrder() && !String.IsNullOrWhiteSpace(Account.FullName);
        }

        public virtual bool EmailAddressExists()
        {
            return ValidAutoshipOrder() && !String.IsNullOrEmpty(Account.EmailAddress);
        }


        public virtual bool ValidAutoshipOrder()
        {
            return AutoshipOrder != null && AutoshipOrder.Account != null;
        }

        public virtual AutoshipOrder GetAutoshipOrder(int orderID)
        {
            return AutoshipOrder.LoadFullByOrderID(orderID);
        }

        #endregion
    }
}
