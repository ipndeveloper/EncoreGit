using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Tests.PromotionKinds
{
    [TestClass]
    public class ProductPromotionFlatDiscountTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

		// This should be fleshed out into components... no time.....
        [TestMethod]
        public void ProductPromotionFlatDiscountTest_should_do_something_good()
        {
            using (var container = Create.NewContainer())
            {
                var randomizer = new Random();
                decimal adjustmentAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                decimal initialAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                var productID = randomizer.Next();
                var marketID = randomizer.Next();
				var priceType = Create.New<IPriceTypeService>().GetPriceType(1);

                var promotion = new ProductPromotionFlatDiscount();
				promotion.AddProductAdjustment(productID, priceType, adjustmentAmount);
				Assert.AreEqual(adjustmentAmount, promotion.GetAdjustedValue(productID, initialAmount, priceType));
            }
        }
    }
}
