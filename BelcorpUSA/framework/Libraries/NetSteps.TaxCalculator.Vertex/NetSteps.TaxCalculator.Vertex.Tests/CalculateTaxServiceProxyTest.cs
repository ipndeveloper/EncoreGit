using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	[TestClass]
	public class CalculateTaxServiceProxyTest
	{
		#region CalculateTaxServiceProxy.Lookup Tests
		
		[TestMethod]
		public void Lookup_Succeeds_When_Item_Has_Zero_Taxable_Total()
		{
			var taxOrder = GetDefaultTaxOrder();
			taxOrder.Customers[0].Items[0].UnitPrice = 0;

			var response = CalculateTaxServiceProxy.Quote(taxOrder);

			Assert.IsTrue(response.Status == TaxCalculationState.Skipped);
		}

		

		#endregion

		#region Private Methods

		private static ITaxOrder GetDefaultTaxOrder()
		{
			var order = Create.New<ITaxOrder>();
			order.Status = TaxOrderState.Complete;
			order.OrderID = "0";
			var customer = Create.New<ITaxCustomer>();
			customer.CustomerID = "0";
			customer.IsTaxExempt = false;
			order.Customers = new List<ITaxCustomer> { customer };
			var address = Create.New<ITestAddress>();
			address.Address1 = "123";
			address.City = "Pleasant Grove";
			address.State = "UT";
			address.PostalCode = "84062";
			var taxAddress = address.ToTaxAddress();
			var item = Create.New<ITaxOrderItem>();
			item.HandlingFee = 0;
			item.ItemID = "0";
			item.OriginAddress = taxAddress;
			item.ProductCode = "0";
			item.Quantity = 1;
			item.ShippingAddress = taxAddress;
			item.ShippingFee = 3.00m;
			//item.TaxableTotal = 6.00m;
			item.UnitPrice = 6.00m;
			customer.Items = new List<ITaxOrderItem> { item };

			return order;
		}

		#endregion
	}
}
