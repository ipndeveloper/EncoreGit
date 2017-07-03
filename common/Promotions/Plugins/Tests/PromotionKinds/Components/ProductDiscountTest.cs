using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;

namespace NetSteps.Promotions.Plugins.Tests.PromotionKinds.Components
{
    [TestClass]
    public class ProductDiscountTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void ProductAdjustment_should_be_wired_up_with_the_IOC()
        {
            using (var container = Create.NewContainer())
            {
                var discount = Create.New<IProductAdjustment>();
                Assert.IsNotNull(discount);
            }
        }
    }
}
