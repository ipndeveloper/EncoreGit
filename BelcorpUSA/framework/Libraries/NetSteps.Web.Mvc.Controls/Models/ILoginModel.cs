using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Models;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public interface ILoginModel : IDynamicViewModel
	{
		string SignUpUrl { get; set; }
		string LoginUrl { get; set; }
		bool EnableForgotPassword { get; set; }
		bool DisplayUsernameField { get; set; }
		bool EnableAnonymousLogin { get; set; }
		string UsernameLabelText { get; set; }
		string UsernameErrorText { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
	}

	[ContainerRegister(typeof(ILoginModel), RegistrationBehaviors.Default)]
	public class LoginModel : DynamicViewModel, ILoginModel
	{
		public bool EnableAnonymousLogin { get; set; }
		public virtual string UsernameLabelText { get; set; }
		public virtual string UsernameErrorText { get; set; }
		public string SignUpUrl { get; set; }
		public virtual string LoginUrl { get; set; }
		public virtual bool EnableForgotPassword { get; set; }
		public bool DisplayUsernameField { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
	}
}
