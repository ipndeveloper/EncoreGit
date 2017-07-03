using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICommissionRun), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CommissionRun : ICommissionRun
	{

		public bool CommissionRunCanBePublished { get; set; }

		public int CommissionRunId { get; set; }

		public CommissionRunKind CommissionRunKind { get; set; }

		public int CommissionRunKindId { get; set; }

		public string CommissionRunName { get; set; }

		public IEnumerable<ICommissionRunPlan> CommissionRunPlans { get; set; }

		public int CommissionRunProgress { get; set; }

		public int CommissionRunStepNumber { get; set; }

		public List<ICommissionRunStep> CommissionRunSteps { get; set; }

		public string CurrentCommissionRunProgress { get; set; }

		public ICommissionRunStep CurrentCommissionRunStep { get; set; }

		public DisbursementFrequencyKind DefaultCommissionRunPlan { get; set; }

		public bool Enabled { get; set; }

		public bool InProgress { get; set; }

		public int LastSystemEventLogId { get; set; }

		public ICommissionRunStep NextCommissionRunStep { get; set; }

		public IEnumerable<IPeriod> OpenPeriods { get; set; }

		public int PeriodId { get; set; }

		public int PlanId { get; set; }

		public int SystemEventId { get; set; }

		public ISystemEventLog SystemEventLog { get; set; }

		public ISystemEventSetting SystemEventSetting { get; set; }

		public int TotalCommissionRunProgress { get; set; }

		public int TotalCommissionRunSteps { get; set; }

		public string ToJson()
		{
			return String.Empty;
		}
	}
}
