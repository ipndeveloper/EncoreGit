using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.TokenValueProviders
{
	[ContainerRegister(typeof(NetSteps.Data.Entities.TokenValueProviders.OrderShippedTokenValueProvider), RegistrationBehaviors.DefaultOrOverrideDefault, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class OrderShippedTokenValueProvider : ITokenValueProvider
	{
		private const string ORDER_NUMBER = "OrderNumber";
		private const string ORDER_DATE_SHIPPED = "OrderDateShipped";
		private const string ORDER_DATE_PLACED = "OrderDatePlaced";
		private const string ORDER_DISTRIBUTOR_FULL_NAME = "DistributorFullName";
		private const string ORDER_SHIPPING_ADDRESS = "OrderShippingAddress";
		private const string ORDER_BILLING_ADDRESS = "OrderBillingAddress";
		private const string ORDER_SHIPPING_METHOD = "OrderShippingMethod";
		private const string ORDER_ITEMS = "OrderItems";
		private const string ORDER_TOTALS = "OrderTotals";
		private const string ORDER_TRACKING_NUMBER = "OrderTrackingNumber";

		private string[] _knownTokens = new string[] { 
			ORDER_BILLING_ADDRESS, 
			ORDER_DATE_PLACED, 
			ORDER_DATE_SHIPPED,
			ORDER_DISTRIBUTOR_FULL_NAME,
			ORDER_ITEMS,
			ORDER_NUMBER,
			ORDER_SHIPPING_ADDRESS,
			ORDER_SHIPPING_METHOD,
			ORDER_TOTALS,
			ORDER_TRACKING_NUMBER
		};

		protected readonly Order _currentOrder;
		protected readonly OrderShipment _orderShipment;
		protected int _languageID;

		public OrderShippedTokenValueProvider(Order currentOrder, OrderShipment orderShipment, int languageID = 1)
		{
			_currentOrder = currentOrder;
			_orderShipment = orderShipment;
			_languageID = languageID;
		}

		public virtual IEnumerable<string> GetKnownTokens()
		{
			return _knownTokens;
		}

		public virtual string GetTokenValue(string token)
		{
			switch (token)
			{
				case ORDER_BILLING_ADDRESS:
					if (_currentOrder != null)
					{
						var customer = _currentOrder.OrderCustomers.FirstOrDefault();
						if (customer != null)
						{
							var account = Account.LoadFull(customer.AccountID);
							Address billing;
                            if (account != null && account.Addresses != null && (account.Addresses.FirstOrDefault(a => a.AddressTypeID == (int)Constants.AddressType.Billing)) != null)
                            {
                                billing = account.Addresses.FirstOrDefault(a => a.AddressTypeID == (int)Constants.AddressType.Billing);
								IAddress bAddress = billing as IAddress;
								if (bAddress != null)
								{
									return bAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.SingleLine);
								}
							}
						}
					}
					return String.Empty;

				case ORDER_DATE_PLACED:
					return _currentOrder != null && _currentOrder.CompleteDate.HasValue ? _currentOrder.CompleteDate.ToString() : String.Empty;

				case ORDER_NUMBER:
					return _currentOrder != null ? _currentOrder.OrderNumber : String.Empty;

				case ORDER_DISTRIBUTOR_FULL_NAME:
					if (_currentOrder != null)
					{
                        Account con = _currentOrder.Consultant;
                        if (con != null || Account.LoadFull(_currentOrder.ConsultantID) != null)
                        {
                            con = Account.LoadFull(_currentOrder.ConsultantID);
                            return con.FullName;
                        }
					}

					return String.Empty;


				case ORDER_ITEMS:
					if (_orderShipment != null)
					{
						var items = _orderShipment.GetOrderItems();
						if (items != null && items.Any())
						{

							return string.Join("\r\n", items.Select(oi => String.Format("{0}: {1} - {2}, {3}: {4}", Translation.GetTerm(_languageID, "Product"), oi.SKU, oi.ProductName, Translation.GetTerm(_languageID, "Quantity"), oi.Quantity)).ToArray());
						}
					}
					return String.Empty;

				case ORDER_DATE_SHIPPED:
					DateTime dateShipped = DateTime.Now;
					if (_orderShipment != null)
					{
						if (_orderShipment.DateShippedUTC.HasValue && _orderShipment.DateShippedUTC.Value > DateTime.MinValue)
						{
							return _orderShipment.DateShippedUTC.ToString();
						}
						if (_orderShipment.OrderShipmentPackages.Any())
						{
							return _orderShipment.OrderShipmentPackages.FirstOrDefault().DateShippedUTC.ToString();
						}
					}
					return String.Empty;

				case ORDER_SHIPPING_ADDRESS:
					if (_orderShipment != null)
					{
						IAddress sAddress = _orderShipment as IAddress;
						if (sAddress != null)
						{
							return sAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.SingleLine);
						}
					}
					return String.Empty;

				case ORDER_SHIPPING_METHOD:
					if (_orderShipment != null)
					{
						return _orderShipment.ShippingMethodName;
					}
					return String.Empty;

				case ORDER_TOTALS:
					if (_currentOrder != null && _currentOrder.GrandTotal.HasValue)
					{
						return _currentOrder.GrandTotal.ToString(_currentOrder.CurrencyID);
					}
					return String.Empty;

				case ORDER_TRACKING_NUMBER:
					if (_orderShipment != null)
					{
						return _orderShipment.TrackingNumber;
					}
					return String.Empty;

				default:
					return String.Empty;
			}
		}
	}
}
