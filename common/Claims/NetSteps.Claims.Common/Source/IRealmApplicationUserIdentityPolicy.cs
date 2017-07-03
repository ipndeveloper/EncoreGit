using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IRealmApplicationUserIdentityPolicy
	{
		IRealmApplication RealmApplication
		{
			get;
			set;
		}

		IUserIdentityPolicy UserIdentityPolicy
		{
			get;
			set;
		}
	}
}
