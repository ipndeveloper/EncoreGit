using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IDistributorPeriodPerformanceData), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class DistributorPeriodPerformanceData : IDistributorPeriodPerformanceData
	{
		public int AccountId { get; set; }

		public ITitle CurrentTitle { get; set; }
		public ITitle PaidAsTitle { get; set; }
		public int PeriodId { get; set; }
		public decimal RequiredVolume { get; set; }
		public string SalesIndicatorLevel { get; set; }
		public decimal Volume { get; set; }
	}
}
