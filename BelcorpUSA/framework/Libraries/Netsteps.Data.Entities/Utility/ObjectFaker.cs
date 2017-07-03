using System;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.DataFaker;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{

	/// <summary>
	/// Author: John Egbert
	/// Description: Class to return object with fake data for testing.
	/// Created: 02-05-2010
	/// </summary>
	public class FakeObjects
	{
		public static Order GetFakeOrder()
		{
			Account account = Account.GetRandomRecordFull();
			return GetFakeOrder(account, true);
		}

		public static Order GetFakeOrder(Account account, bool autoAddItem)
		{

			var name = account.ToDataFakerNameObject();
			Address mainAddress = account.Addresses.GetDefaultByTypeID(Generated.ConstantsGenerated.AddressType.Main);
			if (mainAddress == null)
			{
				mainAddress = GetFakeIAddress(name) as Address;
			}

			var order = new Order(account);
			order.OrderPendingState = Constants.OrderPendingStates.Quote;
			OrderCustomer orderCustomer = order.OrderCustomers[0];
			order.SetDefaultsWithAccountValues(orderCustomer);

			// Add Order Item
			if (autoAddItem)
			{
				var availableProducts = GetAllAvailableProducts(account.AccountTypeID, order);

				for (int i = 0; i < NetSteps.Common.Random.Next(1, 5); i++)
				{
					var product = availableProducts.GetRandom();
					order.AddItem(product, 1);
				}
			}

			var orderService = Create.New<IOrderService>();
			var orderContext = Create.New<IOrderContext>();
			orderContext.Order = order;
			orderService.UpdateOrder(orderContext);

			order = orderContext.Order.AsOrder();

			// Only add Address and Payment if there is not already one there from the Account Default for Testing - JHE
			if (orderCustomer.OrderPayments.Count == 0)
			{
				OrderPayment orderPayment = GetFakeOrderPayment(name, mainAddress);
				orderPayment.PaymentTypeID = Constants.PaymentType.CreditCard.ToInt();
				orderPayment.OrderID = order.OrderID;
				orderCustomer.OrderPayments.Add(orderPayment);
			}
			else
			{
				orderCustomer.OrderPayments[0].DecryptedAccountNumber = MiscFaker.GetCreditCard();
				orderCustomer.OrderPayments[0].ExpirationDate = MiscFaker.GetExpirationDate();

				var billingAddress = orderCustomer.OrderPayments[0] as IAddress;
				if (billingAddress.PostalCode.IsNullOrEmpty())
				{
					Address.CopyPropertiesTo(mainAddress, orderCustomer.OrderPayments[0]);
				}
			}

			// Add Order Shipment
			OrderShipment orderShipment = order.GetDefaultShipmentNoDefault();
			if (orderShipment == null || orderShipment.IsEmpty())
			{
				if (orderShipment == null)
				{
					orderShipment = new OrderShipment();
					orderShipment.StartEntityTracking();
				}

				if (orderShipment.IsEmpty())
				{
					var newOrderShipment = GetFakeOrderShipment(name, mainAddress);
					orderShipment.OrderShipmentStatusID = Constants.OrderShipmentStatus.Pending.ToShort();
					Address.CopyPropertiesTo(newOrderShipment, orderShipment);

					order.UpdateOrderShipmentAddress(orderShipment, newOrderShipment);
					order.UpdateOrderShipmentAddressAndDefaultShipping(newOrderShipment);
				}
			}
			return order;
		}

		private static System.Collections.Generic.List<Product> GetAllAvailableProducts(short accountTypeID, Order order)
		{
			var inventory = Create.New<InventoryBaseRepository>();
			int catalogId = 1;
			var availableProducts = inventory.GetActiveProductsForCatalog(1, catalogId);
			while (availableProducts.Count == 0)
			{
				availableProducts = inventory.GetActiveProductsForCatalog(1, ++catalogId);
			}
			availableProducts = availableProducts.Where(p => p.ContainsPrice(accountTypeID, Constants.PriceRelationshipType.Commissions, order.CurrencyID, order.OrderTypeID)).ToList();
			return availableProducts;
		}

		public static OrderPayment GetFakeOrderPayment(Name name, IAddress billingAddress)
		{
			var orderPayment = new OrderPayment()
			{
				Amount = MiscFaker.OrderDollarAmount(),
				OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending
			};

			Reflection.CopyPropertiesDynamic<IPayment, IPayment>(GetFakeIPayment(name, billingAddress), orderPayment);
			Address.CopyPropertiesTo(billingAddress, orderPayment);
			orderPayment.CurrencyID = Constants.Currency.UsDollar.ToInt();
			return orderPayment;
		}

		public static OrderShipment GetFakeOrderShipment(Name name, Address billingAddress)
		{
			var orderShipment = new OrderShipment()
			{
				Email = InternetFaker.Email(name),
				DayPhone = PhoneFaker.Phone(),
				EveningPhone = PhoneFaker.Phone(),
				TrackingNumber = MiscFaker.GetTrackingNumber(),
				DateShipped = DateTime.Now,
				GovernmentReceiptNumber = string.Empty,
				OrderShipmentStatusID = 3
			};
			Address.CopyPropertiesTo(billingAddress, orderShipment);
			return orderShipment;
		}

		public static IPayment GetFakeIPayment(Name name, IAddress billingAddress)
		{
			var payment = new Payment()
			{
				DecryptedAccountNumber = MiscFaker.GetCreditCard(),
				ExpirationDate = MiscFaker.GetExpirationDate(),
				NameOnCard = name.FirstAndLastName,
				BillingFirstName = name.FirstName,
				BillingLastName = name.LastName,
				BillingAddress = billingAddress,
				CVV = "123"
			};
			(payment as IPayment).PaymentTypeID = Constants.PaymentType.CreditCard.ToInt();

			return payment;
		}

		public static IAddress GetFakeIAddress(Name name)
		{
			int countryId = Constants.Country.UnitedStates.ToInt();

			// TODO: Return a more accurate address with Postal code lookup later - JHE
			var address = new Address();
			address.StartTracking();

			address.FirstName = name.FirstName;
			address.LastName = name.LastName;
			address.Name = name.FirstAndLastName;
			address.Attention = name.FirstAndLastName;
			address.Address1 = LocationFaker.StreetName();
			address.Address2 = string.Empty;
			address.Address3 = string.Empty;
			address.City = LocationFaker.City();
			address.County = "Utah";
			address.State = "UT";
			address.StateProvinceID = 56;
			address.PostalCode = "84058";
			address.PhoneNumber = PhoneFaker.Phone();
			address.CountryID = countryId;

			// Get actual random address data from TaxCache table - JHE
			var taxcache = TaxCache.Repository.GetRandomRecord(address.CountryID);
			address.PostalCode = taxcache.PostalCode;
			address.City = taxcache.City;
			address.State = taxcache.StateAbbreviation;
			var state = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(s => s.CountryID == address.CountryID && s.StateAbbreviation.ToCleanString() == taxcache.StateAbbreviation.ToCleanString());
			if (state != null)
			{
				address.StateProvinceID = state.StateProvinceID;
			}

			address.County = taxcache.County;
			address.AddressTypeID = Constants.AddressType.Main.GetRandom<Constants.AddressType>(true).ToShort();

			return address;
		}

		public static MailMessage GetTestMailMessage()
		{
			var name = new Name()
			{
				FirstName = "John",
				LastName = "Egbert",
				IsMale = true
			};

			var mailAccount = new MailMessage();
			mailAccount.To.Add(new MailMessageRecipient()
			{
				Name = name.FirstAndLastName,
				Email = "johnegbert@yahoo.com",
				MailMessageRecipientType = NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual
			});

			mailAccount.FromAddress = "test@netsteps.com";
			mailAccount.FromNickName = "Test";
			mailAccount.Subject = "Test Email";
			mailAccount.Body = LoremIpsum.GetParagraphs(1, true);
			mailAccount.HTMLBody = mailAccount.Body;

			return mailAccount;
		}
	}
}
