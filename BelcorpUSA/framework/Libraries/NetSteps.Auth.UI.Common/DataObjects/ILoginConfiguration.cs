
namespace NetSteps.Auth.UI.Common.DataObjects
{
	public interface ILoginConfiguration
	{
		int PrimaryCredentialType { get; set; }
		bool EnableForgotPassword { get; set; }
        bool ShowUsernameFormFields { get; set; }
	}
}
