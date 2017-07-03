using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;

namespace NetSteps.Common.Tests.TokenReplacement
{
    [TestClass]
    public class CompositeTokenValueProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionWithNullTokenValueProviders_ThrowsArgumentException()
        {
            var compositeTokenValueProvider = new CompositeTokenValueProvider(null);
        }

        [TestMethod]
        public void ReplaceTokensWithTwoProviders_GetsTokenValuesFromBothProviders()
        {
            var provider1 = new TokenValueProvider(new Dictionary<string, string>() { { "Token1", "123" } });
            var provider2 = new TokenValueProvider(new Dictionary<string, string>() { { "Token2", "456" } });
            var compositeProvider = new CompositeTokenValueProvider(provider1, provider2);
            var tokenReplacer = new TokenReplacer(compositeProvider, "{{", "}}");
            string tokenizedText = "abc{{Token1}}def{{Token2}}ghi";

            string replacedText = tokenReplacer.ReplaceTokens(tokenizedText);

            Assert.AreEqual("abc123def456ghi", replacedText);
        }
    }
}
