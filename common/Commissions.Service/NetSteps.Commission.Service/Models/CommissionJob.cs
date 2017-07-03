using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICommissionJob), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CommissionJob : ICommissionJob
	{
		public int CommissionRunId { get; set; }

		public CommissionRunKind CommissionRunType { get; set; }

		public DisbursementFrequencyKind DisbursementFrequency { get; set; }

		public string JobDisplayName { get; set; }

		public string JobName { get; set; }
	}
}
