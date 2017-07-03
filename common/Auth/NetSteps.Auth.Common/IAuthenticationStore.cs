
using NetSteps.Auth.Common.Model;
using System;
namespace NetSteps.Auth.Common
{
	public interface IAuthenticationStore
	{
		IUserAuthInfo GetUserAuthInfo(string userIdentifier, AuthenticationStoreField field);
		bool TriggerPasswordRetrieval(string userIdentifier, int siteID, AuthenticationStoreField field);
        bool TriggerPasswordRetrieval_(string userIdentifier, string CFP, DateTime birthDay, int siteID, AuthenticationStoreField field);
		void RecordLoginResultInfo(IProviderAuthenticationResult result);
	}

	public enum AuthenticationStoreField
	{
		AccountID,
		Username,
		EmailAddress,
		CorporateUsername,
	}
}
