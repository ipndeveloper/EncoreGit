using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Common.Extensions;
using NetSteps.Testing.Core;

namespace NetSteps.Common.Tests.Extensions
{
    [TestClass]
    public class HttpRequestExtensionsTests
    {
        [TestMethod]
        public void CurrentSiteUrl_NullHttpContext_ReturnsEmptyString()
        {
            HttpContextBase httpContext = null;
            string expected = string.Empty;

            var actual = HttpContextExtensions.CurrentSiteUrl(httpContext);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentSiteUrl_ValidUrl_ReturnsUrl()
        {
            var httpContext = MockHelpers.FakeHttpContextBase();
            var httpRequest = Mock.Get<HttpRequestBase>(httpContext.Request);
            httpRequest.Setup(x => x.Url).Returns(new Uri("http://foo.com/foo"));
            httpRequest.Setup(x => x.ApplicationPath).Returns("/");
            string expected = "http://foo.com/";

            var actual = HttpContextExtensions.CurrentSiteUrl(httpContext);

            Assert.AreEqual(expected, actual);
        }
    }
}
