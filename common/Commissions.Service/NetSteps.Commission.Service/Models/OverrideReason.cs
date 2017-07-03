using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
	public class OverrideReason : IOverrideReason
	{
		public string Name { get; set; }

		public int OverrideReasonId { get; set; }

		public IOverrideReasonSource OverrideReasonSource { get; set; }

		public string ReasonCode { get; set; }

		public string TermName { get; set; }
	}
}
