using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Auth.Common.Model
{
	[DTO]
	public interface IPasswordRetrievalResult
	{
		int PasswordRetrievalResultTypeID { get; }
		IEnumerable<IProviderPasswordRetrievalResult> ProviderResponseMessages { get; }
		void AddProviderPasswordRetrievalResult(IProviderPasswordRetrievalResult result);
	}
}
