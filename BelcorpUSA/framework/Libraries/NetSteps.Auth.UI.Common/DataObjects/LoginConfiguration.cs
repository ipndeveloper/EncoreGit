using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.UI.Common.DataObjects
{
    [ContainerRegister(typeof (ILoginConfiguration), RegistrationBehaviors.Default)]
    public class LoginConfiguration : ILoginConfiguration
    {
        public int LoginCredentialType { get; set; }

        public bool EnableForgotPassword { get; set; }

        public bool ShowUsernameFormFields { get; set; }

		public int PrimaryCredentialType { get; set; }
	}
}
