using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Security.Authentication
{
	public abstract class AbstractAuthorizationProvider<T> : IAuthorizationProvider<T>
	{
		public IAuthorizationResult CheckAuthorization(T context)
		{
			return DoCheckAuthorization(context);
		}

		public IAuthorizationResult CheckAuthorization(object context)
		{
			IAuthorizationResult result = null;

			if (context is T)
				result = CheckAuthorization((T)context);
			else
			{
				result = Create.New<IAuthorizationResult>();
				result.IsAuthorzied = true;
			}

			return result;
		}

		protected abstract IAuthorizationResult DoCheckAuthorization(T context);

	}
}
