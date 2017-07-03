
namespace NetSteps.Auth.UI.Common.DataObjects
{
	public interface IAuthenticationUIResult
	{
		bool WasLoginSuccessful { get; set; }
		string FailureMessage { get; set; }
		int CredentialTypeID { get; set; }
	}
}
