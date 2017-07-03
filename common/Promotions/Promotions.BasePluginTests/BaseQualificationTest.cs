using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;

namespace Promotions.BasePluginTests
{
    [TestClass]
    public abstract class BaseQualificationTest<TQualificationExtension> where TQualificationExtension : IPromotionQualificationExtension
    {
        protected Func<TQualificationExtension> extensionConstructor;

        public BaseQualificationTest(Func<TQualificationExtension> constructor)
        {
            extensionConstructor = constructor;
        }

        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_have_a_nonnull_AssociatedPropertyNames_list()
        {
            var extension = Create.New<TQualificationExtension>();
            Assert.IsNotNull(extension.AssociatedPropertyNames);
        }

        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_return_valid_for_property_if_not_found_in_AssociatedPropertyNames()
        {
            var extension = Create.New<TQualificationExtension>();
            string testString = Guid.NewGuid().ToString();
            Assert.IsFalse(extension.AssociatedPropertyNames.Contains(testString));
            Assert.IsTrue(extension.ValidFor<string>(testString, testString));
        }

        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_be_constructable_by_the_IoC()
        {
            var extension = Create.New<TQualificationExtension>();
            Assert.IsInstanceOfType(extension, typeof(TQualificationExtension));
        }

        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_have_handler_registered_by_interface_name_with_dataobjectproviderregistry()
        {
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            var handler = registry.RetrieveExtensionProviderForRegisteredProvidedType(typeof(TQualificationExtension).ToString());
            Assert.IsNotNull(handler);
            Assert.IsTrue(typeof(IPromotionQualificationHandler).IsAssignableFrom(handler.GetType()));
        }

        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_have_handler_registered_by_concrete_name_with_dataobjectproviderregistry()
        {
            var extension = Create.New<TQualificationExtension>();
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            var handler = registry.RetrieveExtensionProviderForRegisteredProvidedType(extension.GetType().ToString());
            Assert.IsNotNull(handler);
            Assert.IsTrue(typeof(IPromotionQualificationHandler).IsAssignableFrom(handler.GetType()));
        }
    }
}
