using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Service.Model.Concrete
{
	public class ProviderPasswordRetrievalResult : IProviderPasswordRetrievalResult
	{
		public string ProviderName { get; internal set; }

		public int PasswordRetrievalResultTypeID { get; internal set; }

		public Exception PasswordRetrievalException { get; internal set; }

		public string Message { get; internal set; }
	}
}
