using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Service.Model.Concrete
{
	[ContainerRegister(typeof(IAuthenticationResult), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AuthenticationResult : IAuthenticationResult
	{
		private List<IProviderAuthenticationResult> _messages;

		public AuthenticationResult()
		{
			_messages = new List<IProviderAuthenticationResult>();
		}
		public IEnumerable<IProviderAuthenticationResult> ProviderResponseMessages { get { return _messages; } }

		public int AuthenticationResultTypeID { get; internal set; }

		public string Message { get; internal set; }

		public void AddProviderAuthenticationResult(IProviderAuthenticationResult result)
		{
			_messages.Add(result);
		}
	}
}
