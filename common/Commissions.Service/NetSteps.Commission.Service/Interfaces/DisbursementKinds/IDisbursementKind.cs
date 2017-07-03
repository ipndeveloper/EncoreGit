using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementKinds
{
	public interface IDisbursementKind
	{
		int DisbursementKindId { get; set; }
		string Name { get; set; }
		int NumberAllowed { get; set; }
		bool IsEnabled { get; set; }
		bool IsEditable { get; set; }
		string TermName { get; set; }
		string Code { get; set; }
		DateTime DateModified { get; set; }
	}
}
