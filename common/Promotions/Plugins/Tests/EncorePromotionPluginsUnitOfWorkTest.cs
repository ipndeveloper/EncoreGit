using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.Plugins.Test
{
    [TestClass]
    public class EncorePromotionPluginsUnitOfWorkTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }
        private void mockWireup()
        {
        }


        [TestMethod]
        public void IEncorePromotionsPluginsUnitOfWork_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();// test the IOC registration to verify that we've got it correct
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));
            }
        }
    }
}
