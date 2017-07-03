using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Validation.Common;

namespace NetSteps.Validation.Handlers.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ValidationHandlers_should_be_registered_by_the_record_kind_they_handle()
        {
            WireupCoordinator.SelfConfigure();
        }

        private void ValidateHandlerRegistration<THandlerConcrete>(string recordKind)
        {
            var handler = Create.NewNamed<IRecordPropertyCalculationHandler>(recordKind);
            Assert.IsNotNull(handler);
            Assert.IsInstanceOfType(handler, typeof(THandlerConcrete));
        }
    }
}
