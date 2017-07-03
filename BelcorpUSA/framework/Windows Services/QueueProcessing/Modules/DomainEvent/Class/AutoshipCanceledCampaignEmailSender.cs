using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class AutoshipCanceledCampaignEmailSender : CampaignEmailSender
    {
        public AutoshipOrder AutoshipOrder { get; set; }
        public bool BccToSponsor { get; set; }
        private readonly Account _sponsor;

        public Account Account
        {
            get { return AutoshipOrder.Account; }
        }

        public AutoshipCanceledCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction, bool bccToSponsor = false)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            InitializeMembers(domainEventQueueItem, bccToSponsor);

            _sponsor = SetAccountSponsor(AutoshipOrder, bccToSponsor);
        }

        protected override string GetRecipientEmailAddress()
        {
            if (BccToSponsor)
            {
                return EmailAddressExists(_sponsor) ? _sponsor.EmailAddress : string.Empty;
            }
            else
            {
                return EmailAddressExists(Account) ? Account.EmailAddress : string.Empty;
            }
        }

        protected override string GetRecipientFullName()
        {
            if (BccToSponsor)
            {
                return FullNameExists(_sponsor) ? _sponsor.FullName : string.Empty;
            }
            else
            {
                return FullNameExists(Account) ? Account.FullName : string.Empty;
            }
        }

        protected override int GetRecipientLanguageID()
        {
            if (BccToSponsor)
            {
                return LanguageIDExists(_sponsor) ? _sponsor.DefaultLanguageID : (int)Constants.Language.English;
            }
            else
            {
                return LanguageIDExists(Account) ? Account.DefaultLanguageID : (int)Constants.Language.English;
            }
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            string url = Account.AccountTypeID == (short)Constants.AccountType.Distributor
                             ? DistributorWorkstationUrl()
                             : GetPwsUrl();

            return new AutoshipCanceledTokenValueProvider(AutoshipOrder, url);
        }

        #region Helpers

        public Account SetAccountSponsor(AutoshipOrder autoshipOrder, bool bccToSponsor)
        {
            Account account = new Account();

            if (bccToSponsor)
            {
                if (autoshipOrder.Account.Sponsor.IsNotNull())
                    account = autoshipOrder.Account.Sponsor;

                else if (autoshipOrder.Account.SponsorID.HasValue)
                    account = Account.Load(autoshipOrder.Account.SponsorID.Value);
            }

            return account;
        }

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

        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem, bool bccToSponsor)
        {
            BccToSponsor = bccToSponsor;

            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.OrderID != null)
            {
                AutoshipOrder = GetAutoshipOrder(domainEventQueueItem.EventContext.OrderID.Value);
            }
        }

        public virtual AutoshipOrder GetAutoshipOrder(int orderID)
        {
            return AutoshipOrder.LoadFullByOrderID(orderID);
        }

        public virtual bool LanguageIDExists(Account account)
        {
            return ValidAutoshipOrder() && account.DefaultLanguageID != default(int);
        }

        public virtual bool FullNameExists(Account account)
        {
            return ValidAutoshipOrder() && !string.IsNullOrWhiteSpace(account.FullName);
        }

        public virtual bool EmailAddressExists(Account account)
        {
            return ValidAutoshipOrder() && !string.IsNullOrEmpty(account.EmailAddress);
        }

        public virtual bool ValidAutoshipOrder()
        {
            return AutoshipOrder != null && AutoshipOrder.Account != null;
        }

        #endregion
    }
}
