using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.BasePluginTests
{
    [TestClass]
    public abstract class BaseRewardEffectTest<TRewardEffectExtension> where TRewardEffectExtension : IPromotionRewardEffectExtension
    {
        [TestMethod]
        public virtual void BASETEST_PromotionReward_should_be_constructable_by_the_IoC()
        {
            var extension = Create.New<TRewardEffectExtension>();
            Assert.IsInstanceOfType(extension, typeof(TRewardEffectExtension));
        }

        [TestMethod]
        public virtual void BASETEST_PromotionReward_should_have_handler_registered_by_interface_name_with_dataobjectproviderregistry()
        {
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            var handler = registry.RetrieveExtensionProviderForRegisteredProvidedType(typeof(TRewardEffectExtension).ToString());
            Assert.IsNotNull(handler);
            Assert.IsTrue(typeof(IPromotionRewardHandler).IsAssignableFrom(handler.GetType()));
        }

        [TestMethod]
        public virtual void BASETEST_PromotionReward_should_have_handler_registered_by_concrete_name_with_dataobjectproviderregistry()
        {
            var extension = Create.New<TRewardEffectExtension>();
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            var handler = registry.RetrieveExtensionProviderForRegisteredProvidedType(extension.GetType().ToString());
            Assert.IsNotNull(handler);
            Assert.IsTrue(typeof(IPromotionRewardHandler).IsAssignableFrom(handler.GetType()));
        }
    }
}
