using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IBonusKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class BonusKind : IBonusKind
	{
		public string BonusCode { get; set; }

		public string BonusDescription { get; set; }

		public int BonusKindId { get; set; }

		public int PlanId  { get; set; }

		public int? EarningsKindId { get; set; }

		public bool IsEditable { get; set; }

		public bool IsEnabled { get; set; }

		public string Name { get; set; }

		public string TermName { get; set; }


		public string ClientCode { get; set; }

		public string ClientName { get; set; }
	}
}
