using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Auth.Common;
using Moq;
using NetSteps.Auth.Common.Model;
using NetSteps.Auth.Service.Providers;
using NetSteps.Security;

namespace NetSteps.Auth.Service.Test.Providers
{
	[TestClass]
	public class AccountIDAuthenticationProviderTest
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
		public void AccountIDAuthenticationProvider_should_authenticate_successfully()
		{
			var username = "123";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new AccountIDAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = password;
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.Success, result.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void AccountIDAuthenticationProvider_should_fail_authentication_gracefully()
		{
			var username = "123";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new AccountIDAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = "Not The Same Password";
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.InvalidPassword, result.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void AccountIDAuthenticationProvider_should_validate_that_the_accountID_is_an_integer()
		{
			var username = "BigUserName";
			var password = "I'm A Big Test Password";

			var info = GetAuthInfoObject();
			info.UserIdentifier = username;
			info.PasswordSalt = null;
			info.PasswordHash = SimpleHash.ComputeHash(password, SimpleHash.Algorithm.SHA512, null);
			info.HashAlgorithm = "SHA512";
			var store = GetAuthenticationStore(info);

			var provider = new AccountIDAuthenticationProvider(() => { return store.Object; });
			var creds = GetCredentialsObject();
			creds.UserUniqueIdentifier = username;
			creds.Password = password;
			var result = provider.Authenticate(creds);
			Assert.AreEqual((int)AuthenticationResultType.InvalidUserIdentifierFormat, result.AuthenticationResultTypeID);
		}
	}
}
