using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.UI.Common.DataObjects
{
	[ContainerRegister(typeof(IAuthenticationUIResult), RegistrationBehaviors.Default)]
	public class AuthenticationUIResult : IAuthenticationUIResult
	{
		private bool _wasLoginSuccessful;
		public bool WasLoginSuccessful
		{
			get
			{
				return _wasLoginSuccessful;
			}
			set
			{
				_wasLoginSuccessful = value;
			}
		}

		private string _failureMessage;
		public string FailureMessage
		{
			get
			{
				return _failureMessage;
			}
			set
			{
				if (value.GetType() == typeof(string))
				{
					_failureMessage = value;
				}
			}
		}

		private int _credentialTypeID;
		public int CredentialTypeID
		{
			get
			{
				return _credentialTypeID;
			}
			set
			{
				_credentialTypeID = value;
			}
		}
	}
}
