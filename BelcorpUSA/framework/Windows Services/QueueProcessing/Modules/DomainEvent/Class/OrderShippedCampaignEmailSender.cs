using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
	public class OrderShippedCampaignEmailSender : CampaignEmailSender
	{
		public Order CurrentOrder { get; set; }
		public OrderShipment OrderShipment { get; set; }
		public OrderCustomer OrderCustomer
		{
			get
			{
				if (CurrentOrder == null)
				{
					return null;
				}
				return CurrentOrder.OrderCustomers.FirstOrDefault();
			}
		}

		protected Account _account;
		public virtual Account Account
		{
			get
			{
				try
				{
					if (OrderCustomer == null || (_account != null && this.OrderCustomer != null && _account.AccountID != this.OrderCustomer.AccountID))
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

		public OrderShippedCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
			: base(domainEventQueueItem, emailCampaignAction)
		{
			InitializeMembers(domainEventQueueItem);
		}

		protected override string GetRecipientEmailAddress()
		{
			// I would change this to be a list but the base campaign sender would need some work;  I'm adding in the list as a semicolon
			// delimited string.
			if (!EmailAddressExists())
				return String.Empty;
			else
			{
				StringBuilder address = new StringBuilder();
				if (_account != null && !String.IsNullOrEmpty(_account.EmailAddress))
					address.Append(_account.EmailAddress);

				//Moved to GetAdditionalRecipientEmailAddresses()
				//OrderShipment shipment = _order.GetDefaultShipment();
				//if (shipment != null && !String.IsNullOrEmpty(shipment.Email))
				//{
				//    if (address.Length > 0)
				//        address.Append(';');
				//    address.Append(shipment.Email);
				//}


				return address.ToString();
			}
		}

		protected override IEnumerable<string> GetAdditionalRecipientEmailAddresses()
		{
			OrderShipment shipment = CurrentOrder.GetDefaultShipment();
			if (shipment != null && !String.IsNullOrEmpty(shipment.Email))
			{
				yield return shipment.Email;
			}
		}

		public bool EmailAddressExists()
		{
			bool accountEmailExists = _account != null && !String.IsNullOrEmpty(_account.EmailAddress);

			bool orderEmailExists = false;
			if (CurrentOrder == null)
				orderEmailExists = false;
			else
			{
				OrderShipment shipment = CurrentOrder.GetDefaultShipment();
				orderEmailExists = shipment != null && !string.IsNullOrEmpty(shipment.Email);
			}

			return accountEmailExists | orderEmailExists;
		}

		protected override string GetRecipientFullName()
		{
			return FullNameExists() ? Account.FullName : string.Empty;
		}

		protected override int GetRecipientLanguageID()
		{
			return LanguageIDExists() ? Account.DefaultLanguageID : (int)Constants.Language.English;
		}

		#region Helper Methods

		private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
		{
			if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.OrderID.HasValue)
			{
				CurrentOrder = GetOrder(domainEventQueueItem.EventContext.OrderID.Value);
				OrderShipment = GetOrderShipment();
			}
		}

		public virtual Order GetOrder(int orderID)
		{
			return Order.LoadFull(orderID);
		}

		public virtual OrderShipment GetOrderShipment()
		{
			return CurrentOrder.OrderShipments.FirstOrDefault(o => o.OrderShipmentStatusID == (short)Constants.OrderShipmentStatus.Shipped);
		}

		public virtual bool OrderCustomerExists()
		{
			return CurrentOrder != null && CurrentOrder.OrderCustomers != null && CurrentOrder.OrderCustomers.Any();
		}

		public virtual bool FullNameExists()
		{
			return this.Account != null && !String.IsNullOrWhiteSpace(Account.FullName);
		}

		public virtual bool LanguageIDExists()
		{
			return this.Account != null && Account.DefaultLanguageID != default(int);
		}

		#endregion

		protected override ITokenValueProvider GetTokenValueProvider()
		{
			return Create.NewWithParams<OrderShippedTokenValueProvider>(
				LifespanTracking.External,
				Param.Value(CurrentOrder),
				Param.Value(OrderShipment),
				Param.Value(GetRecipientLanguageID())
			);
		}
	}
}
