using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
	public class OverrideKind : IOverrideKind
	{
		public string Description { get; set; }

		public string Operator { get; set; }

		public string OverrideCode { get; set; }

		public int OverrideKindId { get; set; }
	}
}
