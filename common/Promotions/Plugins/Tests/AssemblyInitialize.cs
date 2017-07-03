using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Tests
{
    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            Container.Root.ForType<IPriceTypeService>()
                .Register<IPriceTypeService>((c,p) => { return getMockPriceTypeService(); })
                .ResolveAnInstancePerRequest()
                .End();
        }

        private static IPriceTypeService getMockPriceTypeService()
        {
            var service = new Mock<IPriceTypeService>();
            return service.Object;
        }


    }
}
