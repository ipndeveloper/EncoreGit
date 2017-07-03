using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
	public class TitleKind : ITitleKind
	{
		public string Name { get; set; }

		public string TermName { get; set; }

		public string TitleKindCode { get; set; }

		public int TitleKindId { get; set; }
	}
}
