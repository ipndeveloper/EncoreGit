using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Auth.Service.Providers;
using NetSteps.Security;
using Moq;
using NetSteps.Auth.Common.Model;
using NetSteps.Auth.Common;

namespace NetSteps.Auth.Service.Test.Providers
{
	[TestClass]
	public class EmailAddressAuthenticationProviderTest
	{
		private Mock<IAuthenticationStore> GetAuthenticationStore(IUserAuthInfo infoToBeReturned)
		{
			var mock = new Mock<IAuthenticationStore>();
			mock.Setup(store => store.GetUserAuthInfo(It.IsAny<string>(), It.IsAny<AuthenticationStoreField>())).Returns(infoToBeReturned);
			return mock;
		}

		private IUserAuthInfo GetAuthInfoObject()
		{
			var mockUserInfo = new Mock<IUserAuthInfo>();
			mockUserInfo.SetupAllProperties();
			return mockUserInfo.Object;
		}

		private ICredentials GetCredentialsObject()
		{
			var mockCreds = new Mock<ICredentials>();
			mockCreds.SetupAllProperties();
			return mockCreds.Object;
		}

		[TestMethod]
		public void EmailAddressAuthenticationProvider_should_authenticate_successfully()
		{
			var username = "carolwhite@netsteps.com";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new EmailAddressAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = "Not The Same Password";
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.InvalidPassword, result.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void EmailAddressAuthenticationProvider_should_fail_authentication_gracefully()
		{
			var username = "carolwhite@netsteps.com";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new EmailAddressAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = "Not The Same Password";
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.InvalidPassword, result.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void EmailAddressAuthenticationProvider_should_validate_that_the_useridentifier_is_a_validly_formed_email_address()
		{
			var username = "123";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new EmailAddressAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = "Not The Same Password";
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.InvalidUserIdentifierFormat, result.AuthenticationResultTypeID);
		}
	}
}
