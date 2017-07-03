using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.TokenReplacement;

namespace NetSteps.Common.Tests.TokenReplacement
{
	[TestClass]
	public class TokenReplacerTests
	{
		private const string BEGIN_TOKEN_DELIMITER = "{{";
		private const string END_TOKEN_DELIMITER = "}}";

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructionWithNullTokenValueProvider_ThrowsNullArgumentException()
		{
			var tokenReplacer = new TokenReplacer(null, BEGIN_TOKEN_DELIMITER, END_TOKEN_DELIMITER);
		}

		[TestMethod]
		public void FindUknownTokens_FindsUnknownTokens()
		{
			string tokenizedString = "abcde{{KnownToken}}fghijklmnopqrst{{UnknownToken}}uvwxyz";
			var tokensAndValues = new Dictionary<string, string>() { { "KnownToken", "somevalue" } };
			var tokenValueProvider = new TokenValueProvider(tokensAndValues);
			var tokenReplacer = new TokenReplacer(tokenValueProvider, "{{", "}}");

			var unknownTokens = tokenReplacer.FindUnknownTokens(tokenizedString);

			Assert.IsTrue(unknownTokens.Contains("UnknownToken"));
		}

		[TestMethod]
		public void ReplaceTokens_ReplacesTokens()
		{
			string tokenizedString = "abc{{Token1}}def{{Token2}}ghi";
			var tokensAndValues = new Dictionary<string, string>()
			{
				{ "Token1", "12345" },
				{ "Token2", "67890" }
			};
			var tokenValueProvider = new TokenValueProvider(tokensAndValues);
			var tokenReplacer = new TokenReplacer(tokenValueProvider, "{{", "}}");

			var replacedString = tokenReplacer.ReplaceTokens(tokenizedString);

			Assert.AreEqual("abc12345def67890ghi", replacedString);
		}

		[TestMethod]
		public void ReplaceTokens_ReplacesTokens_WithDefault()
		{
			string tokenizedString = "abc{{Token1}}def{{Token2||DEFAULT}}ghi";
			var tokensAndValues = new Dictionary<string, string>()
			{
				{ "Token1", "12345" },
				{ "Token2", null }
			};
			var tokenValueProvider = new TokenValueProvider(tokensAndValues);
			var tokenReplacer = new TokenReplacer(tokenValueProvider, "{{", "}}");

			var replacedString = tokenReplacer.ReplaceTokens(tokenizedString);

			Assert.AreEqual("abc12345defDEFAULTghi", replacedString);
		}
	}
}
