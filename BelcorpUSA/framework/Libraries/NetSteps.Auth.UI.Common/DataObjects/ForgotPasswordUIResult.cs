using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.UI.Common.DataObjects
{
	[ContainerRegister(typeof(IForgotPasswordUIResult), RegistrationBehaviors.Default)]
	public class ForgotPasswordUIResult : IForgotPasswordUIResult
	{
		private bool _wasSuccessful;
		public bool WasSuccessful
		{
			get
			{
				return _wasSuccessful;
			}
			set
			{
				_wasSuccessful = value;
			}
		}

		private string _message;
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
			}
		}
	}
}
