using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	[TestClass]
	public class VertexProxyTests
	{
		private static long keygen { get { return DateTime.UtcNow.Ticks; } }

		// Disabled due to invalid credentials. This test is for development use.
		[Ignore]
		[TestMethod]
		public void CalculateTaxProxyQuotation_ReturnsTaxForCA()
		{
			var order = CreateTestCAOrder();
			var results = CalculateTaxServiceProxy.Quote(order);
			Assert.IsTrue(results.Status == TaxCalculationState.Succeeded);
			Assert.IsTrue(results.Order.Customers.SelectMany(c => c.Items).Any() && results.Order.Customers.SelectMany(c => c.Items).All(i => i.Taxes != null));
			var tax = results.Order.Customers.SelectMany(c => c.Items).First().Taxes;
			Assert.IsTrue(tax.First().CalculatedTax > 0);
		}

		[TestMethod]
		public void CalculateTaxProxy_ReturnsSkippedWhenNoCountryCodeIsSet()
		{
			var order = CreateTestCAOrder();
			order.CountryCode = null;
			var results = CalculateTaxServiceProxy.Quote(order);
			Assert.IsTrue(results.Status == TaxCalculationState.Skipped);
		}


		#region Helpers
		public ITaxOrder CreateTestCAOrder()
		{
			var order = Create.New<ITaxOrder>();
			order.CountryCode = "CA";
			order.Customers = new List<ITaxCustomer> { CreateTestCustomer() };
			order.OrderID = "testusord"+keygen;
			var shippingAddr = CreateCAShippingAddress();
			order.Customers[0].ShippingAddress = shippingAddr;
			order.Customers[0].Items = new List<ITaxOrderItem> { CreateTestItem(shippingAddr) };

			return order;
		}

		public ITaxAddress CreateOriginAddress()
		{
			var addr = Create.New<ITaxAddress>();
			addr.City = Create.New<ITaxAddressProperty>();
			addr.Country = Create.New<ITaxAddressProperty>();
			addr.MainDivision = Create.New<ITaxAddressProperty>();
			addr.SubDivision = Create.New<ITaxAddressProperty>();
			addr.PostalCode = Create.New<ITaxAddressProperty>();
			addr.StreetAddress1 = "6205 S Arizona Ave";
			addr.City.Kind = AddressPropertyKind.City;
			addr.City.Name = "Chandler";
			addr.Country.Kind = AddressPropertyKind.Country;
			addr.Country.Name = "United States";
			addr.Country.Code = "US";
			addr.MainDivision.Kind = AddressPropertyKind.MainDivision;
			addr.MainDivision.Name = "AB";
			addr.SubDivision.Kind = AddressPropertyKind.SubDivision;
			addr.PostalCode.Kind = AddressPropertyKind.PostalCode;
			addr.PostalCode.Name = "85248";
			addr.PostalCode.Code = "85248";
			return addr;
		}

		//public ITaxAddress CreateUSShippingAddress()
		//{
		//    var addr = Create.New<ITaxAddress>();
		//    addr.City = Create.New<ITaxAddressProperty>();
		//    addr.Country = Create.New<ITaxAddressProperty>();
		//    addr.MainDivision = Create.New<ITaxAddressProperty>();
		//    addr.SubDivision = Create.New<ITaxAddressProperty>();
		//    addr.PostalCode = Create.New<ITaxAddressProperty>();
		//    addr.StreetAddress1 = "6205 S Arizona Ave";
		//    addr.City.Kind = AddressPropertyKind.City;
		//    addr.City.Name = "Chandler";
		//    addr.Country.Kind = AddressPropertyKind.Country;
		//    addr.Country.Name = "United States";
		//    addr.Country.Code = "US";
		//    addr.MainDivision.Kind = AddressPropertyKind.MainDivision;
		//    addr.MainDivision.Name = "AB";
		//    addr.SubDivision.Kind = AddressPropertyKind.SubDivision;
		//    addr.PostalCode.Kind = AddressPropertyKind.PostalCode;
		//    addr.PostalCode.Name = "T0G0S0";
		//    addr.PostalCode.Code = "T0G0S0";
		//    return addr;
		//}

		public ITaxAddress CreateCAShippingAddress()
		{
			var addr = Create.New<ITaxAddress>();
			addr.City = Create.New<ITaxAddressProperty>();
			addr.Country = Create.New<ITaxAddressProperty>();
			addr.MainDivision = Create.New<ITaxAddressProperty>();
			addr.SubDivision = Create.New<ITaxAddressProperty>();
			addr.PostalCode = Create.New<ITaxAddressProperty>();
			addr.StreetAddress1 = "RR1";
			addr.City.Kind = AddressPropertyKind.City;
			addr.City.Name = "Dapp";
			addr.Country.Kind = AddressPropertyKind.Country;
			addr.Country.Name = "Canada";
			addr.Country.Code = "CA";
			addr.MainDivision.Kind = AddressPropertyKind.MainDivision;
			addr.MainDivision.Name = "AB";
			addr.SubDivision.Kind = AddressPropertyKind.SubDivision;
			addr.PostalCode.Kind = AddressPropertyKind.PostalCode;
			addr.PostalCode.Name = "T0G0S0";
			addr.PostalCode.Code = "T0G0S0";
			return addr;
		}

		public ITaxOrderItem CreateTestItem(ITaxAddress shippingAddr)
		{
			var item = Create.New<ITaxOrderItem>();
			item.ProductCode = "XYZ";
			item.ItemID = "testitem" + keygen;
			item.Quantity = 1;
			//item.TaxableTotal = 18;
			item.UnitPrice = 15;
			item.ShippingFee = 3;
			item.ShippingAddress = shippingAddr;
			return item;
		}

		public ITaxCustomer CreateTestCustomer()
		{
			var cust = Create.New<ITaxCustomer>();
			cust.CustomerID = "testcust" + keygen;
			cust.IsTaxExempt = false;
			return cust;
		}

		public ITaxOrder CreateTestUSOrder()
		{
			var order = Create.New<ITaxOrder>();
			order.CountryCode = "US";
			order.Customers = new List<ITaxCustomer>();
			order.Customers[0].Items = new List<ITaxOrderItem>();
			order.OrderID = "12346";
			return order;
		}
		#endregion
	}
}
