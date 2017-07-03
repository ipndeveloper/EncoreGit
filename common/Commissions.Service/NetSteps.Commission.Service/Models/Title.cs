using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ITitle), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class Title : ITitle
	{
		public bool Active { get; set; }

		public int SortOrder { get; set; }

		public string TermName { get; set; }

		public string TitleCode { get; set; }

		public int TitleId { get; set; }

		public string TitleName { get; set; }


		public string ClientCode { get; set; }

		public string ClientName { get; set; }

		public bool ReportsVisibility { get; set; }
	}
}
