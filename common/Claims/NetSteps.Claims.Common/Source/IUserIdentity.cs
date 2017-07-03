using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IUserIdentity : IRegisteredIdentity
	{
		string Tenant
		{
			get;
			set;
		}
	}
}
