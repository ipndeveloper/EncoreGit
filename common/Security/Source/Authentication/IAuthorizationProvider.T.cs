using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Security.Authentication
{
	[ContractClass(typeof(Contracts.DummyAuthorizationProvider<>))]
	public interface IAuthorizationProvider<T> : IAuthorizationProvider
	{
		IAuthorizationResult CheckAuthorization(T context);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAuthorizationProvider<>))]
		internal abstract class DummyAuthorizationProvider<T> : IAuthorizationProvider<T>
		{
			public IAuthorizationResult CheckAuthorization(object context)
			{
				throw new NotImplementedException();
			}

			public IAuthorizationResult CheckAuthorization(T context)
			{
				Contract.Requires<ArgumentNullException>(context != null, "The context object cannot be null.");
				Contract.Ensures(Contract.Result<IAuthorizationResult>() != null);

				throw new NotImplementedException();
			}
		} 
	}
}
