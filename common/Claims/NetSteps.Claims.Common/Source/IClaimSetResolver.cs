using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IClaimSetResolver
	{
		ClaimSet ResolveClaimSet(string userName, int claimSetId, int claims);
		ClaimSet ResolveClaimSet(string userName, string claimSetName, IEnumerable<string> claims);
	}
}
