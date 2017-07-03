using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ILedgerEntryOrigin), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class LedgerEntryOrigin : ILedgerEntryOrigin
	{
		public int EntryOriginId { get; set; }

		public string Code { get; set; }

		public bool IsEnabled { get; set; }

		public string Name { get; set; }

		public string TermName { get; set; }


		public bool IsEditable { get; set; }
	}
}
