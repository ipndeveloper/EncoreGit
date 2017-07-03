using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ISystemEventSetting), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class SystemEventSetting : ISystemEventSetting
	{
		public int PrepSystemEventApplicationId { get; set; }

		public int PublishSystemEventApplicationId { get; set; }

		public int SystemEventLogErrorTypeId { get; set; }

		public int SystemEventLogNoticeTypeId { get; set; }
	}
}
