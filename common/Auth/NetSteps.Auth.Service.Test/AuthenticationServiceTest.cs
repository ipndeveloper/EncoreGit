using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Auth.Common;
using Moq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Auth.Common.Model;
using NetSteps.Auth.Common.Configuration;

namespace NetSteps.Auth.Service.Test
{
	[TestClass]
	public class AuthenticationServiceTest
	{
		private class authConfig
		{
			internal string providerName { get; set; }
			internal AuthenticationResultType desiredAuthResult { get; set; }
			internal PasswordRetrievalResultType desiredPasswordRetrieveResult { get; set; }
			internal string exceptionMessage { get; set; }
		}

		private Mock<ICredentials> createCredentials()
		{
			var cred = new Mock<ICredentials>();
			cred.SetupAllProperties();
			return cred;
		}

		private Mock<IProviderAuthenticationResult> createProviderAuthenticateResponse(string providerName, AuthenticationResultType desiredResult)
		{
			var result = new Mock<IProviderAuthenticationResult>();
			result.Setup(res => res.ProviderName).Returns(providerName);
			result.Setup(res => res.AuthenticationResultTypeID).Returns((int)desiredResult);
			return result;
		}

		private Mock<IProviderPasswordRetrievalResult> createProviderPasswordRetrievalResponse(string providerName, PasswordRetrievalResultType desiredResult)
		{
			var result = new Mock<IProviderPasswordRetrievalResult>();
			result.Setup(res => res.ProviderName).Returns(providerName);
			result.Setup(res => res.PasswordRetrievalResultTypeID).Returns((int)desiredResult);
			return result;
		}

		private Mock<IAuthenticationProvider> createProviderMock(authConfig providerConfig)
		{
			var provider = new Mock<IAuthenticationProvider>();
			provider.Setup(prov => prov.GetProviderName()).Returns(providerConfig.providerName);
			
			// authenticate wireup
			if (providerConfig.desiredAuthResult == AuthenticationResultType.ProviderException)
			{
				provider.Setup(prov => prov.Authenticate(It.IsAny<ICredentials>())).Throws(new Exception(providerConfig.exceptionMessage));
			}
			else
			{
				var result = createProviderAuthenticateResponse(providerConfig.providerName, providerConfig.desiredAuthResult);
				provider.Setup(prov => prov.Authenticate(It.IsAny<ICredentials>())).Returns(result.Object);
			}

			// retrieveaccount wireup
			if (providerConfig.desiredPasswordRetrieveResult == PasswordRetrievalResultType.ProviderException)
			{
				provider.Setup(prov => prov.RetrieveAccount(It.IsAny<IPartialCredentials>())).Throws(new Exception(providerConfig.exceptionMessage));
			}
			else
			{
				var result = createProviderPasswordRetrievalResponse(providerConfig.providerName, providerConfig.desiredPasswordRetrieveResult);
				provider.Setup(prov => prov.RetrieveAccount(It.IsAny<IPartialCredentials>())).Returns(result.Object);
			}
	
			return provider;
		}

		private Mock<IAuthenticationProviderManager> createProviderManagerMock()
		{
			return createProviderManagerMock(new authConfig[] { });
		}

		private Mock<IAuthenticationProviderManager> createProviderManagerMock(authConfig config)
		{
			return createProviderManagerMock(new authConfig[] { config });
		}

		private Mock<IAuthenticationProviderManager> createProviderManagerMock(IEnumerable<authConfig> configs)
		{
			var manager = new Mock<IAuthenticationProviderManager>();
			List<IAuthenticationProvider> providers = new List<IAuthenticationProvider>();
			foreach (var config in configs)
			{
				providers.Add(createProviderMock(config).Object);
			}
			manager.Setup(mang => mang.GetProviders()).Returns(providers);
			manager.Setup(mang => mang.GetRegisteredProviderNames()).Returns(providers.Select(p => p.GetProviderName()));
            var settingsDictionary = new Dictionary<string, bool>();
            settingsDictionary.Add(DefaultAdminSettingKinds.EnableForgotPassword, true);
            manager.SetupGet<IDictionary<string, bool>>(mang => mang.AdminSettings).Returns(settingsDictionary);
			return manager;
		}

