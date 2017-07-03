using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ISystemEventLog), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class SystemEventLog : ISystemEventLog
	{
		public DateTime CreatedDate { get; set; }

		public string EventMessage { get; set; }

		public int SystemEventApplicationId { get; set; }

		public int SystemEventLogId { get; set; }

		public int SystemEventLogTypeId { get; set; }
	}
}
