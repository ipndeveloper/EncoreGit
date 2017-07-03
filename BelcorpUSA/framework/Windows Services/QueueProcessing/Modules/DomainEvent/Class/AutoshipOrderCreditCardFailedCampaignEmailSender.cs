using System;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class AutoshipOrderCreditCardFailedCampaignEmailSender : CampaignEmailSender
    {
        public OrderPayment OrderPayment { get; set; }
        public OrderCustomer OrderCustomer { get; set; }

        protected Account _account;
        public virtual Account Account
        {
            get
            {
                try
                {
                    if (this.OrderCustomer == null || (_account != null && _account.AccountID != this.OrderCustomer.AccountID))
                    {
                        _account = null;
                    }
                    if (_account == null && this.OrderCustomer != null && this.OrderCustomer.AccountID != 0)
                    {
                        _account = Account.Load(this.OrderCustomer.AccountID);
                    }
                    return _account;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                _account = value;
            }
        }

        public AutoshipOrderCreditCardFailedCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
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
            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.OrderID.HasValue)
            {
                var currentOrder = GetOrder(domainEventQueueItem.EventContext.OrderID.Value);

                if (currentOrder != null && currentOrder.OrderID != default(int))
                {
                    OrderCustomer = currentOrder.OrderCustomers.First();
                    OrderPayment = OrderCustomer.OrderPayments.FirstOrDefault(x => x.OrderID == currentOrder.OrderID);
                }
            }
        }

        public virtual Order GetOrder(int orderID)
        {
            return Order.LoadFull(orderID);
        }

        public bool EmailExists()
        {
            return Account != null && !String.IsNullOrEmpty(Account.EmailAddress);
        }

        public bool FullNameExists()
        {
            return Account != null && !String.IsNullOrWhiteSpace(Account.FullName);
        }

        public bool LanguageExists()
        {
            return Account != null && Account.DefaultLanguageID != default(int);
        }

        #endregion



        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return new AutoshipOrderFailedTokenValueProvider(OrderPayment,
                            ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.DistributorWorkstationUrl, ""));
        }
    }
}
