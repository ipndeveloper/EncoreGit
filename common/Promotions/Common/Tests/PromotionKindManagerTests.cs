using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using Moq;
using NetSteps.Promotions.Common.CoreImplementations;

namespace NetSteps.Promotions.Common.Tests
{
    
    [TestClass]
    public class PromotionKindManagerTests
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

        private void mockWireup(string mockPromotionKind)
        {
            var promotionMock = new Mock<IMockPromotion>();
            promotionMock.SetupAllProperties();
            promotionMock.SetupGet(x => x.PromotionKind).Returns(mockPromotionKind);

            Container.Root.Registry.ForType<IMockPromotion>()
                .Register<IMockPromotion>((c, p) => { return promotionMock.Object; })
                .ResolveAnInstancePerRequest()
                .End();

        }

        public interface IMockPromotion : IPromotion
        {
            
        }

        [TestMethod]
        public void PromotionKindManager_should_not_allow_null_promotion_kinds()
        {
            using (var container = Create.NewContainer())
            {
                var mockPromotionKind = Guid.NewGuid().ToString();

                mockWireup(mockPromotionKind); 

				bool exceptionThrown = false;
                try
                {
                    var manager = new PromotionKindManager();
                    var create = manager.CreatePromotion(null);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains("Assertion failed"));
                    exceptionThrown = true;
                }
                if (!exceptionThrown)
                    Assert.Fail("Failed to produce exception on attempting to register a null promotion kind.");
            }
        }

        [TestMethod]
        public void PromotionKindManager_should_resolve_an_generic_promotion_to_the_generic_Promotion_kind()
        {
            using (var container = Create.NewContainer())
            {
                var mockPromotionKind = Guid.NewGuid().ToString();
                
                mockWireup(mockPromotionKind);

                var manager = new PromotionKindManager();
                manager.RegisterPromotionKind<BasicPromotion>(BasicPromotion.PromotionKindName);

                var create = manager.CreatePromotion(BasicPromotion.PromotionKindName);
                Assert.IsInstanceOfType(create, typeof(BasicPromotion));
            }
        }

        [TestMethod]
        public void PromotionKindManager_should_resolve_a_specific_promotion_kind()
        {
            using (var container = Create.NewContainer())
            {
                var mockPromotionKind = Guid.NewGuid().ToString();

                mockWireup(mockPromotionKind); 
                
                var manager = new PromotionKindManager();
                manager.RegisterPromotionKind<IMockPromotion>(mockPromotionKind);

                var created = manager.CreatePromotion(mockPromotionKind);
                Assert.AreEqual(created.PromotionKind, mockPromotionKind);
            }
        }
    }
}
