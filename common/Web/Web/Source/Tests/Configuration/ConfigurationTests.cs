using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Web;

namespace NetSteps.Web.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void Configuration_Constructor_InstantiatesWithoutError()
        {
            var configuration = new NetSteps.Web.Configuration();
        }
    }
}
