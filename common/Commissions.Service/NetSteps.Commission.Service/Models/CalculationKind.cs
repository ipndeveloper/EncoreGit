using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICalculationKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CalculationKind : ICalculationKind
	{
		public int CalculationKindId { get; set; }

		public string ClientCode { get; set; }

		public string ClientName { get; set; }

		public string Code { get; set; }

		public DateTime DateModified { get; set; }

		public bool IsRealTime { get; set; }

		public bool IsUserOverridable { get; set; }

		public string Name { get; set; }

		public bool? ReportVisibility { get; set; }

		public string TermName { get; set; }
	}
}
