using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.TokenReplacement;

namespace NetSteps.Common.Tests.TokenReplacement
{
    [TestClass]
    public class TokenValueProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionWithNullTokensAndValues_ThrowsArgumentNullException()
        {
            var tokenValueProvider = new TokenValueProvider(null);
        }

        [TestMethod]
        public void GetKnowTokenTypes_ReturnsNoKnownTokensIfTokensAndValuesWasEmpty()
        {
            var tokensAndValues = new Dictionary<string, string>();
            var tokenValueProvider = new TokenValueProvider(tokensAndValues);

            int knownTokenCount = tokenValueProvider.GetKnownTokens().Count();

            Assert.AreEqual(knownTokenCount, 0);
        }

        [TestMethod]
        public void GetKnownTokenTypes_ReturnsKnownTokens()
        {
            var tokensAndValues = new Dictionary<string, string>()
            {
                {"Token1", "value1"},
                {"Token2", "value2"}
            };

            var tokenValueProvider = new TokenValueProvider(tokensAndValues);
            var knownTokens = tokenValueProvider.GetKnownTokens();

            Assert.IsTrue(knownTokens.Contains("Token1"));
            Assert.IsTrue(knownTokens.Contains("Token2"));
        }
    }
}
