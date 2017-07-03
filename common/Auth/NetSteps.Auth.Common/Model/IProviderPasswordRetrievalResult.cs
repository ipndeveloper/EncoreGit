using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
	public interface IProviderPasswordRetrievalResult
	{
		string ProviderName { get; }
		int PasswordRetrievalResultTypeID { get; }
		Exception PasswordRetrievalException { get; }
		string Message { get; }
	}
}
