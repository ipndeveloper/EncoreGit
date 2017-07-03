using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICommissionPlan), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CommissionPlan : ICommissionPlan
	{
		public DisbursementFrequencyKind DisbursementFrequency { get; set; }

		public bool IsDefault { get; set; }

		public bool IsEnabled { get; set; }

		public string Name { get; set; }

		public string PlanCode { get; set; }

		public string TermName { get; set; }

		public int CommissionPlanId { get; set; }
	}
}
