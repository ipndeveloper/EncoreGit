using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICommissionRunPlan), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CommissionRunPlan : ICommissionRunPlan
	{
		public bool DefaultPlan { get; set; }

		public bool Enabled { get; set; }

		public string Name { get; set; }

		public string PlanCode { get; set; }

		public int PlanId { get; set; }

		public CommissionRunKind RunKind { get; set; }

		public string RunName { get; set; }

		public string RunTypeName { get; set; }

		public string TermName { get; set; }
	}
}
