using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    public interface IRegisteredClaimSet
    {
        string Name { get; set; }

        int Id { get; set; }

        IEnumerable<IRegisteredClaim> Claims { get; set; }

		IApplication Application
		{
			get;
			set;
		}

		string ClaimSetResolutionUrl
		{
			get;
			set;
		}
    }
}
