using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Integrations.Service.Entities
{
	public class KitItemValuation
	{
		public int KitItemValuationId { get; set; }
		public string ParentSku { get; set; }
		public string ChildSku { get; set; }
		public decimal ParticipationPercentage { get; set; }
	}
}
