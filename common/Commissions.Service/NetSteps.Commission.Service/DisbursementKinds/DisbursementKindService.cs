using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.DisbursementKinds
{
	public class DisbursementKindService : IDisbursementKindService
	{
		protected readonly IDisbursementKindProvider DisbursementKindProvider;

		public DisbursementKindService(IDisbursementKindProvider disbursementKindProvider)
		{
			DisbursementKindProvider = disbursementKindProvider;
		}

		public IDisbursementKind GetDisbursementKind(DisbursementMethodKind method)
		{
			return DisbursementKindProvider.GetDisbursementKind(method);
		}


        public string GetDisbursementMethodCode(int disbursementMethodId)
        {
            return DisbursementKindProvider.GetDisbursementMethodCode(disbursementMethodId);
        }
    }
}
