using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IRealmApplication
	{
		IRealm Realm
		{
			get;
			set;
		}

		IApplication Application
		{
			get;
			set;
		}
	}
}
