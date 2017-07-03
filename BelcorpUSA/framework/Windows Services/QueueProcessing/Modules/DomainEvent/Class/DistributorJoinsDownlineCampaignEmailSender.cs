using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
	public class DistributorJoinsDownlineCampaignEmailSender : CampaignEmailSender
	{
		public Order InitialOrder { get; set; }
		public Account Enroller { get; set; }
		public Account NewAccount { get; set; }
		public Account Sponsor { get; set; }


		public DistributorJoinsDownlineCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
			: base(domainEventQueueItem, emailCampaignAction)
		{
			InitializeMembers(domainEventQueueItem);
		}


		protected override string GetRecipientEmailAddress()
		{
            return EmailExists() ? Sponsor.EmailAddress : string.Empty;
		}

		protected override string GetRecipientFullName()
		{
            return FullNameExists() ? Sponsor.FullName : string.Empty;
		}

		protected override int GetRecipientLanguageID()
		{
			return LanguageExists() ? Enroller.DefaultLanguageID : (int)Constants.Language.English;
		}

		#region Helper Methods

		private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
		{
			if (ValidValues(domainEventQueueItem))
			{
				NewAccount = GetAccount(domainEventQueueItem.EventContext.AccountID.Value);

				InitialOrder = GetOrder(domainEventQueueItem.EventContext.OrderID.Value);

				Enroller = GetAccount(NewAccount.EnrollerID.Value);

				Sponsor = GetAccount(NewAccount.SponsorID.Value);
			}
		}

		public virtual bool ValidValues(DomainEventQueueItem domainEventQueueItem)
		{
            if (domainEventQueueItem.EventContext.OrderID.HasValue == false)
            {
                domainEventQueueItem.EventContext.OrderID = 0;
            }
			return (domainEventQueueItem.EventContext != null
						&& domainEventQueueItem.EventContext.OrderID.HasValue
						&& domainEventQueueItem.EventContext.AccountID.HasValue);
		}

		public virtual Account GetAccount(int accountID)
		{
			return Account.Load(accountID);
		}

		public virtual Order GetOrder(int orderID)
		{
            if (orderID == 0)
            {
                return Order.Load(1);
            }
            else 
            {
                return Order.Load(orderID);
            }
		}

		public bool EnrollerExists()
		{
			return Enroller != null;
		}

		public bool EmailExists()
		{
			return EnrollerExists() && !String.IsNullOrEmpty(Enroller.EmailAddress);
		}

		public bool FullNameExists()
		{
			return EnrollerExists() && !String.IsNullOrWhiteSpace(Enroller.FullName);
		}

		public bool LanguageExists()
		{
			return EnrollerExists() && Enroller.DefaultLanguageID != default(int);
		}
		#endregion

		protected override ITokenValueProvider GetTokenValueProvider()
		{
			return Create.NewWithParams<DistributorJoinsDownlineTokenValueProvider>(LifespanTracking.External,
			                                                                        Param.Value(InitialOrder),
			                                                                        Param.Value(NewAccount), Param.Value(Sponsor));
		}
	}
}
