using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    public interface IApplication
    {
        string Name
        {
            get;
            set;
        }

        IEnumerable<IRealm> Realms
        {
            get;
            set;
		}

		IEnumerable<IRegisteredClaimSet> ClaimSets
		{
			get;
			set;
		}

		IUserIdentity AdministrativeUser
		{
			get;
			set;
		}
    }
}
