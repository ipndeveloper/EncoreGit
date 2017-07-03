using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common;
using NetSteps.Common.EldResolver;

namespace NetSteps.Common.Tests
{
    [TestClass]
    public class DefaultEldResolverTests
    {
        [TestMethod]
        public void EldEncode_String_WithHttpsPortPathQuery_AppendsEld()
        {
            var test = new
            {
                ELD = ".client.qa.netsteps.local",
                InputUrl = "https://workstation.client.com:81/Login?id=123",
                ExpectedUrl = "https://workstation.client.com.client.qa.netsteps.local:81/Login?id=123"
            };
            var resolver = new DefaultEldResolver(test.ELD);

            var result = resolver.EldEncode(test.InputUrl);

            Assert.AreEqual(test.ExpectedUrl, result);
        }

        [TestMethod]
        public void EldDecode_String_WithHttpsPortPathQuery_RemovesEld()
        {
            var test = new
            {
                Eld = ".client.qa.netsteps.local",
                InputUrl = "https://workstation.client.com.client.qa.netsteps.local:81/Login?id=123",
                ExpectedUrl = "https://workstation.client.com:81/Login?id=123"
            };
            var resolver = new DefaultEldResolver(test.Eld);

            var result = resolver.EldDecode(test.InputUrl);

            Assert.AreEqual(test.ExpectedUrl, result);
        }

        [TestMethod]
        public void EldEncode_String_EmptyEld_ReturnsSameUrl()
        {
            var test = new
            {
                Eld = string.Empty,
                InputUrl = "http://www.natura.com/",
                ExpectedUrl = "http://www.natura.com/"
            };
            var resolver = new DefaultEldResolver(test.Eld);

            var result = resolver.EldEncode(test.InputUrl);

            Assert.AreEqual(test.ExpectedUrl, result);
        }

        [TestMethod]
        public void EldDecode_String_EmptyEld_ReturnsSameUrl()
        {
            var test = new
            {
                Eld = string.Empty,
                InputUrl = "http://www.natura.com/",
                ExpectedUrl = "http://www.natura.com/"
            };
            var resolver = new DefaultEldResolver(test.Eld);

            var result = resolver.EldDecode(test.InputUrl);

            Assert.AreEqual(test.ExpectedUrl, result);
        }

        [TestMethod]
        public void EldDecode_Uri_LocalHost_ReturnsSameUri()
        {
            var test = new
            {
                Eld = ".client.qa.netsteps.local",
                InputUri = new Uri("http://localhost:40000/"),
                ExpectedUri = new Uri("http://localhost:40000/")
            };
            var resolver = new DefaultEldResolver(test.Eld);

            var result = resolver.EldDecode(test.InputUri);

            Assert.AreEqual(test.ExpectedUri, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EldDoesNotStartWithDot_ThrowsArgumentException()
        {
            new DefaultEldResolver("natura.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EldEndsWithDot_ThrowsArgumentException()
        {
            new DefaultEldResolver("natura.com.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EldEncode_UriBuilder_Null_ThrowsArgumentNullException()
        {
            new DefaultEldResolver(string.Empty).EldEncode((UriBuilder)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EldContainsSpace_ThrowsArgumentException()
        {
            new DefaultEldResolver(".natura .com");
        }

        [TestMethod]
        public void NonProductionUriAuthorityResolver_Append_appsetting_contains_querystring_pass()
        {
            var url = "http://www.natura.fr.local.ntf.netsteps.com/spencer/killian?spencerok=12";
            var expectedAuthority = "http://www.natura.fr.local.ntf.netsteps.com/spencer/killian?spencerok=12";
            var domainToRemove = ".local.ntf.netsteps.com";
            var uri = new Uri(url);

            var newAuthority = new DefaultEldResolver(domainToRemove).EldEncode(uri).AbsoluteUri;

            Assert.AreEqual(expectedAuthority, newAuthority);
        }
    }
}
