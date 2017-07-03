using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.DisbursementKinds
{
	[ContainerRegister(typeof(IDisbursementKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DisbursementKind : IDisbursementKind
	{
		public int DisbursementKindId { get; set; }

		public string Name { get; set; }

		public int NumberAllowed { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsEditable { get; set; }

		public string TermName { get; set; }

		public string Code { get; set; }

		public DateTime DateModified { get; set; }
	}
}
