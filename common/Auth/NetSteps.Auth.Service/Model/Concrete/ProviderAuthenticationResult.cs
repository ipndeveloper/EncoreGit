using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Service.Model.Concrete
{
	public class ProviderAuthenticationResult : IProviderAuthenticationResult
	{
		public string ProviderName { get; internal set; }

		public int AuthenticationResultTypeID { get; internal set; }

		public Exception AuthenticationException { get; internal set; }

		public string Message { get; internal set; }

		public int UserID { get; internal set; }
	}
}
