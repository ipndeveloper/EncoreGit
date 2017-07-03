using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Modules.ModuleBase;
namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class BirthdayAlertCampaignEmailSender : CampaignEmailSender
    {
   public Order InitialOrder { get; private set; }
		public Account NewAccount { get; private set; }
		public User NewAccountUser { get; private set; }

        public BirthdayAlertCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
			: base(domainEventQueueItem, emailCampaignAction)
		{
			InitializeMembers(domainEventQueueItem);
		}

		protected override string GetRecipientEmailAddress()
		{
			return EmailExists() ? NewAccount.EmailAddress : string.Empty;
		}

		protected override string GetRecipientFullName()
		{
			return FullNameExists() ? NewAccount.FullName : string.Empty;
		}

		protected override int GetRecipientLanguageID()
		{
			return LanguageExists() ? NewAccount.DefaultLanguageID : (int)Constants.Language.English;
		}

		#region Helper Methods

		private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
		{
			if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.AccountID != null)
			{
				NewAccount = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);
				if ((NewAccountUser = NewAccount.User) == null && NewAccount.UserID.GetValueOrDefault() > 0)
				{
					NewAccountUser = User.Load(NewAccount.UserID.GetValueOrDefault());
				}
			}

			if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.OrderID != null)
				InitialOrder = GetOrder(domainEventQueueItem.EventContext.OrderID.Value);
		}

		public virtual Account GetAccount(int accountID)
		{
			return Account.LoadAccountProperties(accountID);
		}

		public virtual Order GetOrder(int orderID)
		{
			return Order.Load(orderID);
		}

		public bool EnrollerExists()
		{
			return NewAccount != null;
		}

		public bool EmailExists()
		{
			return EnrollerExists() && !String.IsNullOrEmpty(NewAccount.EmailAddress);
		}

		public bool FullNameExists()
		{
			return EnrollerExists() && !String.IsNullOrEmpty(NewAccount.FullName);
		}

		public bool LanguageExists()
		{
			return EnrollerExists() && NewAccount.DefaultLanguageID != default(int);
		}

		#endregion

		protected override ITokenValueProvider GetTokenValueProvider()
		{
			using (var create = Create.SharedOrNewContainer())
			{
				return create.NewWithParams<IConsultantEnrolledTokenValueProvider>(LifespanTracking.External, Param.Value(InitialOrder), Param.Value(NewAccount), Param.Value(NewAccountUser));
			}
		}
}
}