		[TestMethod]
		public void AuthenticationService_should_authenticate_with_mock_provider_returning_Success()
		{
			var config = new authConfig() 
							{ 
							desiredAuthResult = AuthenticationResultType.Success, 
							providerName = "MockAuthProvider" 
							};
			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var authResult = service.Authenticate(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.Success, authResult.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_authenticate_with_mock_provider_returning_InvalidAccount()
		{
			var config = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.InvalidUserIdentifier,
				providerName = "MockAuthProvider"
			};
			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var authResult = service.Authenticate(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.InvalidUserIdentifier, authResult.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_return_no_providers_for_authenticate_if_no_providers_are_registered()
		{
			var manager = createProviderManagerMock();

			var service = new AuthenticationService(manager.Object);
			var authResult = service.Authenticate(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.NoRegisteredProviders, authResult.AuthenticationResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_return_provider_exception_for_authenticate_if_provider_throws_exception()
		{
			var exceptionMessage = "Provider threw an exception.";
			var config = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.ProviderException,
				exceptionMessage = exceptionMessage,
				providerName = "MockAuthProvider"
			};
			
			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var authResult = service.Authenticate(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.ProviderException, authResult.AuthenticationResultTypeID);
			Assert.AreEqual((int)AuthenticationResultType.ProviderException, authResult.ProviderResponseMessages.ElementAt(0).AuthenticationResultTypeID);
			Assert.AreEqual(exceptionMessage, authResult.ProviderResponseMessages.ElementAt(0).AuthenticationException.Message);
		}

		[TestMethod]
		public void AuthenticationService_should_authenticate_even_if_first_provider_throws_exception()
		{
			var exceptionMessage = "Provider threw an exception.";
			
			var config1 = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.ProviderException,
				exceptionMessage = exceptionMessage,
				providerName = "MockAuthProvider1"
			};

			var config2 = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.Success,
				providerName = "MockAuthProvider2"
			};

			var manager = createProviderManagerMock(new authConfig[] { config1, config2 } );

			var service = new AuthenticationService(manager.Object);
			var authResult = service.Authenticate(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.Success, authResult.AuthenticationResultTypeID);
			Assert.AreEqual(2, authResult.ProviderResponseMessages.Count());

			Assert.AreEqual((int)AuthenticationResultType.ProviderException, authResult.ProviderResponseMessages.ElementAt(0).AuthenticationResultTypeID);
			Assert.AreEqual(config1.providerName, authResult.ProviderResponseMessages.ElementAt(0).ProviderName);
			Assert.AreEqual(exceptionMessage, authResult.ProviderResponseMessages.ElementAt(0).AuthenticationException.Message);

			Assert.AreEqual((int)AuthenticationResultType.Success, authResult.ProviderResponseMessages.ElementAt(1).AuthenticationResultTypeID);
			Assert.AreEqual(config2.providerName, authResult.ProviderResponseMessages.ElementAt(1).ProviderName);
		}

		[TestMethod]
		public void AuthenticationService_should_retrievepassword_with_mock_provider_returning_Success()
		{
			var config = new authConfig()
			{
				desiredPasswordRetrieveResult = PasswordRetrievalResultType.Success,
				providerName = "MockAuthProvider"
			};
			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var passwordRetrievalResult = service.RetrieveAccount(createCredentials().Object);
			Assert.AreEqual((int)PasswordRetrievalResultType.Success, passwordRetrievalResult.PasswordRetrievalResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_retrievepassword_with_mock_provider_returning_InvalidAccount()
		{
			var config = new authConfig()
			{
				desiredPasswordRetrieveResult = PasswordRetrievalResultType.InvalidUserIdentifier,
				providerName = "MockAuthProvider"
			};
			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var passwordRetrievalResult = service.RetrieveAccount(createCredentials().Object);
			Assert.AreEqual((int)PasswordRetrievalResultType.InvalidUserIdentifier, passwordRetrievalResult.PasswordRetrievalResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_return_no_providers_for_retrievepassword_if_no_providers_are_registered()
		{
			var manager = createProviderManagerMock();

			var service = new AuthenticationService(manager.Object);
			var passwordRetrievalResult = service.RetrieveAccount(createCredentials().Object);
			Assert.AreEqual((int)AuthenticationResultType.NoRegisteredProviders, passwordRetrievalResult.PasswordRetrievalResultTypeID);
		}

		[TestMethod]
		public void AuthenticationService_should_return_provider_exception_for_retrievepassword_if_provider_throws_exception()
		{
			var exceptionMessage = "Provider threw an exception.";
			var config = new authConfig()
			{
				desiredPasswordRetrieveResult = PasswordRetrievalResultType.ProviderException,
				exceptionMessage = exceptionMessage,
				providerName = "MockAuthProvider"
			};

			var manager = createProviderManagerMock(config);

			var service = new AuthenticationService(manager.Object);
			var passwordRetrievalResult = service.RetrieveAccount(createCredentials().Object);
			Assert.AreEqual((int)PasswordRetrievalResultType.ProviderException, passwordRetrievalResult.PasswordRetrievalResultTypeID);
			Assert.AreEqual((int)PasswordRetrievalResultType.ProviderException, passwordRetrievalResult.ProviderResponseMessages.ElementAt(0).PasswordRetrievalResultTypeID);
			Assert.AreEqual(exceptionMessage, passwordRetrievalResult.ProviderResponseMessages.ElementAt(0).PasswordRetrievalException.Message);
		}

		[TestMethod]
		public void AuthenticationService_should_retrievepassword_even_if_first_provider_throws_exception()
		{
			var exceptionMessage = "Provider threw an exception.";

			var config1 = new authConfig()
			{
				desiredPasswordRetrieveResult = PasswordRetrievalResultType.ProviderException,
				exceptionMessage = exceptionMessage,
				providerName = "MockAuthProvider1"
			};

			var config2 = new authConfig()
			{
				desiredPasswordRetrieveResult = PasswordRetrievalResultType.Success,
				providerName = "MockAuthProvider2"
			};

			var manager = createProviderManagerMock(new authConfig[] { config1, config2 });

			var service = new AuthenticationService(manager.Object);
			var passwordRetrievalResult = service.RetrieveAccount(createCredentials().Object);
			Assert.AreEqual((int)PasswordRetrievalResultType.Success, passwordRetrievalResult.PasswordRetrievalResultTypeID);
			Assert.AreEqual(2, passwordRetrievalResult.ProviderResponseMessages.Count());

			Assert.AreEqual((int)PasswordRetrievalResultType.ProviderException, passwordRetrievalResult.ProviderResponseMessages.ElementAt(0).PasswordRetrievalResultTypeID);
			Assert.AreEqual(config1.providerName, passwordRetrievalResult.ProviderResponseMessages.ElementAt(0).ProviderName);
			Assert.AreEqual(exceptionMessage, passwordRetrievalResult.ProviderResponseMessages.ElementAt(0).PasswordRetrievalException.Message);

			Assert.AreEqual((int)PasswordRetrievalResultType.Success, passwordRetrievalResult.ProviderResponseMessages.ElementAt(1).PasswordRetrievalResultTypeID);
			Assert.AreEqual(config2.providerName, passwordRetrievalResult.ProviderResponseMessages.ElementAt(1).ProviderName);
		}

		[TestMethod]
		public void AuthenticationService_should_retrieve_authentication_configuration()
		{
			var exceptionMessage = "Provider threw an exception.";

			var config1 = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.ProviderException,
				exceptionMessage = exceptionMessage,
				providerName = "MockAuthProvider1"
			};

			var config2 = new authConfig()
			{
				desiredAuthResult = AuthenticationResultType.Success,
				providerName = "MockAuthProvider2"
			};

			var manager = createProviderManagerMock(new authConfig[] { config1, config2 });

			var service = new AuthenticationService(manager.Object);

			var registrationConfiguration = service.GetAuthenticationConfiguration();
			Assert.IsNotNull(registrationConfiguration);
			Assert.IsNotNull(registrationConfiguration.RegisteredProviders);
			Assert.AreEqual(2, registrationConfiguration.RegisteredProviders.Count());
			Assert.AreEqual("MockAuthProvider1", registrationConfiguration.RegisteredProviders.ElementAt(0));
			Assert.AreEqual("MockAuthProvider2", registrationConfiguration.RegisteredProviders.ElementAt(1));
		}
	}
}
