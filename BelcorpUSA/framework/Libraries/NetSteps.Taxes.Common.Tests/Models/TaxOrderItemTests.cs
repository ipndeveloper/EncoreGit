using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common.Tests.Models
{
    [TestClass]
    public class TaxOrderItemTests
    {
        [TestMethod]
        public void GetCalculatedTaxByLevel_SeemsToWork()
        {
            var item = Create.New<ITaxOrderItem>();
            var taxes = new List<ITaxOrderItemTaxes>();
            item.UnitPrice = 100m;
            item.Taxes = taxes;
			item.Quantity = 1;

            taxes.Add(Create.Mutation(Create.New<ITaxOrderItemTaxes>(), 
                t => {
                    t.TaxableAmount = item.UnitPrice;
                    t.Jurisdiction = Create.Mutation(Create.New<IJurisdiction>(),
                        j => {
                            j.Level = JurisdictionLevel.State;
                            j.Name = "Utah";
                        });
                    t.TaxRule = "Normal";
                    t.EffectiveRate = 0.06m;
                    t.CalculatedTax = t.TaxableAmount * t.EffectiveRate;
                }));
            taxes.Add(Create.Mutation(Create.New<ITaxOrderItemTaxes>(),
                 t =>
                 {
                     t.TaxableAmount = item.UnitPrice;
                     t.Jurisdiction = Create.Mutation(Create.New<IJurisdiction>(),
                         j =>
                         {
                             j.Level = JurisdictionLevel.District;
                             j.Name = "Mass Transit District";
                         });
                     t.TaxRule = "Transit";
                     t.EffectiveRate = 0.00125m;
                     t.CalculatedTax = t.TaxableAmount * t.EffectiveRate;
                 }));

            Assert.AreEqual(6m, item.GetCalculatedTaxByLevel(JurisdictionLevel.State));
			Assert.AreEqual(0.125m, item.GetCalculatedTaxByLevel(JurisdictionLevel.District));
			Assert.AreEqual(0m, item.GetCalculatedTaxByLevel(JurisdictionLevel.County));
			Assert.AreEqual(0m, item.GetCalculatedTaxByLevel(JurisdictionLevel.City));
			Assert.AreEqual(6.125m, item.GetCalculatedTax());
        }
    }
}
