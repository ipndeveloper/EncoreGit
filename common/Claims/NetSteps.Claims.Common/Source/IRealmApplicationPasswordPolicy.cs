using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IRealmApplicationPasswordPolicy
	{
		IRealmApplication RealmApplication { get; set; }

		IPasswordPolicy PasswordPolicy { get; set; }
	}
}
