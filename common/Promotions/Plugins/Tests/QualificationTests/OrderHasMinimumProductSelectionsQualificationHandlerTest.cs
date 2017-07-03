using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Tests.QualificationTests
{
    [TestClass]
    public class OrderHasMinimumProductSelectionsQualificationHandlerTest
    {
        private IContainer _container;

        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
            _container = Container.Current.MakeChildContainer();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _container = null;
        }
    }
}
