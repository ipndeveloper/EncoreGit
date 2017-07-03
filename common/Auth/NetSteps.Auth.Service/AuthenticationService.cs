using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Configuration;
using NetSteps.Auth.Common.Model;
using NetSteps.Auth.Service.Model.Concrete;

namespace NetSteps.Auth.Service
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IAuthenticationProviderManager _providerManager;
		
		#region Constructor
		
		public AuthenticationService(IAuthenticationProviderManager providerManager)
		{
			_providerManager = providerManager;
		}

		#endregion
		
		public IAuthenticationResult Authenticate(ICredentials creds)
		{
			var result = new AuthenticationResult();
			var providers = _providerManager.GetProviders();
			if (!providers.Any())
			{
				result.AuthenticationResultTypeID = (int)AuthenticationResultType.NoRegisteredProviders;
			}
			else
			{
				foreach (var provider in providers)
				{
					try
					{
						var providerResult = provider.Authenticate(creds);
						result.AddProviderAuthenticationResult(providerResult);
						result.AuthenticationResultTypeID = providerResult.AuthenticationResultTypeID;

						if (providerResult.AuthenticationResultTypeID == (int)AuthenticationResultType.Success)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						result.AuthenticationResultTypeID = (int)AuthenticationResultType.ProviderException;
						result.AddProviderAuthenticationResult(new ProviderAuthenticationResult() { ProviderName = provider.GetProviderName(), AuthenticationException = ex, AuthenticationResultTypeID = (int)AuthenticationResultType.ProviderException });
					}
				}
			}
			return result;
		}

		public IPasswordRetrievalResult RetrieveAccount(IPartialCredentials creds)
		{
            if (!_providerManager.AdminSettings[DefaultAdminSettingKinds.EnableForgotPassword])
			{
				throw new Exception("Password retrieval options are not enabled.  Please enable options in the configuration settings for the authentication module.");
			}
			var result = new PasswordRetrievalResult();
			var providers = _providerManager.GetProviders();
			if (!providers.Any())
			{
				result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.NoRegisteredProviders;
			}
			else
			{
				
				foreach (var provider in providers)
				{
					try
					{
						var providerResult = provider.RetrieveAccount(creds);
						result.AddProviderPasswordRetrievalResult(providerResult);
						result.PasswordRetrievalResultTypeID = providerResult.PasswordRetrievalResultTypeID;

						if (providerResult.PasswordRetrievalResultTypeID == (int)PasswordRetrievalResultType.Success)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.ProviderException;
						result.AddProviderPasswordRetrievalResult(new ProviderPasswordRetrievalResult() { ProviderName = provider.GetProviderName(), PasswordRetrievalException = ex, PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.ProviderException });
					}
				}
			}
			return result;
		}

        public IPasswordRetrievalResult RetrieveAccount_(IPartialCredentials creds)
        {
            if (!_providerManager.AdminSettings[DefaultAdminSettingKinds.EnableForgotPassword])
            {
                throw new Exception("Password retrieval options are not enabled.  Please enable options in the configuration settings for the authentication module.");
            }
            var result = new PasswordRetrievalResult();
            var providers = _providerManager.GetProviders();
            if (!providers.Any())
            {
                result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.NoRegisteredProviders;
            }
            else
            {

                foreach (var provider in providers)
                {
                    try
                    {
                        var providerResult = provider.RetrieveAccount_(creds);
                        result.AddProviderPasswordRetrievalResult(providerResult);
                        result.PasswordRetrievalResultTypeID = providerResult.PasswordRetrievalResultTypeID;

                        if (providerResult.PasswordRetrievalResultTypeID == (int)PasswordRetrievalResultType.Success)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.ProviderException;
                        result.AddProviderPasswordRetrievalResult(new ProviderPasswordRetrievalResult() { ProviderName = provider.GetProviderName(), PasswordRetrievalException = ex, PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.ProviderException });
                    }
                }
            }
            return result;
        }

		public IAuthenticationConfiguration GetAuthenticationConfiguration()
		{
			var config = new NetSteps.Auth.Service.Model.Concrete.AuthenticationConfiguration();
			config.RegisteredProviders = _providerManager.GetRegisteredProviderNames();
            config.AdminSettings = _providerManager.AdminSettings;
			return config;
		}


        public IAuthenticationResult Authenticate(IAuthenticationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
