using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Security;
using System.Globalization;

namespace NetSteps.Security.Tests
{
	[TestClass]
	public class EncryptionTests
	{
		[TestMethod]
		public void Encryption_Constructor_InstantiatesWithoutError()
		{
			var encryption = new NetSteps.Security.Encryption();
		}

		[TestMethod]
		public void Encryption_SingleSignOnSalt_ShouldReturnUtcDate()
		{
			var expected = DateTime.UtcNow.ToString("dddd, MMMM dd, yyyy", new CultureInfo("en-US"));
			var actual = Encryption.SingleSignOnSalt;

			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("This result is inconclusive since we have no testing wrapper for DateTime.  A failing result is conclusive, however.");
		}
	}
}
