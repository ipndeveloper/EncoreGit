using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    public interface IRegisteredClaim
    {
        int Id { get; set; }

        string Name { get; set; }

		IRegisteredClaimSet ClaimSet { get; set; }
    }
}
