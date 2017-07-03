using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class OrderStatusCampaignEmailSender : CampaignEmailSender
    {
        protected Order _order;
        protected OrderCustomer _orderCustomer
        {
            get
            {
                if (_order == null)
                {
                    return null;
                }
                return _order.OrderCustomers.FirstOrDefault();
            }
        }
        private Account _accountField;
        protected Account _account
        {
            get
            {
                try
                {
                    if (_orderCustomer == null || (_accountField != null && this._orderCustomer != null && _accountField.AccountID != this._orderCustomer.AccountID))
                    {
                        _accountField = null;
                    }
                    if (_accountField == null && this._orderCustomer != null && this._orderCustomer.AccountID != 0)
                    {
                        _accountField = Account.Load(this._orderCustomer.AccountID);
                    }
                    return _accountField;
                }
                catch
                {
                    return null;
                }
            }
        }

        public OrderStatusCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            if (domainEventQueueItem.EventContext.OrderID.HasValue)
                _order = Order.LoadFull(domainEventQueueItem.EventContext.OrderID.Value);
            else
                throw new ArgumentException(
                    string.Format("The EventContext for DomainEventQueueItem {0} does not specify a valid OrderID",
                                  domainEventQueueItem.EventContextID));
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
            OrderShipment shipment = _order.GetDefaultShipment();
            if (shipment != null && !String.IsNullOrEmpty(shipment.Email))
            {
                yield return shipment.Email;
            }
        }

        private bool EmailAddressExists()
        {
            bool accountEmailExists = _account != null && !String.IsNullOrEmpty(_account.EmailAddress);

            OrderShipment shipment = _order.GetDefaultShipment();
            bool orderEmailExists = shipment != null && !string.IsNullOrEmpty(shipment.Email);

            return accountEmailExists | orderEmailExists;
        }

        protected override string GetRecipientFullName()
        {
            return _account != null ? _account.FullName : "";
        }

        protected override int GetRecipientLanguageID()
        {
            return _account != null ? _account.DefaultLanguageID : 0;
        }

        protected override ITokenValueProvider GetTokenValueProvider()
        {
            return new OrderStatusTokenValueProvider(_order);
        }
    }
}
