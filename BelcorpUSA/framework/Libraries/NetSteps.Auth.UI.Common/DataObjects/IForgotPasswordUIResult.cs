
namespace NetSteps.Auth.UI.Common.DataObjects
{
	public interface IForgotPasswordUIResult
	{
		bool WasSuccessful { get; set; }
		string Message { get; set; }
	}
}
