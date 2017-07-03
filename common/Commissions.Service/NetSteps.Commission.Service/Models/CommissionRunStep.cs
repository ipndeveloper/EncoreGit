using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICommissionRunStep), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CommissionRunStep : ICommissionRunStep
	{
		public string CommissionProcedureName { get; set; }

		public int CommissionRunId { get; set; }

		public int CommissionRunProcedureId { get; set; }

		public int CommissionRunStepId { get; set; }

		public int CommissionRunStepNumber { get; set; }

		public string DisplayMessage { get; set; }

		public bool Enabled { get; set; }

		public bool IncludePeriodIdParameter { get; set; }

		public int SQLTimeOutSeconds { get; set; }

		public int SortOrder { get; set; }

		public ISystemEventLog SystemEventLog { get; set; }
	}
}
