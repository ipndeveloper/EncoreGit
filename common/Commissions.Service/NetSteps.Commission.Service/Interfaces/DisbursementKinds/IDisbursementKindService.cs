using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementKinds
{
	public interface IDisbursementKindService
	{
		IDisbursementKind GetDisbursementKind(Common.Models.DisbursementMethodKind method);

        string GetDisbursementMethodCode(int disbursementMethodId);
    }
}
