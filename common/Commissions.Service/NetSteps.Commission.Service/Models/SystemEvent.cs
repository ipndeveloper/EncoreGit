using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ISystemEvent), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class SystemEvent : ISystemEvent
	{
		public bool Completed { get; set; }

		public DateTime CreatedDate { get; set; }
		public int Duration { get; set; }

		public DateTime EndTime { get; set; }

		public int PeriodId { get; set; }

		public DateTime StartTime { get; set; }

		public int SystemEventApplicationId { get; set; }
		public int SystemEventId { get; set; }
	}
}
