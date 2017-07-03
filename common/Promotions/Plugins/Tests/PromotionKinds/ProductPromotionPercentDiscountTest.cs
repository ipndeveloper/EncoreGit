using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Tests.PromotionKinds
{
    [TestClass]
    public class ProductPromotionPercentDiscountTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void ProductPromotionPercentDiscount_should_calculate_adjustments_correctly()
        {
            using (var container = Create.NewContainer())
            {
                var randomizer = new Random();
                decimal adjustmentAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                decimal initialAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                var productID = randomizer.Next();
                var marketID = randomizer.Next();
				var priceType = Create.New<IPriceTypeService>().GetPriceType(1);

                var promotion = new ProductPromotionPercentDiscount();
                promotion.DefaultMarketID = randomizer.Next();
                promotion.AddProductAdjustment(productID, priceType, marketID, adjustmentAmount);
                
                Assert.AreEqual(adjustmentAmount * initialAmount, promotion.GetAdjustedValue(productID, initialAmount, marketID, priceType));
            }
        }

        [TestMethod]
        public void ProductPromotionPercentDiscount_should_place_adjustment_amounts_with_pricetype_and_market()
        {
            using (var container = Create.NewContainer())
            {
                var randomizer = new Random();
                decimal adjustmentAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                decimal initialAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                var productID = randomizer.Next();
                var marketID = randomizer.Next();
				var priceType = Create.New<IPriceTypeService>().GetPriceType(1);

                var promotion = new ProductPromotionPercentDiscount();
                promotion.DefaultMarketID = randomizer.Next();
				promotion.AddProductAdjustment(productID, priceType, marketID, adjustmentAmount);

                Assert.IsTrue(promotion.PromotedProductIDs.Contains(productID));
				Assert.IsTrue(promotion.GetPromotedPriceTypesForProductID(productID).Contains(priceType));
				Assert.IsTrue(promotion.GetMarketsForProductIDAndPriceType(productID, priceType).Contains(marketID));
				Assert.AreEqual(adjustmentAmount, promotion.GetAdjustmentAmount(productID, priceType, marketID));
            }
        }

        [TestMethod]
        public void ProductPromotionPercentDiscount_should_replace_adjustment_amounts_with_equal_pricetype_and_market()
        {
            using (var container = Create.NewContainer())
            {
                var randomizer = new Random();
                decimal adjustmentAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                decimal initialAmount = randomizer.Next() + Convert.ToDecimal(randomizer.NextDouble());
                var productID = randomizer.Next();
                var marketID = randomizer.Next();
				var priceType = Create.New<IPriceTypeService>().GetPriceType(1);

                var promotion = new ProductPromotionPercentDiscount();
                promotion.DefaultMarketID = randomizer.Next();
                promotion.AddProductAdjustment(productID, priceType, marketID, adjustmentAmount);

                Assert.IsTrue(promotion.PromotedProductIDs.Contains(productID));
                Assert.IsTrue(promotion.GetPromotedPriceTypesForProductID(productID).Contains(priceType));
                Assert.IsTrue(promotion.GetMarketsForProductIDAndPriceType(productID, priceType).Contains(marketID));
                Assert.AreEqual(adjustmentAmount, promotion.GetAdjustmentAmount(productID, priceType, marketID));
            }
        }
    }
}
