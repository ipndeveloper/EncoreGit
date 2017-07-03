using NetSteps.Auth.UI.Common.DataObjects;
using System;

namespace NetSteps.Auth.UI.Common
{
	public interface IAuthenticationUIService
	{
		IAuthenticationUIResult Authenticate(string username, string password, int siteId);

		IForgotPasswordUIResult ForgotPassword(string username, int siteId);

        IForgotPasswordUIResult ForgotPassword_(string username, string CFP, DateTime BirthDay, int siteId);

		ILoginConfiguration GetConfiguration();
	}
}
