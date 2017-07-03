using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common.Tests.Models
{
    [TestClass]
    public class TaxOrderTests
    {
        [TestMethod]
        public void GetCalculatedTaxByLevel_SeemsToWork()
        {
            var order = Create.New<ITaxOrder>();
            order.Customers = new List<ITaxCustomer>();
            order.Customers.Add(Create.Mutation(Create.New<ITaxCustomer>(),
                c =>
                {
                    c.IsTaxExempt = false;
                    c.CustomerID = "CUSTOMER001";
                }));
            order.OrderID = "TEST001";
            order.Customers[0].Items = new List<ITaxOrderItem>();
            
            var item = Create.New<ITaxOrderItem>();
            item.OriginAddress = Create.Mutation(Create.New<ITaxAddress>(),
                a =>
                {
                    a.PostalCode = TaxAddressProperty.FromPostalCode("89103");
                });
            item.ShippingAddress = Create.Mutation(Create.New<ITaxAddress>(),
                a =>
                {
                    a.PostalCode = TaxAddressProperty.FromPostalCode("84103");
                });
            item.ItemID = "ITEM001";
            item.ProductCode = "TEST001";
            item.Quantity = 1;
            item.UnitPrice = 100m;
            
            order.Customers[0].Items.Add(item);


        }

    }
}
