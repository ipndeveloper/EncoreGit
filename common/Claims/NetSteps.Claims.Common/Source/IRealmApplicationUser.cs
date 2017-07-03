using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IRealmApplicationUser
	{
		IRealmApplication RealmApplication { get; set; }

		NetSteps.Claims.Common.IUserIdentity Identity { get; set; }
	}
}
