using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ILedgerEntryKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class LedgerEntryKind : ILedgerEntryKind
	{

		public string Code { get; set; }

		public bool IsEditable { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsTaxable { get; set; }

		public int LedgerEntryKindId { get; set; }

		public string Name { get; set; }

		public string TermName { get; set; }
	}
}
