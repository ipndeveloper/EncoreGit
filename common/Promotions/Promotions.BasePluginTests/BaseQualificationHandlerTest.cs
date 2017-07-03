using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.BasePluginTests
{
    [TestClass]
    public abstract class BaseQualificationHandlerTest<TQualificationHandler> where TQualificationHandler : IPromotionQualificationHandler
    {
        [TestMethod]
        public virtual void BASETEST_PromotionQualification_should_be_constructable_by_the_IoC()
        {
            var extension = Create.New<TQualificationHandler>();
            Assert.IsInstanceOfType(extension, typeof(TQualificationHandler));
        }

        [TestMethod]
        public virtual void BASETEST_QualificationHandler_should_be_registered_with_data_object_extension_provider()
        {
            var extension = Create.New<TQualificationHandler>();
            if (extension is IDataObjectExtensionProvider)
            {
                var extensionHandler = extension as IDataObjectExtensionProvider;
                Assert.IsFalse(string.IsNullOrEmpty(extensionHandler.GetProviderKey()));
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var fromRegistry = registry.RetrieveExtensionProvider(extensionHandler.GetProviderKey());
            }
            else
            {
                Assert.Fail(String.Format("Qualification handler {0} is not an IDataObjectExtensionProvider.", typeof(TQualificationHandler)));
            }
        }
    }
}
