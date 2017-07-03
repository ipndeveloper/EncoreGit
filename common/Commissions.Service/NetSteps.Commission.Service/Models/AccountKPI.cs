using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IAccountKPI), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class AccountKPI : IAccountKPI
	{
		public string DataType { get; set; }

		public string KPITypeCode { get; set; }

		public string KPIValue { get; set; }

		public string TermName { get; set; }
	}
}
