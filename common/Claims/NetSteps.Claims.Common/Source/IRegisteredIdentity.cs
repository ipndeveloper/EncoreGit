using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    public interface IRegisteredIdentity
    {
        string Name
        {
            get;
            set;
		}

		IdentityKind Kind
		{
			get;
			set;
		}
    }
}
