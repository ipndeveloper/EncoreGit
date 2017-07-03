using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
	public class OverrideReasonSource : IOverrideReasonSource
	{
		public string Code { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public int OverrideReasonSourceId { get; set; }

		public string TermName { get; set; }
	}
}
