using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.Service.Model.Concrete
{
	[ContainerRegister(typeof(IPasswordRetrievalResult), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PasswordRetrievalResult : IPasswordRetrievalResult
	{
		private List<IProviderPasswordRetrievalResult> _messages;

		public PasswordRetrievalResult()
		{
			_messages = new List<IProviderPasswordRetrievalResult>();
		}
		public IEnumerable<IProviderPasswordRetrievalResult> ProviderResponseMessages { get { return _messages; } }

		public int PasswordRetrievalResultTypeID { get; internal set; }

		public string Message { get; internal set; }

		public void AddProviderPasswordRetrievalResult(IProviderPasswordRetrievalResult result)
		{
			_messages.Add(result);
		}
	}
}
